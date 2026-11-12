using Cairn.Infrastructure.APIs.YahooFinance.Responses;
using Cairn.Application.Interfaces.APIs;
using Cairn.Domain.Enums;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using YahooFinanceApi;
using Cairn.Application.DTO.YahooFinance;

namespace Cairn.Infrastructure.APIs.YahooFinance
{
    public class YahooFinanceApiService : IYahooFinanceApiService   
    {
        private readonly YahooFinanceApiOptions _options;
        private readonly HttpClient _httpClient;

        public YahooFinanceApiService(IOptions<YahooFinanceApiOptions> options, HttpClient httpClient)
        {
            _options = options.Value;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }

        private static readonly HashSet<string> TypesAcceptes = new(StringComparer.OrdinalIgnoreCase)
        {
            "EQUITY", "ETF", "MUTUALFUND", "CRYPTOCURRENCY"
        };

        private static readonly string[] SuffixesEuro =
        {
            "-EUR", ".PA", ".DE", ".F", ".AS", ".MI", ".MC", ".BR", ".LS", ".VI", ".HE", ".IR"
        };

        public async Task<Dictionary<string, decimal>> GetPrixActuelAsync(IEnumerable<string> tickers)
        {
            var dictionnairePrix = new Dictionary<string, decimal>();
            
            if (!tickers.Any())
                return dictionnairePrix;

            try
            {
                // On ajoute le taux EUR/USD aux tickers à interroger
                var listeTickers = tickers.ToList();
                if (!listeTickers.Contains("EURUSD=X"))
                    listeTickers.Add("EURUSD=X");


                IReadOnlyDictionary<string, Security> resultats = await Yahoo.Symbols(listeTickers.ToArray()).QueryAsync();

                // Récupération du taux de change EUR -> USD (ex: 1.08)
                decimal tauxEurUsd = 1.0m;
                if (resultats.TryGetValue("EURUSD=X", out var dataFx))
                {
                    tauxEurUsd = (decimal)dataFx.RegularMarketPrice;
                }

                foreach (var ticker in tickers)
                {
                    if (resultats.TryGetValue(ticker, out var data))
                    {
                        decimal prixOriginal = (decimal)data.RegularMarketPrice;
                        string devise = data.Currency; 

                        // Si c'est en USD, on convertit en EUR
                        if (string.Equals(devise, "USD", StringComparison.OrdinalIgnoreCase))
                            prixOriginal = prixOriginal / tauxEurUsd;

                        dictionnairePrix[ticker] = Math.Round(prixOriginal, 2);
                    }
                    else
                    {
                        Console.WriteLine($"AVERTISSEMENT (YahooApi): ticker non trouvé {ticker}");
                        throw new Exception("ERREUR de ticker");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"ERREUR inconnue avec l'API Yahoo Finance, impossible de recuperer le prix des actifs.");
                throw new Exception($"Echec du chargement du patrimoine, connectez-vous à internet pour visualiser votre patrimoine");
            }

            return dictionnairePrix;
        }

        // REVOIR ET REFACTORISER CETTE FONCTION POUR LA RENDRE UTILISABLE
        public async Task<Dictionary<LapsTemps, decimal>> GetPrixHistorique(string ticker)
        {
            var dictionnairePrix = new Dictionary<LapsTemps, decimal>();

            // On demande directement 1 an d'historique (range=1y) avec un intervalle d'un jour (1d)
            string url = $"https://query1.finance.yahoo.com/v8/finance/chart/{ticker}?range=1y&interval=1d";

            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                using var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode) return dictionnairePrix;

                // On parse le gros JSON de Yahoo
                using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                var chart = doc.RootElement.GetProperty("chart");

                if (chart.TryGetProperty("result", out var resultList) && resultList.GetArrayLength() > 0)
                {
                    var firstResult = resultList[0];
                    var historique = new List<(DateTime Date, decimal Price)>();

                    if (firstResult.TryGetProperty("timestamp", out var timestamps))
                    {
                        var indicators = firstResult.GetProperty("indicators");
                        var quote = indicators.GetProperty("quote");

                        if (quote.GetArrayLength() > 0 && quote[0].TryGetProperty("close", out var closes))
                        {
                            for (int i = 0; i < timestamps.GetArrayLength(); i++)
                            {
                                if (i < closes.GetArrayLength() && closes[i].ValueKind != JsonValueKind.Null)
                                {
                                    var dateBougie = DateTimeOffset.FromUnixTimeSeconds(timestamps[i].GetInt64()).DateTime.Date;
                                    historique.Add((dateBougie, closes[i].GetDecimal()));
                                }
                            }
                        }
                    }

                    foreach (LapsTemps periode in Enum.GetValues(typeof(LapsTemps)))
                    {
                        if (periode == LapsTemps.Tout) continue;

                        DateTime dateCible = DateTime.UtcNow.AddDays(-(int)periode);

                        if (historique.Any())
                        {
                            var proche = historique
                                .OrderBy(x => Math.Abs((x.Date - dateCible).TotalDays))
                                .FirstOrDefault();

                            dictionnairePrix[periode] = proche.Price;
                        }
                        else
                        {
                            firstResult.TryGetProperty("meta", out var meta);
                            dictionnairePrix[periode] = meta.TryGetProperty("regularMarketPrice", out var marketPrice) ? marketPrice.GetDecimal() : 0;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur Yahoo Chart pour {ticker}: {ex.Message}");
                // En cas de pépin, on initialise à 0 pour ne pas faire crasher ton dashboard
                foreach (LapsTemps p in Enum.GetValues(typeof(LapsTemps))) dictionnairePrix[p] = 0;
            }

            return dictionnairePrix;
        }

        public async Task<YahooFinanceResultDto?> GetInfoActifYahoo(string? code, string libelle)
        {
            // on essaie d'abord avec l'ISIN, puis libellé + "EUR" (cryptos), puis libellé seul
            var requetes = new List<string>();
            if (!string.IsNullOrWhiteSpace(code))
                requetes.Add(code);
            if (!string.IsNullOrWhiteSpace(libelle))
            {
                requetes.Add($"{libelle} EUR");
                requetes.Add(libelle);
            }

            foreach (var requete in requetes)
            {
                var result = await SearchOnYahoo(requete);
                if (result != null)
                    return result;
            }

            return null;
        }

        private async Task<YahooFinanceResultDto?> SearchOnYahoo(string requete)
        {
            var url = _options.BaseUri + string.Format(_options.SearchEndPoint, Uri.EscapeDataString(requete));
            var reponse = await _httpClient.GetFromJsonAsync<YahooSearchApiResponse>(url);

            return reponse?.Quotes
                .Where(q => q.IsYahooFinance && TypesAcceptes.Contains(q.QuoteType ?? ""))
                .OrderByDescending(q => EstEnEuros(q.Ticker))
                .ThenByDescending(q => q.Score)
                .Select(q => new YahooFinanceResultDto
                {
                   Ticker = q.Ticker,
                   TypeActif = q.QuoteType ?? string.Empty
                })
                .FirstOrDefault();
        }

        private static bool EstEnEuros(string ticker)
        {
            return SuffixesEuro.Any(s => ticker.EndsWith(s, StringComparison.OrdinalIgnoreCase));
        }
    }
}

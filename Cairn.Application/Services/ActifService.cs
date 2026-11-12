using Cairn.Application.DTO.YahooFinance;
using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Domain.Enums;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Services
{
    public class ActifService(ITradeRepublicLogoApiService tradeRepublicLogoApiService,
                              IYahooFinanceApiService yahooFinanceApiService,
                              IActifRepository actifRepository) : IActifService
    {
        private readonly ITradeRepublicLogoApiService _tradeRepublicLogoApiService = tradeRepublicLogoApiService;
        private readonly IActifRepository _actifRepository = actifRepository;
        private readonly IYahooFinanceApiService _yahooFinanceApiService = yahooFinanceApiService;

        private static readonly HashSet<string> _motsInutiles = new(StringComparer.OrdinalIgnoreCase)
        {
            // Format du fonds
            "UCITS", "ETF", "ETC", "ETN", "SICAV", "FCP", "FUND", "TRACKER",
            // Distribution
            "ACC", "DIST", "DIS", "CAPI", "CAP", "ACCUMULATING", "DISTRIBUTING",
            // Classe de parts
            "C", "D", "1C", "1D", "A", "CLASS",
            // Devise et couverture
            "EUR", "USD", "GBP", "CHF", "JPY", "HEDGED", "H",
            // Enveloppe et réplication
            "PEA", "PEA-PME", "SWAP", "SYNTHETIC",
            // Forme juridique
            "SA", "SE", "AG", "NV", "PLC", "INC", "CORP", "LTD", "GROUP",
            // Divers
            "ESG", "SRI", "MONDE"
        };

        private static readonly char[] _ponctuation = ['(', ')', '[', ']', ',', '.', '-', '/', '|'];

        public async Task<int> GetActifId(string? code, string libelle)
        {
            var libelleNettoye = NettoyerLibelle(libelle);

            var actif = await _actifRepository.GetByCodeOuLibelle(code, libelleNettoye);
            if (actif != null)
                return actif.Id;

            var infosActif = await ChercherInfosActif(code, libelleNettoye);

            actif = new Actif
            {
                Libelle = libelleNettoye,
                CodePowens = code,
                Symbole = infosActif?.Ticker,
                Type = DeterminerType(infosActif?.TypeActif, libelle, code),
                LogoSource = await _tradeRepublicLogoApiService.LogoExiste(code) ? LogoSource.TradeRepublic : LogoSource.LogoDev,
                DateRechercheSymbole = DateTime.UtcNow
            };
            await _actifRepository.Add(actif);

            return actif.Id;
        }

        public string NettoyerLibelle(string libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle))
                return string.Empty;

            var motsNettoyes = libelle
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(mot => mot.Trim(_ponctuation))
                .Where(mot => mot.Length > 0 && !_motsInutiles.Contains(mot));

            var resultat = string.Join(" ", motsNettoyes);

            return resultat.Length > 0 ? resultat : libelle.Trim();
        }

        private async Task<YahooFinanceResultDto?> ChercherInfosActif(string? code, string libelle)
        {
            if (code == null)
                return null;

            try
            {
                return await _yahooFinanceApiService.GetInfoActifYahoo(code, libelle);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static TypeActif DeterminerType(string? typeYahoo, string? libelle, string? code)
        {
            if (!string.IsNullOrWhiteSpace(libelle) && libelle.Contains("SCPI", StringComparison.OrdinalIgnoreCase))
                return TypeActif.SCPI;

            return typeYahoo?.ToUpperInvariant() switch
            {
                "EQUITY" => TypeActif.Action,
                "CRYPTOCURRENCY" => TypeActif.Crypto,
                "ETF" => TypeActif.ETF,
                "MUTUALFUND" => TypeActif.Fonds,
                _ => TypeActif.Autre
            };
        }
    }
}

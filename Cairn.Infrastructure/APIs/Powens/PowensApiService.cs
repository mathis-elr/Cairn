using Cairn.Application.DTO.Powens;
using Cairn.Application.Interfaces.APIs;
using Cairn.Infrastructure.APIs.Powens.Responses;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Cairn.Infrastructure.APIs.Powens
{
    public class PowensApiService : IPowensApiService
    {
        private readonly PowensApiOptions _options;
        private readonly HttpClient _httpClient;

        public PowensApiService(IOptions<PowensApiOptions> options,
                                HttpClient httpClient)
        {
            _options = options.Value;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(_options.BaseUri);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public Task<string> GetUrlConnexion(string token) => GetUrlWebview(token, _options.ConnectEndPoint, idConnectionPowens: null);
        public Task<string> GetUrlReconnexion(string token, int idConnectionPowens) => GetUrlWebview(token, _options.ReconnectEndPoint, idConnectionPowens);

        public async Task<string> GenerateCodeTemporaire(string tokenClair)
        {
            var reponse = await RequeteAvecToken(HttpMethod.Get, tokenClair, _options.CodeByTokenEndPoint);
            var reponseString = await reponse.Content.ReadAsStringAsync();
            var codeTemporaire = JsonSerializer.Deserialize<PowensCodeTemporaireApiResponse>(reponseString);

            return codeTemporaire?.Code ?? throw new Exception("Echec de la récupération d'un code temporaire");
        }

        public async Task<UtilisateurImportDto> CreeNouvelUtilisateur(int userId)
        {
            using var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _options.ClientId,
                ["client_secret"] = _options.ClientSecret
            });

            var reponse = await _httpClient.PostAsync(_options.NouvelUtilisateurEndPoint, body);

            await VerifierReponse(reponse);

            var reponseString = await reponse.Content.ReadAsStringAsync();
            var reponseJson = JsonDocument.Parse(reponseString);
            var rootReponse = reponseJson.RootElement;

            var newUser = JsonSerializer.Deserialize<PowensNouvelUtilisateurApiResponse>(reponseString);
            if (newUser?.AuthToken == null) 
                throw new Exception("Echec de la création d'un nouvel utilisateur powens.");

            return new UtilisateurImportDto
            {
                AuthToken = newUser.AuthToken,
                IdUser = newUser.IdUser,
            };
        }

        public async Task<string> GetNomBanque(string token, int idConnector)
        {
            var reponse = await RequeteAvecToken(HttpMethod.Get, token, string.Format(_options.ConnectorEndPoint, idConnector));
            var reponseString = await reponse.Content.ReadAsStringAsync();
            var comptes = JsonSerializer.Deserialize<PowensConnectorApiResponse>(reponseString);

            if (comptes?.NomBanque == null)
                throw new Exception("L'API n'a renvoyé aucun connector pour cette banque.");

            return comptes.NomBanque;
        }

        public async Task<ConnectionImportDto> GetConnection(string token, int connectionBanqueId)
        {
            var reponse = await RequeteAvecToken(HttpMethod.Get, token, string.Format(_options.ConnectionEndPoint, connectionBanqueId));
            var reponseString = await reponse.Content.ReadAsStringAsync();
            var connection = JsonSerializer.Deserialize<PowensConnectionApiResponse>(reponseString);

            if(connection?.IdConnector == null)
                throw new Exception("L'API n'a renvoyé aucune connection pour cet utilisateur.");

            return new ConnectionImportDto
            {
                IdConnector = connection.IdConnector.Value,
                LastUpdate = connection.LastUpdate ?? DateTime.UtcNow,
                PowensState = connection.PowensState ?? string.Empty,
            };
        }

        public async Task<IEnumerable<CompteImportDto>> GetComptes(string token, int connectionBanqueId)
        {
            var url = string.Format(_options.AccountsConnectionEndPoint, connectionBanqueId);

            var reponse = await RequeteAvecToken(HttpMethod.Get, token, url);
            var reponseString = await reponse.Content.ReadAsStringAsync();

            var comptes = JsonSerializer.Deserialize<PowensComptesApiResponse>(reponseString);

            return comptes?.Comptes.Select(c => new CompteImportDto
            {
                Id = c.Id,
                NomCompte = c.NomCompte,
                Type = c.Type,
                Solde = c.Solde,
                Desactive = c.Disabled != null,
            }) ?? [];
        }

        public async Task<List<FluxBancaireImportDto>> GetTransactions(string tokenClair, int idComptePowens, DateTime? depuis)
        {
            var endpoint = string.Format(_options.TransactionsEndPoint, idComptePowens);
            var requete = depuis.HasValue ? $"{endpoint}?min_date={depuis:yyyy-MM-dd}&limit=1000" : $"{endpoint}?limit=1000";

            var flux = new List<FluxBancaireImportDto>();

            // on gère la pagination si jamais y a plus de 1000 transactions
            while (requete != null)
            {
                var reponse = await RequeteAvecToken(HttpMethod.Get, tokenClair, requete);
                var reponseString = await reponse.Content.ReadAsStringAsync();
                var page = JsonSerializer.Deserialize<PowensTransactionsApiResponse>(reponseString);

                flux.AddRange(page?.Transactions?
                    .Select(t => new FluxBancaireImportDto
                    {
                        Id = t.Id,
                        Date = t.Date,
                        Valeur = t.Valeur,
                        Libelle = t.Libelle ?? string.Empty
                    })
                    .ToList() ?? []);

                requete = page?.Links?.Next?.Href;
            }

            return flux;
        }

        public async Task<List<PositionInvestImportDto>> GetPositionInvestissements(string tokenClair, int idComptePowens)
        {
            var reponse = await RequeteAvecToken(HttpMethod.Get, tokenClair, string.Format(_options.InvestEndPoint, idComptePowens));

            var reponseString = await reponse.Content.ReadAsStringAsync();
            var investissements = JsonSerializer.Deserialize<PowensInvestissementsApiResponse>(reponseString);

            return investissements?.PositionsInvest?
                .Select(t => new PositionInvestImportDto
                {
                    IdPowens = t.Id,
                    CodeType = t.CodeType,
                    Code = t.Code,
                    Label = t.Label,
                    Quantite = t.Quantite,
                    PrixAchat = t.PrixAchat,
                    PrixCourant = t.PrixCourant,
                    Valorisation = t.Valorisation,
                    DatePrixCourant = t.DatePrixCourant,
                    DateDerniereMAJ = t.DateDerniereMAJ,
                })
                .ToList() ?? [];
        }

        private async Task<string> GetUrlWebview(string token, string endPoint, int? idConnectionPowens)
        {
            var code = await GenerateCodeTemporaire(token);

            var url = new Uri(new Uri(_options.BaseUri), endPoint);

            var query = $"?client_id={Uri.EscapeDataString(_options.ClientId)}" +
                        $"&redirect_uri={Uri.EscapeDataString(_options.RedirectUri)}" +
                        $"&code={Uri.EscapeDataString(code)}";

            if (idConnectionPowens != null)
                query += $"&connection_id={idConnectionPowens}";

            return $"{url}{query}";
        }

        private async Task<HttpResponseMessage> RequeteAvecToken(HttpMethod type, string token, string requete)
        {
            using var request = new HttpRequestMessage(type, requete);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var reponse = await _httpClient.SendAsync(request);

            await VerifierReponse(reponse);
            return reponse;
        }

        private static async Task VerifierReponse(HttpResponseMessage reponse)
        {
            if (reponse.IsSuccessStatusCode)
                return;

            var codeStatus = (int)reponse.StatusCode;
            var detail = await reponse.Content.ReadAsStringAsync();

            throw codeStatus switch
            {
                401 => new Exception("Token Powens invalide ou expiré."),
                403 => new Exception("Accès refusé par Powens."),
                404 => new Exception("Ressource Powens introuvable (URL ou id incorrect)."),
                >= 500 => new Exception("Erreur du serveur Powens."),
                _ => new Exception($"Erreur Powens {codeStatus} : {detail}")
            };
        }
    }
}
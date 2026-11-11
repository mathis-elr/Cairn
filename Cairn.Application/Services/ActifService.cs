using Cairn.Application.Interfaces.APIs;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Services
{
    public class ActifService(IActifRepository actifRepository,
                              IYahooFinanceApiService yahooFinanceApiService) : IActifService
    {
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

        public async Task<List<Actif>> GetAll()
        {
            return await _actifRepository.GetAll();
        }

        public async Task<int> GetActifId(string? code, string libelle)
        {
            var codeNormalise = string.IsNullOrWhiteSpace(code) ? null : code.Trim().ToUpperInvariant();
            var libelleNettoye = NettoyerLibelle(libelle);

            var actif = await _actifRepository.GetByCodeOuLibelle(codeNormalise, libelleNettoye);
            if (actif != null)
                return actif.Id;

            actif = new Actif
            {
                Libelle = libelleNettoye,
                CodePowens = codeNormalise,
                Symbole = await ChercherSymbole(codeNormalise, libelleNettoye),
                DateRechercheSymbole = DateTime.UtcNow
            };
            await _actifRepository.Add(actif);

            return actif.Id;
        }

        public async Task<IEnumerable<string>> GetTickers()
        {
            return await _actifRepository.GetAllSymboles();
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

        private async Task<string?> ChercherSymbole(string? code, string libelle)
        {
            if (code == null)
                return null;

            try
            {
                return await _yahooFinanceApiService.ChercherTicker(code, libelle);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

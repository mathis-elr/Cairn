using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Services
{
    public class ActifService(IActifRepository actifRepository) : IActifService
    {
        private readonly IActifRepository _actifRepository = actifRepository;

        private readonly List<string> _motsInutiles = ["EUR", "(ACC)", "PEA", "SWAP", "(DIST)","ESG", "USD", "Monde", "UCITS", "ETF", "Acc", "Dist"];

        public async Task<List<Actif>> GetAll()
        {
            return await _actifRepository.GetAll();
        }

        public async Task<IEnumerable<string>> GetTickers()
        {
            return await _actifRepository.GetAllSymboles();
        }

        public string NettoyerLibelle(string libelle)
        {
            if (string.IsNullOrWhiteSpace(libelle))
                return string.Empty;

            var motsNettoyes = libelle.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(mot => !_motsInutiles.Contains(mot, StringComparer.OrdinalIgnoreCase));

            string resultat = string.Join(" ", motsNettoyes);

            return resultat.Trim();
        }

        public async Task<int> AddActif(Actif actif)
        {
            return await _actifRepository.Add(actif);
        }

        public async Task UpdateActif(Actif actif)
        {
            await _actifRepository.Update(actif);
        }
    }
}

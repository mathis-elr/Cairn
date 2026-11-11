using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Services
{
    public class HistoriqueSoldeCompteService(IHistoriqueSoldeCompteRepository historiqueSoldeCompteRepository) : IHistoriqueSoldeCompteService
    {
        private readonly IHistoriqueSoldeCompteRepository _historiqueSoldeCompteRepository = historiqueSoldeCompteRepository;

        public async Task SaveRange(
            IEnumerable<HistoriqueSoldeCompte> historiques)
        {
            var liste = historiques.ToList();

            if (liste.Count == 0)
                return;

            var date = liste[0].Date.Date;

            if (liste.Any(h => h.Date != date))
                throw new ArgumentException("Les historiques doivent concerner une même journée, sans heure.", nameof(historiques));

            var existants = (await _historiqueSoldeCompteRepository.GetByDate(
                date,
                liste.Select(h => h.CompteBanqueId)))
                .ToDictionary(h => h.CompteBanqueId);

            var aAjouter = new List<HistoriqueSoldeCompte>();
            var aModifier = new List<HistoriqueSoldeCompte>();

            foreach (var historique in liste)
            {
                if (!existants.TryGetValue(historique.CompteBanqueId, out var existant))
                {
                    aAjouter.Add(historique);
                    continue;
                }

                var montantInvesti = historique.MontantInvesti ?? existant.MontantInvesti;

                if (existant.Solde == historique.Solde && existant.MontantInvesti == montantInvesti)
                    continue;

                existant.Solde = historique.Solde;
                existant.MontantInvesti = montantInvesti;

                aModifier.Add(existant);
            }

            if (aAjouter.Count > 0)
                await _historiqueSoldeCompteRepository.AddRange(aAjouter);

            if (aModifier.Count > 0)
                await _historiqueSoldeCompteRepository.UpdateRange(aModifier);
        }

    }
}

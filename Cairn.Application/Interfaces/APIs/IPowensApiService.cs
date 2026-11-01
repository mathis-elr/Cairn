using Cairn.Application.DTO;
using Cairn.Domain.Enums;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.APIs
{
    public interface IPowensApiService
    {
        Task<string> GetUrlConnexionPowens(int idUser);

        Task<string> GetUrlReconnexionPowens(int idUser, int compteId);

        Task CreeNouvelUtilisateur(int userId);

        Task VerifierUtilisateurPowensExists(int userId);

        Task<string> GenerateCodeTemporaireByUserId(int userId);

        Task SaveBanque(int connectionId, int userId);

        Task VerifierEtSynchroniserFluxBancairesAsync();

        Task GetTransactions(DateTime dateDebut, DateTime dateFin, CompteBanque compteBanque);

        Task<List<PositionInvestissementImportDto>?> GetPositionInvestissements(CompteBanque compteBanque);

        Task SynchroniserSoldeComptes();
    }
}
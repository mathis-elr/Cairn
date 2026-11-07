using Cairn.Application.DTO.Powens;
namespace Cairn.Application.Interfaces.APIs
{
    public interface IPowensApiService
    {
        Task<string> GetUrlConnexion(string token);
        Task<string> GetUrlReconnexion(string token, int idConnectionPowens);

        Task<string> GenerateCodeTemporaire(string tokenClair);
        Task<UtilisateurImportDto> CreeNouvelUtilisateur(int userId);

        Task<IEnumerable<CompteImportDto>> GetComptes(string token, int connectionBanqueId);
        Task<string> GetNomBanque(string token, int idConnector);
        Task<ConnectionImportDto> GetConnection(string token, int connectionBanqueId);

        Task<List<FluxBancaireImportDto>> GetTransactions(string tokenClair, int idComptePowens, DateTime? depuis);
        Task<List<PositionInvestImportDto>> GetPositionInvestissements(string tokenClair, int idComptePowens);
    }
}
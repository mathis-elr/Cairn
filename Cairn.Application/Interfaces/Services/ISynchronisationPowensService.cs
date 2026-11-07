namespace Cairn.Application.Interfaces.Services
{
    public interface ISynchronisationPowensService
    {
        Task<string> GetUrlConnexionPowens(int userId);

        Task<string> GetUrlReconnexionPowens(int userId, int compteId);
    }
}

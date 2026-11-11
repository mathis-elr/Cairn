namespace Cairn.Application.Interfaces.Services
{
    public interface IBanqueService
    {
        Task SynchroniserSoldeComptesAsync();

        Task SaveBanque(int connectionBanqueId, int userId);
    }
}

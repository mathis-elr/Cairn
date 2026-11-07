namespace Cairn.Application.Interfaces.Services
{
    public interface IBanqueService
    {
        Task SynchroniserSoldeComptes();

        Task SaveBanque(int connectionBanqueId, int userId);
    }
}

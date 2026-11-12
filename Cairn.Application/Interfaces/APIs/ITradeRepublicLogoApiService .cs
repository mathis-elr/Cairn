namespace Cairn.Application.Interfaces.APIs
{
    public interface ITradeRepublicLogoApiService
    {
        Task<bool> LogoExiste(string? isin);
    }
}

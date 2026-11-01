using Cairn.Application.DTO.Auth;

namespace Cairn.Application.Interfaces.APIs
{
    public interface ITradeRepublicApiService
    {
        Task<(int, string)> GetSms(int userId);

        Task<string> ConfirmSms(string codeSms);

        Task<bool> ChargerTransactions(int userId);

        Task<TradeRepublicAccesDto?> GetTradeRepublicAcces(int userId);

        Task SaveAcces(TradeRepublicAccesDto accesDto, int userId);
    }
}

using Cairn.Domain.Enums;

namespace Cairn.Application.Interfaces.APIs
{
    public interface IYahooFinanceApiService
    {
        Task<Dictionary<string, decimal>> GetPrixActuelAsync(IEnumerable<string> symboles);

        Task<Dictionary<LapsTemps, decimal>> GetPrixHistorique(string ticker);

        Task<string?> ChercherTicker(string? code, string libelle);
    }
}


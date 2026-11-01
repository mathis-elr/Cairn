using Cairn.Application.DTO.FluxBancaires;

namespace Cairn.Application.Interfaces.Services
{
    public interface ICompteTradeRepubliqueService
    {
        Task<SourceDto?> GetByUserId(int userId);
    }
}

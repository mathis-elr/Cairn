using Cairn.Application.DTO.FluxTradeRepublic;
using Cairn.Domain.Modeles;
using Cairn.Domain.Enums;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IFluxTradeRepublicRepository
    {
        Task<IEnumerable<FluxTradeRepublicDto>> GetAllByUserId(int userId);

        Task<FluxTradeRepublic?> GetLastByUserId(int userId);

        Task<FluxTradeRepublic?> GetFirstDateByUserId(int userId);

        Task<List<PositionActifDto>> GetPositionsParActifByUserId(int userId);

        Task<decimal> GetValeurInvestissementTotalByUserId(int userId);

        Task<IEnumerable<PositionTradeRepublicDto>> GetPositionsInvestiesParActifByUserId(int userId);

        Task<List<InvestissementParMoisDto>> GetInvestissementParMoisByUserId(PeriodeHistoriqueInvest periode, int userId);

        Task AddRange(List<FluxTradeRepublic> flux);
    }
}

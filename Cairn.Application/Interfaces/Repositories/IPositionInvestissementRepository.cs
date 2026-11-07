using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IPositionInvestissementRepository
    {
        Task<List<PositionInvestissement>> GetByCompteBanqueId(int compteBanqueId);
        Task<List<PositionInvestissement>> GetByCompteBanqueIdSansInclude(int compteBanqueId);

        Task AddRange(List<PositionInvestissement> positions);

        Task UpdateRange(List<PositionInvestissement> positions);

        Task DeleteByIds(List<int> ids);
    }
}

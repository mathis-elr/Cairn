using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IPositionInvestissementRepository
    {
        Task<List<PositionInvestissement>> GetByCompteBanqueId(int compteId);

        Task AddRange(List<PositionInvestissement> positions);

        Task UpdateRange(List<PositionInvestissement> positions);
    }
}

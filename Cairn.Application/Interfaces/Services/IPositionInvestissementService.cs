using Cairn.Application.DTO;

namespace Cairn.Application.Interfaces.Services
{
    public interface IPositionInvestissementService
    {
        Task<IEnumerable<PositionInvestissementDto>> GetPositionsByCompte(int compteId);

        Task SynchroniserPositions();
    }
}

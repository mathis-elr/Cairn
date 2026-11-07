using Cairn.Application.DTO.PositionsInvestissements;
using Cairn.Application.DTO.Powens;

namespace Cairn.Application.Interfaces.Services
{
    public interface IPositionInvestissementService
    {
        Task<IEnumerable<PositionInvestissementDto>> GetPositionsByCompte(int compteId);

        Task SynchroniserPositions();

        Task SavePositions(List<PositionInvestImportDto> positionsPowens, int compteBanqueId);
    }
}

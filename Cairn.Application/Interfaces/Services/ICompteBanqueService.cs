using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.DTO.Powens;
namespace Cairn.Application.Interfaces.Services
{
    public interface ICompteBanqueService
    {
        Task<List<SourceDto>> GetAllByUserId(int userId);

        Task GetPositionsInvestissement(int compteBanqueId);

        Task SaveComptes(IEnumerable<CompteImportDto> comptesPowens, int banqueId);

        Task UpdateNom(SourceDto source, int userId);
    }
}

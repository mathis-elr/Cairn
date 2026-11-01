using Cairn.Application.DTO.Patrimoine;
using Cairn.Domain.Modeles;
using Cairn.Domain.Enums;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IValeurPatrimoineRepository
    {
        Task<IEnumerable<BougieChartDto>> GetBougiesPlusValueByUserId(Periode periode, Granulometrie granulometrie, int userId);

        Task<IEnumerable<PointChartDto>> GetPointsPlusValueByUserId(Periode periode, Granulometrie granulometrie, int userId);

        Task<IEnumerable<BougieChartDto>> GetBougiesJournalieresValeurPatrimoineSurInvestissementTotalByUserId(int userId);

        Task<DateTime?> GetDateDernierEnregistrement();

        Task<List<ValeurPatrimoine>> GetAllHistoriqueByUserId(int userId);

        Task<List<PositionPatrimoineUtilisateurDto>> GetPositionsPatrimoineParUtilisateur();

        Task AddRange(List<ValeurPatrimoine> valeursPatrimoine);
    }
}
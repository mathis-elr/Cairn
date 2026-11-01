using Cairn.Application.DTO.Patrimoine;
using Cairn.Domain.Enums;

namespace Cairn.Application.Interfaces.Services
{
    public interface IValeurPatrimoineService
    {
        Task<IEnumerable<BougieChartDto>> GetBougiesPlusValueByUserId(Periode periode, Granulometrie granulometrie, int userId);

        Task<IEnumerable<PointChartDto>> GetPointsPlusValueByUserId(Periode periode, Granulometrie granulometrie, int userId);

        Task<IEnumerable<BougieChartDto>> GetBougiesJournalieresValeurPatrimoineSurInvestissmentTotal(int userId);

        Task<IEnumerable<VariationDto>> GetVariations(decimal valeurActuelle, decimal valeurInvestissementTotal, int userId);

        Task<DateTime?> GetDateDernierEnregistrement();

        Task SaveValeurPatrimoine(Dictionary<string, decimal> prixParActif);
    }
}


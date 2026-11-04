using Cairn.Application.DTO.MacroCategories;

namespace Cairn.Application.Interfaces.Services
{
    public interface IMacroCategorieService
    {
        Task<List<MacroCategorieDto>> GetAll();

        Task InitialiserAsync(int userId);
    }
}

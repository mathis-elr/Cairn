using Cairn.Application.DTO.MicroCategories;

namespace Cairn.Application.Interfaces.Services
{
    public interface IMicroCategorieService
    {
        Task<List<MicroCategorieDto>> GetAll();
    }
}

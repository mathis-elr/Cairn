using Cairn.Application.DTO.MicroCategories;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;

namespace Cairn.Application.Services
{
    public class MicroCategorieService(IMicroCategorieRepository microCategorieRepository) : IMicroCategorieService
    {
        private readonly IMicroCategorieRepository _microCategorieRepository = microCategorieRepository;

        public async Task<List<MicroCategorieDto>> GetAll()
        {
            var microCategories = await _microCategorieRepository.GetAll();

            return microCategories.Select(mc => new MicroCategorieDto
            {
                Id = mc.Id,
                Libelle = mc.Libelle,
                Ordre = mc.Ordre,
            }).ToList();
        }
    }
}

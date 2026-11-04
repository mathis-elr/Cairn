using Cairn.Application.Categories;
using Cairn.Application.DTO.MacroCategories;
using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Domain.Modeles;

namespace Cairn.Application.Services
{
    public class MacroCategorieService(IMacroCategorieRepository macroCategorieRepository) : IMacroCategorieService
    {
        private readonly IMacroCategorieRepository _macroCategorieRepository = macroCategorieRepository;

        public async Task<List<MacroCategorieDto>> GetAll()
        {
           var categories = await _macroCategorieRepository.GetAll();

           return categories.Select(c => new MacroCategorieDto
           {
               Id = c.Id,
               Libelle = c.Libelle,
               Role = c.Role,
               Ordre = c.Ordre,
               CouleurHexa = c.CouleurHexa,
               EstActive = c.EstActive
           }).ToList();
        }

        public async Task InitialiserAsync(int userId)
        {
            var macros = CategoriesParDefaut.Macros
                .Select((def, indexMacro) => new MacroCategorie
                {
                    Libelle = def.Libelle,
                    Role = def.Role,
                    Ordre = (byte)indexMacro,
                    CouleurHexa = def.CouleurHexa,
                    EstActive = true,
                    UtilisateurId = userId,
                    MicroCategories = def.Micros
                        .Select((libelle, indexMicro) => new MicroCategorie
                        {
                            Libelle = libelle,
                            Ordre = (byte)indexMicro,
                            EstActive = true
                        })
                        .ToList()
                })
                .ToList();

            await _macroCategorieRepository.AddRangesAsync(macros);
        }
    }
}

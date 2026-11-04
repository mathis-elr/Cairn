using Cairn.Domain.Enums;

namespace Cairn.Application.DTO.MacroCategories
{
    public class MacroCategorieDto
    {
        public int Id { get; set; }

        public required string Libelle { get; set; }

        public RoleCategorie Role { get; set; }

        public byte Ordre { get; set; }

        public required string CouleurHexa { get; set; }

        public bool EstActive { get; set; }
    }
}

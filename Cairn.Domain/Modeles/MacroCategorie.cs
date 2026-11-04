using Cairn.Domain.Enums;

namespace Cairn.Domain.Modeles
{
    public class MacroCategorie
    {
        public int Id { get; set; }

        public required string Libelle { get; set; }

        public RoleCategorie Role { get; set; }

        public byte Ordre { get; set; }

        public required string CouleurHexa { get; set; }

        public bool EstActive { get; set; } = true;

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; } = null!;

        public ICollection<MicroCategorie> MicroCategories { get; set; } = [];

    }
}

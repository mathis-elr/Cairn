using Cairn.Domain.Enums;

namespace Cairn.Domain.Modeles
{
    public class Banque
    {
        public int Id { get; set; }

        public int IdConnectionPowens { get; set; }

        public int IdConnectorPowens { get; set; }

        public required string Nom { get; set; }

        public StatutConnexion StatutConnexion { get; set; }
        public DateTime? DerniereSynchro { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; } = null!;

        public ICollection<CompteBanque> ComptesBanques { get; set; } = [];

    }
}

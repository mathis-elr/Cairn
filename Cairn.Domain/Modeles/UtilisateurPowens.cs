namespace Cairn.Domain.Modeles
{
    public class UtilisateurPowens
    {
        public int Id { get; set; }

        public int IdPowens { get; set; }

        public required string AccessTokenCrypte { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; } = null!;
    }
}

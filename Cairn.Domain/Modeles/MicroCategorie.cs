namespace Cairn.Domain.Modeles
{
    public class MicroCategorie
    {
        public int Id { get; set; }

        public required string Libelle { get; set; }

        public byte Ordre { get; set; }

        public bool EstActive { get; set; } = true;

        public int MacroCategorieId { get; set; }
        public MacroCategorie MacroCategorie { get; set; } = null!;

        public ICollection<FluxBancaire> FluxBancaires { get; set; } = [];
    }
}

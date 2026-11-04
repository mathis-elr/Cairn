using Cairn.Domain.Enums;

namespace Cairn.Application.Categories
{
    public static class CategoriesParDefaut
    {
        public sealed record MacroDefaut(string Libelle, RoleCategorie Role, string CouleurHexa, string[] Micros);

        public static readonly IReadOnlyList<MacroDefaut> Macros =
        [
            new("Revenus",        RoleCategorie.Revenu,         "#22C55E", ["Salaire", "Aides", "Remboursements", "Autres revenus"]),
            new("Logement",       RoleCategorie.Depense,        "#3B82F6", ["Loyer", "Charges", "Énergie", "Internet / Mobile", "Assurance habitation"]),
            new("Alimentation",   RoleCategorie.Depense,        "#F59E0B", ["Courses", "Restaurants"]),
            new("Transports",     RoleCategorie.Depense,        "#6366F1", ["Carburant", "Transports en commun", "Assurance auto", "Entretien"]),
            new("Loisirs",        RoleCategorie.Depense,        "#EC4899", ["Sorties", "Abonnements", "Voyages", "Shopping"]),
            new("Santé",          RoleCategorie.Depense,        "#EF4444", ["Médecin", "Pharmacie", "Mutuelle"]),
            new("Épargne",        RoleCategorie.Epargne,        "#14B8A6", ["Virement épargne"]),
            new("Investissement", RoleCategorie.Investissement, "#8B5CF6", ["Versement investissement"]),
            new("Neutre",         RoleCategorie.Neutre,         "#94A3B8", ["Virement interne"]),
            new("Divers",         RoleCategorie.Depense,        "#64748B", ["Autre"]),
        ];
    }
}

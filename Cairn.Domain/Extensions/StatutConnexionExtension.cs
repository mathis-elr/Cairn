using Cairn.Domain.Enums;

namespace Cairn.Domain.Extensions
{
    public static class StatutConnexionExtension
    {
        public static StatutConnexion ToStatutConnexion(string? powensState)
        {
            if (string.IsNullOrWhiteSpace(powensState))
                return StatutConnexion.Valide;

            return powensState.Trim().ToLowerInvariant() switch
            {
                "valide" or "valid" => StatutConnexion.Valide,

                "scarequired"
                or "webauthrequired"
                or "additionalinformationneeded"
                or "decoupled"
                or "wrongpass"
                or "passwordexpired"
                or "action_needed" => StatutConnexion.ReconnexionRequise,

                "websiteunavailable" => StatutConnexion.SiteIndisponible,

                _ => StatutConnexion.ErreurTechnique
            };
        }
    }
}

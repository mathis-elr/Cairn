namespace Cairn.Application.Interfaces.Services
{
    public interface IActifService
    {
        Task<int> GetActifId(string? code, string libelle);

        string NettoyerLibelle(string libelle);
    }
}

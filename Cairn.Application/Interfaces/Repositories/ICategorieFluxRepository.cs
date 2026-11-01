using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface ICategorieFluxRepository
    {
        Task<List<CategorieFlux>> GetAll();
    }
}

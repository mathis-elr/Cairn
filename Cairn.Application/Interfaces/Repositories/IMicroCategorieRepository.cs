using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface IMicroCategorieRepository
    {
        Task<List<MicroCategorie>> GetAll();
    }
}

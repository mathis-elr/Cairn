using Cairn.Domain.Modeles;

namespace Cairn.Application.Interfaces.Repositories
{
    public interface ITradeRepublicAccesRepository
    {
        Task<CompteTradeRepublic?> GetByUserId(int userId);

        Task<CompteTradeRepublic?> GetLoginByUserId(int userId);

        Task Add(CompteTradeRepublic acces);

        Task UpdateDateSynchornisation(int userId);
    }
}

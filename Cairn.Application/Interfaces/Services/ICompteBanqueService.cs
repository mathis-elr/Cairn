using Cairn.Application.DTO.FluxBancaires;

namespace Cairn.Application.Interfaces.Services
{
    public interface ICompteBanqueService
    {
        Task<List<SourceDto>> GetAllByUserId(int userId);

        Task UpdateNom(SourceDto sourceDto); 
    }
}

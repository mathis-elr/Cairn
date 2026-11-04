using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.Interfaces.APIs;

namespace Cairn.Application.Services
{
    public class CompteBanqueService(ICompteBanqueRepository compteBanqueRepository,
                                     ILogoDevApiService logoDevApiService) : ICompteBanqueService
    {
        private readonly ICompteBanqueRepository _compteBanqueRepository = compteBanqueRepository;
        private readonly ILogoDevApiService _logoDevApiService = logoDevApiService;

        public async Task<List<SourceDto>> GetAllByUserId(int userId)
        {
            var comptes = await _compteBanqueRepository.GetAllByUserId(userId);
            return comptes.Select(c => new SourceDto
            {
                Id = c.Id,
                NomSource = c.Banque.Nom,
                NomCompte = c.NomPersonnalise ?? c.NomPowens,
                TypeCompte = c.TypeCompte,
                LogoUrl = _logoDevApiService.GetUrlLogoByName(c.Banque.Nom),
                StatutConnexion = c.Banque.StatutConnexion,
                DerniereSychro = c.Banque.DerniereSynchro
            }).ToList();
        }

        public async Task UpdateNom(SourceDto source) 
        {
            await _compteBanqueRepository.UpdateNom(source);
        }
    }
}

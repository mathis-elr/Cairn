using Cairn.Application.Interfaces.Repositories;
using Cairn.Application.Interfaces.Services;
using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.Interfaces.APIs;
using Cairn.Domain.Enums;

namespace Cairn.Application.Services
{
    public class CompteTradeRepubliqueService(ITradeRepublicAccesRepository tradeRepublicAccesRepository,
                                              ILogoDevApiService logoDevApiService) : ICompteTradeRepubliqueService
    {
        private readonly ITradeRepublicAccesRepository _tradeRepublicAccesRepository = tradeRepublicAccesRepository;
        private readonly ILogoDevApiService _logoDevApiService = logoDevApiService;

        public async Task<SourceDto?> GetByUserId(int userId)
        {
            var compte = await _tradeRepublicAccesRepository.GetByUserId(userId);
            if(compte == null)
                return null;

            return new SourceDto
            {
                Id = -1,
                NomSource = "Trade Republic",
                NomCompte = "Portefeuille",
                TypeCompte = TypeCompte.Investissement,
                LogoUrl = _logoDevApiService.GetUrlLogoByName("Trade Republic")
            };
        }
    }
}

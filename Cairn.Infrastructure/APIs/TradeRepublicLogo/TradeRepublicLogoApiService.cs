using Cairn.Application.Interfaces.APIs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cairn.Infrastructure.APIs.TradeRepublicLogo
{
    public class TradeRepublicLogoApiService(IOptions<TradeRepublicLogoApiOptions> options,
                                             ILogger<TradeRepublicLogoApiService> logger,
                                             HttpClient httpClient) : ITradeRepublicLogoApiService
    {
        private readonly TradeRepublicLogoApiOptions _options = options.Value;
        private readonly ILogger<TradeRepublicLogoApiService> _logger = logger;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<bool> LogoExiste(string? isin)
        {
            if(string.IsNullOrWhiteSpace(isin))
            {
                _logger.LogWarning("ISIN vide ou null pour la vérification du logo Trade Republic");
                return false;
            }

            var url = string.Format(_options.BaseUrl, Uri.EscapeDataString(isin));

            try
            {
                using var requete = new HttpRequestMessage(HttpMethod.Head, url);
                using var reponse = await _httpClient.SendAsync(requete);
                return reponse.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(ex, "Vérification du logo Trade Republic impossible pour {Isin}", isin);
                return false;
            }
        }
    }
}

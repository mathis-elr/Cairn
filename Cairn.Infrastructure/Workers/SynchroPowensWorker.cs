using Cairn.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cairn.Infrastructure.Workers
{
    public class SynchroPowensWorker(ILogger<SynchroPowensWorker> logger,
                                     IServiceProvider serviceProvider,
                                     IHostEnvironment environment) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<SynchroPowensWorker> _logger = logger;
        private readonly IHostEnvironment _environment = environment;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_environment.IsDevelopment())
                    return;

                _logger.LogInformation("Début de la synchronisation des flux Powens ...");

                using var scope = _serviceProvider.CreateScope();
                var fluxBancaireService = scope.ServiceProvider.GetRequiredService<IFluxBancaireService>();
                var positionService = scope.ServiceProvider.GetRequiredService<IPositionInvestissementService>();

                try
                {
                    await fluxBancaireService.SynchroniserFluxBancairesAsync();
                    _logger.LogInformation("Fin de la synchronisation des flux Powens.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur synchronisation des flux bancaires Powens");
                    Console.WriteLine($"Error synchronizing bank flows: {ex.Message}");
                }

                try
                {
                    await positionService.SynchroniserPositions();
                    _logger.LogInformation("Fin de la synchronisation des positions Powens.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur synchronisation des positions Powens");
                    Console.WriteLine($"Error synchronizing positions: {ex.Message}");
                }

                _logger.LogInformation("Fin de la synchronisation des flux et positions Powens.");

                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}

using Cairn.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cairn.Infrastructure.Workers
{
    public class SynchroFluxBancairesWorker(IServiceProvider serviceProvider,
                                                 IHostEnvironment environment) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IHostEnvironment _environment = environment;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    bool isJourOk = !_environment.IsDevelopment() && DateTime.UtcNow.Day >= 5;

                    if (isJourOk)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var fluxBancaireService = scope.ServiceProvider.GetRequiredService<IFluxBancaireService>();

                        await fluxBancaireService.SynchroniserFluxBancairesAsync();
                    }

                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur dans le worker de synchro : {ex.Message}");
                }
            }
        }
    }
}

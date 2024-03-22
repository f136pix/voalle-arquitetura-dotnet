using GrupoVoalle.Treinamento.Consoles.Works.Executors;
using Microsoft.Extensions.Hosting;

namespace GrupoVoalle.Treinamento.Consoles.Works.HostedServices
{
    public class DashboardHostedService : BackgroundService
    {
        private readonly DashboardExecutor _executor;

        public DashboardHostedService(IServiceProvider serviceProvider)
        {
            _executor = new DashboardExecutor(serviceProvider);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _executor.Execute(stoppingToken);

            return Task.CompletedTask;
        }
    }
}
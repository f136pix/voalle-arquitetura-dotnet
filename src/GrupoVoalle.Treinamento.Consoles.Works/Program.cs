using GrupoVoalle.Treinamento.CrossCutting.IoC;
using GrupoVoalle.Domain.Core.Application;
using GrupoVoalle.Queues.Core.Helper;
using GrupoVoalle.Treinamento.Consoles.Works.Extensions;
using GrupoVoalle.Treinamento.Consoles.Works.HostedServices;
using GrupoVoalle.Treinamento.Consoles.Works.Managements;
using GrupoVoalle.Utility.Security.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrupoVoalle.Treinamento.Consoles.Works
{
    class Program
    {
        static void Main(string[] args)
        {
            Utility.Globalization.Globalization.SetToPtBR();

            // Registra o nome da aplicação
            ApplicationInformation.Name = "GrupoVoalle.Treinamento.Console";

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddLogging((builder) => builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information));
                    services.AddScoped<IDataQueueInject, DataQueueInject>();
                    services.AddScoped<IAccessorData, AccessorDataQueue>();

                    MediatRExtension.Register(services);

                    NativeInjectorBootStrapper.RegisterServices(services);

                    // Carrega configuracoes de gerenciamento
                    ConfigDatabaseManagement.Load();

                    // RabbitMQ
                    services.AddRabbitConnection();

                    // Queues em execução
                    services.AddHostedService<DashboardHostedService>();

                    NativeInjectorBootStrapper.RegisterServices(services);

                    var provider = services.BuildServiceProvider();

                    // var loggerFactory = provider.GetRequiredService<ILoggingBuilder>();
                    // loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                    // LogManager.Setup().LoadConfigurationFromFile("nlog.config");
                })
                .UseNLog()
                .UseConsoleLifetime()
                .Build();

            host.Run();
        }
    }
}
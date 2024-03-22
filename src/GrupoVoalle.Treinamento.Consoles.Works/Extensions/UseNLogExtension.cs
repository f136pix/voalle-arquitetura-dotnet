using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Extensions.Logging;
using System.Reflection;

namespace GrupoVoalle.Treinamento.Consoles.Works.Extensions
{
    public static class UseNLogExtension
    {
        public static IHostBuilder UseNLog(this IHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ConfigureServices(services =>
            {
                LogManager.Setup().SetupExtensions(s =>
                    s.RegisterAssembly(typeof(UseNLogExtension).GetTypeInfo().Assembly)
                );

                LogManager.AddHiddenAssembly(typeof(UseNLogExtension).GetTypeInfo().Assembly);

                // services.AddSingleton(new LoggerFactory().AddNLog(new NLogProviderOptions
                // {
                //     CaptureMessageTemplates = true,
                //     CaptureMessageProperties = true
                // }));

                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddNLog(new NLogProviderOptions
                    {
                        CaptureMessageTemplates = true,
                        CaptureMessageProperties = true
                    });
                });
            });

            return builder;
        }
    }
}
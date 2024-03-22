using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GrupoVoalle.Treinamento.Consoles.Works.Extensions
{
    public static class MediatRExtension
    {
        public static void Register(IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
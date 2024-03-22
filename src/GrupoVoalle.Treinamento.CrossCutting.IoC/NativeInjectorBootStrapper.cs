using GrupoVoalle.Treinamento.Infra.DataContext;
using GrupoVoalle.Database.Core.Context;
using GrupoVoalle.Treinamento.CrossCutting.IoC.System;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoVoalle.Treinamento.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Registra as dependencias dos módulos
            InjectTreinamento.Register(services);
            InjectSystem.Register(services);
            #endregion Registra as dependencias dos módulos

            #region Contexto
            services.AddScoped<GrupoVoalleContext>(provider => { return ResolveDbContext(provider); });
            #endregion Contexto

            services.AddScoped<UserHttp>(provider => provider.GetService<RecoveryStringConnection>()!.UserHttp);
            services.AddScoped<HttpRequestData>(provider => provider.GetService<RecoveryStringConnection>()!.HttpRequestData);
        }

        private static GrupoVoalleContext ResolveDbContext(IServiceProvider provider)
        {
            var recovery = provider.GetService<RecoveryStringConnection>();

            return new DefaultDbContextFactory<GrupoVoalleContext>().CreateDbContext(recovery?.UserHttp.SynData.ToArray());
        }
    }
}
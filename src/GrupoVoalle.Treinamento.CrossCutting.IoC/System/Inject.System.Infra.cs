using GrupoVoalle.Treinamento.Infra.DataContext;
using GrupoVoalle.CrossCutting.Core.Bus;
using GrupoVoalle.Domain.Core.Interfaces;
using GrupoVoalle.Utility.Security.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoVoalle.Treinamento.CrossCutting.IoC.System
{
    /// <summary>
    /// ================================== A t e n ç ã o ==================================
    ///
    /// Ao fazer a adição de uma nova classe, deverá ser realizado a ordenação dos itens
    /// contidos dentro do método "private static void Register....." e também dos "using's".
    ///
    /// ================================== A t e n ç ã o ==================================
    /// </summary>
    public static partial class InjectSystem
    {
        private static void RegisterInfra(IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<RecoveryStringConnection>();
        }
    }
}
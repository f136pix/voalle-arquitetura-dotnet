using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
using GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogItems;
using GrupoVoalle.Treinamento.Infra.Persistence.Postgres.LogTables;
using Microsoft.Extensions.DependencyInjection;

namespace GrupoVoalle.Treinamento.CrossCutting.IoC
{
    /// <summary>
    /// ================================== A t e n ç ã o ==================================
    ///
    /// Ao fazer a adição de uma nova classe, deverá ser realizado a ordenação dos itens
    /// contidos dentro do método "private static void Register....." e também dos "using's".
    ///
    /// ================================== A t e n ç ã o ==================================
    /// </summary>
    public static partial class InjectTreinamento
    {
        private static void RegisterRepository(IServiceCollection services)
        {
            services.AddScoped<ILogItemsRepository, LogItemsRepository>();
            services.AddScoped<ILogTablesRepository, LogTablesRepository>();
        }
    }
}
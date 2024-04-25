using GrupoVoalle.Base.Business.Primitives.People;
using GrupoVoalle.Treinamento.Business.Primitives.LogItems;
using GrupoVoalle.Treinamento.Business.Primitives.LogTables;
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
        private static void RegisterService(IServiceCollection services)
        {
            services.AddScoped<ILogItemsService, LogItemsService>();
            services.AddScoped<ILogTablesService, LogTablesService>();
            services.AddScoped<IPeopleService, PeopleService>();
        }
    }
}
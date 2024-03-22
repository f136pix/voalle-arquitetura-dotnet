using GrupoVoalle.Treinamento.Business.Business.Interfaces.WebService;
using GrupoVoalle.Treinamento.ExternalCommunications.WebService;
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
        private static void RegisterWebService(IServiceCollection services)
        {
            services.AddScoped<IExempleWebService, ExempleWebService>();
        }
    }
}
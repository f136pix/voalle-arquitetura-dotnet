using GrupoVoalle.CrossCutting.Core.Cqrs.Notifications;
using MediatR;
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
        private static void RegisterEvent(IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<CommitTransactionNotification>, CommitTransactionNotificationHandler>();
            // services.AddScoped<INotificationHandler<NewLogEvent>, LogEventHandler>();
        }
    }
}
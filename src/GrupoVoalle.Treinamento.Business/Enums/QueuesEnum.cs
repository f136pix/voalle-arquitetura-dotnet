using System.ComponentModel;

namespace GrupoVoalle.Treinamento.Business.Enums
{
    public enum QueuesEnum
    {
        [Description("GrupoVoalle.Treinamento.Works.QueueExempleAsync")]
        QueueExemple = 1,

        [Description("GrupoVoalle.Treinamento.Works.QueueExempleAsync.NotProcessed")]
        QueueExempleNotProcessed = 2,

        [Description("GrupoVoalle.Treinamento.Works.QueueExempleWithExecutor")]
        QueueExempleWithExecutor = 3,

        [Description("GrupoVoalle.Treinamento.Works.QueueExempleWithExecutor.NotProcessed")]
        QueueExempleNotProcessedWithExecutor = 4,
    }
}
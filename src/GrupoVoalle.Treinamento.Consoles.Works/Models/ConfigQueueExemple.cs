using GrupoVoalle.Queues.Core.Base;

namespace GrupoVoalle.Treinamento.Consoles.Works.Models
{
    public class ConfigQueueExemple : ConfigBase
    {
        public string QueueName => Business.Enums.QueueName.GetQueueExempleName(DatabaseName);
    }
}
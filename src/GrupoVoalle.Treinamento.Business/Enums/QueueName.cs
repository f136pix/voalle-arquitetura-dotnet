namespace GrupoVoalle.Treinamento.Business.Enums
{
    public class QueueName
    {
        private const string NAME_BASE = "GrupoVoalle.Treinamento.Works.";
        private const string QUEUE_NAME_BASE = NAME_BASE;

        public const string REGISTER_DATABASE = NAME_BASE + "RegisterDatabase";

        public static string GetQueueExempleName(string database)
        {
            return $"{QUEUE_NAME_BASE}{database}.QueueExempleAsync";
        }
    }
}
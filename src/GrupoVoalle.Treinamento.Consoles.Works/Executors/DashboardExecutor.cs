using GrupoVoalle.Queues.Core.Base;
using GrupoVoalle.Treinamento.Consoles.Works.Managements;
using GrupoVoalle.Treinamento.Consoles.Works.Utils;
using GrupoVoalle.Utility.Extensions;

namespace GrupoVoalle.Treinamento.Consoles.Works.Executors
{
    public class DashboardExecutor : BaseExecutor<DashboardExecutor>
    {
        public DashboardExecutor(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Init();
        }

        public override async void Execute(CancellationToken ct)
        {
            Console.WriteLine("Inicializando console...");

            while (!ct.IsCancellationRequested)
            {
                // Removido logs para evitar problemas de espaço
                // Se necessário olhar no Rabbit MQ
                // Write(GetCounterData);

                await Task.Delay(TimeSpan.FromSeconds(20), ct);
            }
        }

        private List<string> GetDatabaseRegistred()
        {
            var list = new List<string>();

            list.Add("Bases registradas");

            var databasesRegistred = ConfigDatabaseManagement.AllDatabasesRegistered
                .Select(x => new { x.DatabaseName, x.HasLogin })
                .ToList();

            foreach (var item in databasesRegistred)
            {
                if (item.HasLogin)
                    list.Add($"{item.DatabaseName} - LOGADO");
                else
                    list.Add($"{item.DatabaseName} - NÃO LOGADO");
            }

            return list;
        }

        private List<string> GetCounterData()
        {
            return CounterData.GetAll();
        }

        private void Init() { }

        private void Write(Func<List<string>> returnListString)
        {
            var list = returnListString();

            var qtdChar = list.Count > 0
                ? list.First().Length
                : 40;
            if (qtdChar < 40)
                qtdChar = 40;

            InternalWrite(TreatLine($"[{DateTime.Now.ToString("dd/MM HH:mm")}]   -   Dashboard Works  --", qtdChar, true));

            foreach (var line in GetDatabaseRegistred())
                InternalWrite(TreatLine(line, qtdChar));

            InternalWrite(TreatLine("---", qtdChar));

            foreach (var line in list)
                InternalWrite(TreatLine(line, qtdChar));

            InternalWrite(TreatLine("", qtdChar, true));
        }

        private void InternalWrite(string message)
        {
            Console.WriteLine(message);
        }

        private string TreatLine(string lineRaw, int qtdChar, bool isHeaderOrFooter = false)
        {
            if (isHeaderOrFooter)
                return "-" + lineRaw.Limit(qtdChar, '-', false) + "-";
            else
                return "|" + lineRaw.Limit(qtdChar, ' ', false) + "|";
        }
    }
}






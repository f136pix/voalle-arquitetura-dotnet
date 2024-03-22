using GrupoVoalle.Queues.Core.Base;
using GrupoVoalle.Utility.ConfigBuilders;
using GrupoVoalle.Utility.Messages;
using GrupoVoalle.Utility.Security.Http;
using GrupoVoalle.Utility.Security.Token;
using Newtonsoft.Json;

namespace GrupoVoalle.Treinamento.Consoles.Works.Managements
{
    public static class ConfigDatabaseManagement
    {
        private const string FileConsumer = "rabbitmq.base.database.json";

        public static List<ConfigDatabase> AllDatabasesRegistered = new List<ConfigDatabase>();
        public static List<ConfigDatabase> LoggedDatabases { get { return AllDatabasesRegistered.Where(x => x.HasLogin).ToList(); } }

        private static void Add(ConfigDatabase config)
        {
            if (config == null)
                throw new ArgumentNullException();

            AllDatabasesRegistered.Add(config);
        }

        public static void Load()
        {
            lock (AllDatabasesRegistered)
            {
                var file = GetFilePath();
                if (File.Exists(file))
                {
                    var list = JsonConvert.DeserializeObject<List<ConfigDatabase>>(File.ReadAllText(file));
                    foreach (var item in list)
                        InternalRegister(item.SynDataRaw);
                }
            }
        }

        private static string GetFilePath()
        {
            var path = ConfigAppSettings.Instance.Configuration["SynSuite:ConfigQueue"];
            if (path == null)
                throw new DirectoryNotFoundException("Não foi encontrado o valor na variável Output.");

            return Path.Combine(path, ConfigDatabaseManagement.FileConsumer);
        }

        private static void Save()
        {
            File.WriteAllText(GetFilePath(), JsonConvert.SerializeObject(AllDatabasesRegistered.ToList(), Formatting.Indented));
        }

        private static bool Any(Func<ConfigDatabase, bool> predicate)
        {
            return AllDatabasesRegistered.Any(predicate);
        }

        public static void TestAutoLogin()
        {
            lock (AllDatabasesRegistered)
            {
                foreach (var item in AllDatabasesRegistered.Where(x => !x.HasLogin).ToList())
                {
                    var ret = AutoLogin(item.SynDataRaw);
                    if (ret.Success)
                    {
                        item.UserHttpRaw = ret.Data;
                        item.HasLogin = true;
                    }
                }
            }
        }

        private static ReturnData<string> AutoLogin(string synDataRaw)
        {
            try
            {
                var userHttp = AutoLoginConsole.Login(new SynData(synDataRaw));
                if (!userHttp.Success)
                    throw new InvalidOperationException();

                return new ReturnData<string>(true, userHttp.Data.Encrypt());
            }
            catch (Exception)
            {
                return new ReturnData<string>(false);
            }
        }

        private static void InternalRegister(string synDataRaw)
        {
            var ret = AutoLogin(synDataRaw);

            Add(new ConfigDatabase
            {
                HasLogin = ret.Success,
                UserHttpRaw = ret.Data,
                SynDataRaw = synDataRaw
            });
        }

        public static void Register(string synDataRaw)
        {
            lock (AllDatabasesRegistered)
            {
                var hasValue = Any(x => x.SynDataRaw == synDataRaw);
                if (!hasValue)
                {
                    InternalRegister(synDataRaw);

                    Save();
                }
            }
        }

        public static void Started(Func<bool> hasMemory, Action<ConfigDatabase> createTask)
        {
            lock (AllDatabasesRegistered)
            {
                var collection = AllDatabasesRegistered.Where(x => x.HasLogin).ToList();
                foreach (var item in collection)
                {
                    if (!hasMemory())
                        break;

                    createTask(item);
                }
            }
        }
    }
}
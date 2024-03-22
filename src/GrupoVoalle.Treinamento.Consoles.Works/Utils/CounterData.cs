namespace GrupoVoalle.Treinamento.Consoles.Works.Utils
{
    public class CounterData
    {
        private static Dictionary<string, long> Counter = new Dictionary<string, long>();

        public static long GetCounter(string hash)
        {
            try
            {
                return Counter
                    .Where(x => x.Key == hash)
                    .Select(x => x.Value)
                    .FirstOrDefault();
            }
            catch
            {
                return 0;
            }
        }

        public static List<string> GetAll()
        {
            try
            {
                var itemsKey = Counter.Select(x => x.Key.Length).ToList();
                var maxColumnKey = itemsKey.Count > 0
                    ? itemsKey.Max()
                    : 0;

                var itemsValue = Counter.Select(x => x.Value.ToString().Length).ToList();
                var maxColumnValue = itemsValue.Count > 0
                    ? itemsValue.Max()
                    : 1;

                return Counter
                    .OrderBy(x => x.Key)
                    .Select(x => string.Concat(
                        x.Key.PadRight(maxColumnKey, ' '),
                        " - ",
                        x.Value.ToString().PadLeft(maxColumnValue, '0')
                    ))
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static void SetCounter(string hash)
        {
            lock (Counter)
            {
                try
                {
                    var hasValue = Counter
                        .Where(x => x.Key == hash)
                        .Any();
                    if (!hasValue)
                    {
                        Counter.Add(hash, 1);
                    }
                    else
                    {
                        var oldValue = Counter[hash];
                        if (long.MaxValue == oldValue)
                            oldValue = 0;

                        Counter[hash] = ++oldValue;
                    }
                }
                catch { }
            }
        }

        public static void Init(string key)
        {
            lock (Counter)
            {
                try
                {
                    var hasValue = Counter
                        .Where(x => x.Key == key)
                        .Any();
                    if (!hasValue)
                        Counter.Add(key, 0);
                }
                catch { }
            }
        }
    }
}
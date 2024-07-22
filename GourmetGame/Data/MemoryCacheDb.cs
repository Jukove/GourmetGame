using GourmetGame.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace GourmetGame.Data
{
    public class MemoryCacheDb
    {

        private static readonly Lazy<MemoryCacheDb> lazy =
       new Lazy<MemoryCacheDb>(() => new MemoryCacheDb());

        public static MemoryCacheDb Instance { get { return lazy.Value; } }

        private ConcurrentDictionary<string, object> _storage;
        public List<string> StorageSearch { get; set; } = new List<string>();

        private MemoryCacheDb()
        {
            _storage = new ConcurrentDictionary<string, object>();
            //AddOrUpdate("options", "nenhuma das opcoes");
        }

        public void AddOrUpdate(string key, object value)
        {
            _storage.AddOrUpdate(key, value, (oldKey, oldValue) => value);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _storage.TryGetValue(key, out value);
        }

        public IEnumerable<KeyValuePair<string, object>> GetAllItems()
        {
            return _storage;
        }

        public KeyValuePair<string, object> GetItem(string search)
        {
            return _storage.FirstOrDefault(hint => hint.Key == search );
        }

    }
}

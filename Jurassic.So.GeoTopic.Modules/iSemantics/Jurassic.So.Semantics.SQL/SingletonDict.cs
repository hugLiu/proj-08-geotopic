using System.Collections.Generic;
using Jurassic.So.Semantics.Entities;

namespace Jurassic.So.Semantics.SQL
{
    public class SingletonDict
    {
        private static SingletonDict _instance;

        public Dictionary<string, List<string>> TransDict { get; set; }
        public Dictionary<string, List<string>> AliasDict { get; set; }

        private SingletonDict()
        {
            var sqlProvider = new SemanticsSQLProvider();
            TransDict = sqlProvider.GetTransDict();
            AliasDict = sqlProvider.GetAliasDict();
            PanGu.Segment.Init();
        }

        public static SingletonDict Instance => _instance ?? (_instance = new SingletonDict());

        public static void Init()
        {
            _instance = new SingletonDict();
        }
    }
}
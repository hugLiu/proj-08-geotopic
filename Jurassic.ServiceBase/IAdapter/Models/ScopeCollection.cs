using Jurassic.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Adapter
{
    /// <summary>适配器域集合</summary>
    [Serializable]
    public class ScopeCollection : List<Scope>
    {
        /// <summary>构造函数</summary>
        public ScopeCollection()
        {
        }
        /// <summary>构造函数</summary>
        public ScopeCollection(IEnumerable<Scope> values) : base(values)
        {
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}

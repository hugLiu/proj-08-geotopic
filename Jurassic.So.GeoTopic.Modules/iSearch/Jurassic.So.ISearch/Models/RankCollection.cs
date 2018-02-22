using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 提升规则
    /// </summary>
    [Serializable]
    [DataContract]
    public class RankCollection : Dictionary<string, double>
    {
    }
}

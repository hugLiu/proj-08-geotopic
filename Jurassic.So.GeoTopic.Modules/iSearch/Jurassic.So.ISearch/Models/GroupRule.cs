using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure.Model;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 聚合条件
    /// </summary>
    [Serializable]
    [DataContract]
    public class GroupRule
    {
        public GroupRule()
        {
            this.GFields=new List<string>();
        }
        [DataMember(Name = "top")]
        public int Top { get; set; }
        [DataMember(Name = "gfields")]
        public List<string> GFields { get; set; }
    }
}

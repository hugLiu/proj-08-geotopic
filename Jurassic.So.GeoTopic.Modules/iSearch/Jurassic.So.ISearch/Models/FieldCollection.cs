using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 字段投影
    /// </summary>
    [Serializable]
    [DataContract]
   public  class FieldCollection : Dictionary<string, int>
    {
    }
}

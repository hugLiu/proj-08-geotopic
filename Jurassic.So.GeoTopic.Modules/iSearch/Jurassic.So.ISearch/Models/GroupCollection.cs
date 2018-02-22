using System.Collections.Generic;
using System.Web.Http.Routing.Constraints;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 聚合结果
    /// </summary>
    public class GroupCollection : Dictionary<string, Dictionary<string, long>>
    { }
}

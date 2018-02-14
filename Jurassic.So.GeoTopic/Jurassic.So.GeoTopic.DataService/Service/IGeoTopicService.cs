using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface IGeoTopicService
    {
        /// <summary>
        /// an KeyValue参数获取topic树的数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<KTopicModel> GetTopics(Dictionary<string,string> parm,string[] code,string[] title,string userId);
    }
}

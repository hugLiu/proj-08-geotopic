using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface IWrapedDataSearchService
    {
        /// <summary>
        /// 获取元数据定义集合
        /// </summary>
        /// <returns></returns>
        MetadataDefinitionCollection GetMetadataDefinition();
    }
}

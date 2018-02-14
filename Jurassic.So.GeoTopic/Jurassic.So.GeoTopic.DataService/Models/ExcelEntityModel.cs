using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class ExcelEntityModel
    {

      public ExcelEntityModel()
        {
            TopicPathTitle = string.Empty;
            IndexDefinitionInfo = string.Empty;
            Layout = string.Empty;
            PtCellInfos =new List<PtCellInfo>();
        }
        public string TopicPathTitle { get; set; }
        public string IndexDefinitionInfo { get; set; }
        public List<PtCellInfo> PtCellInfos { get; set; }
        public string Layout { get; set; }
        public bool IsRemark { get; set; }
        public bool IsPtList { get; set; }
    }


    public class PtCellInfo
    {
        public string falgId { get; set; }
        public string PtTitle { get; set; }
        public string Params { get; set; }
        public string Url { get; set; }
    }
}

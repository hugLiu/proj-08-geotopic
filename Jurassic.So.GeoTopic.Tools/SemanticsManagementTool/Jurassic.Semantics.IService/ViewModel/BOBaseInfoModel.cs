using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class BoBaseInfoModel
    {
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        public string BOT { get; set; }

        public string SID { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public Guid? TypeID { get; set; }

        public string SpaceLocationArea { get; set; }
        /// <summary>
        /// 空间坐标类型的描述
        /// </summary>
        public string SpaceLocationType { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }

        public string SourceID { get; set; }

        public string SourceDB { get; set; }

        public string SourceTable { get; set; }

        /// <summary>
        /// 业务对象的额别名用逗号隔开
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 是否为叶子节点
        /// </summary>
        public bool isLeaf { get; set; }
        //是否展开
        public bool expanded { get; set; }
    }
}

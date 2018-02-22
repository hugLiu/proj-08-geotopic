using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Jurassic.Sooil.IServiceBase
{
    /// <summary>
    /// 表示业务数据模型的基础类型。
    /// </summary>
    [Serializable]
    public class BusinessModel
    {
        /// <summary>
        /// 获取或设置变化数据状态。
        /// </summary>
        public virtual DataStatus DataStatus { get; set; }
       
        //protected virtual void SetDataStatus(DataStatus status)
        //{
        //    if (this.DataStatus ==  DataStatus.Added)
        //    {
        //        return;
        //    }

        //    if (this.DataStatus == Infrastructure.DataStatus.Deleted)
        //    {
        //        return;
        //    }

        //    this.DataStatus = status;
        //}
    }
}

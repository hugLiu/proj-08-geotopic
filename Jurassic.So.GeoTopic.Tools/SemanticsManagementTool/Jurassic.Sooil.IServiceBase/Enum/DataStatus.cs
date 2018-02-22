using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    /// <summary>
    /// 数据状态枚举
    /// </summary>
    public enum DataStatus
    {
        //查询
        Unchanged = 0,
        //修改
        Modified = 1,
        //添加
        Added = 2,
        //删除
        Deleted = 3,
        //自动识别为添加或修改
        Automated = 4
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface ICodeOperateService
    {

        /// <summary>
        /// 获取所有的码表类型
        /// </summary>
        /// <returns></returns>
        List<CodeTypeModel> GetAllCodeType();

        /// <summary>
        /// 获取码表记录
        /// </summary>
        /// <param name="codeTypeId"></param>
        /// <returns></returns>
        List<CodeModel> GetCodes(int? codeTypeId);


        /// <summary>
        /// 编辑码表信息
        /// </summary>
        /// <param name="codeModel">传入码表对象</param>
        void UpdateCode(List<CodeModel> codeModels);
    }
}

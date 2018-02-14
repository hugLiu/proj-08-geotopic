using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;

namespace Jurassic.So.GeoTopic.DataService.Service
{
   public interface IDictTypeService
    {
        /// <summary>
        /// 类型查询
        /// </summary>
        /// <returns></returns>
       List<Dic_tTypeModel> GetTypeData();
        /// <summary>
        /// 类型的增删改
        /// </summary>
        /// <param name="typeModels"></param>
        void UpdateDictType(List<Dic_tTypeModel> typeModels,string userName);
        /// <summary>
        /// 根据类型id查询url
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<Dic_tUrlModel> QueryUrlData(int typeId);
        /// <summary>
        /// Url的增删改
        /// </summary>
        /// <param name="urlModels"></param>
        /// <param name="typeId"></param>
        void UpdataDictUrl(List<Dic_tUrlModel> urlModels,int typeId,string userName);
        /// <summary>
        /// 查询所有Type
        /// </summary>
        /// <returns></returns>
        List<Dic_tTypeModel> QueryDType();

        /// <summary>
        /// 根据type的id查询Url
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<Dic_tUrlModel> QueryDUrl(int typeId);

        /// <summary>
        /// 获取所有的页面展示Url
        /// </summary>
        /// <returns></returns>
       List<Dic_tUrlModel> QueryAllUrl();


        /// <summary>
        /// 通过相应展示类型的展示Url
        /// </summary>
        /// <returns></returns>
        List<Dic_tUrlModel> QueryAllUrlByTypeCode();
    }
}

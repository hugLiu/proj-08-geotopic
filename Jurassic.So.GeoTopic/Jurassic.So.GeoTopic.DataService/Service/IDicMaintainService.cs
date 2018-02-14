using System.Collections.Generic;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface IDicMaintainService
    {
        /// <summary>
        /// 获取知识卡的默认参数模板
        /// </summary>
        /// <returns></returns>
        List<PTempModel> GetPTemps();


        /// <summary>
        /// 获取知识卡的默认参数模板
        /// </summary>
        /// <returns></returns>
        List<PTempModel> GetPTempsByUrlId(int urlId);

        /// <summary>
        /// 根据展示页面获取相应的参数模板数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        PTempModel GetPTempsByUrl(string url);

        /// <summary>
        /// 获取主题索引元数据名称集合
        /// </summary>
        /// <returns></returns>
        List<Dic_tIndexModel> GetDictIndexs();

        /// <summary>
        /// 更新DicTIndex
        /// </summary>
        /// <param name="dicTIndexModels"></param>
        void UpdateGeoDicTIndexs(List<Dic_tIndexModel> dicTIndexModels);

        /// <summary>
        /// 更新参数模板
        /// </summary>
        /// <param name="pTempModels"></param>
        void UpdateGeoPTemps(PTempModel pTempModels);
    }
}
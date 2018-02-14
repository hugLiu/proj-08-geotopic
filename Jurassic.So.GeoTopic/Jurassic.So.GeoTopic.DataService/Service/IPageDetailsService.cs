using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface IPageDetailsService
    {
        /// <summary>
        /// 获取主题树集合
        /// </summary>
        /// <returns></returns>
        List<KTopicModel> GetGeoKTopics();

        /// <summary>
        /// 获取知识卡片集合
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        List<TCardModel> GetGeoTCards(int topicId);

        /// <summary>
        /// 通过Id获取知识卡片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TCardModel GetGeoTCardById(int id);

        /// <summary>
        /// 获取主题的Tab页渲染卡片集合
        /// </summary>
        /// <param name="topicId">主题Id</param>
        /// <returns></returns>
        List<TCardModel> GetLayoutTabs(int topicId);

        /// <summary>
        /// 获取主题索引集合
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        List<TIndexModel> GetTIndexs(string topicId);

        /// <summary>
        /// 获取主题索引属性集合
        /// </summary>
        /// <returns></returns>
        List<Dic_tIndexModel> GetDictIndexs();

        /// <summary>
        /// 获取具体TCard的具体信息
        /// </summary>
        /// <param name="Id">TCard的标识Id</param>
        /// <returns></returns>
        TCardModel GetTCardContent(int Id);
        /// <summary>
        /// 更新KTopicsTree
        /// </summary>
        /// <param name="kTopicModels"></param>
        void UpdateGeoKTopics(List<KTopicModel> kTopicModels);

        /// <summary>
        /// 添加新节点到KTopicsTree
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddNewNodeToGeoKTopics(KTopicModel model);

        /// <summary>
        /// 更新TIndex
        /// </summary>
        /// <param name="tIndexModels"></param>
        void UpdateGeoTIndexs(List<TIndexModel> tIndexModels);


        /// <summary>
        /// 保存知识卡片
        /// </summary>
        /// <param name="Id">知识卡Id</param>
        /// <param name="topicId">主题Id</param>
        /// <param name="title">标题</param>
        /// <param name="data">Definition节点数据</param>
        /// <param name="orderId">排序值</param>
        void SaveGeoTCard(int Id,int topicId, string title, string data, int orderId);

        /// <summary>
        /// 保存知识卡信息
        /// </summary>
        /// <param name="Id">知识卡Id</param>
        /// <param name="topicId">主题Id</param>
        /// <param name="title">标题</param>
        /// <param name="data">Definition节点数据</param>
        /// <param name="orderId">排序值</param>
        void SaveGeoTCardNoConvert(int Id, int topicId, string title, string data, int orderId);

        /// <summary>
        /// 更新知识卡信息
        /// </summary>
        /// <param name="Id">知识卡Id</param>
        /// <param name="topicId">主题Id</param>
        /// <param name="title">标题</param>
        /// <param name="data">Definition节点数据</param>
        /// <param name="orderId">排序值</param>
        void UpdateGeoTCard(int Id, int topicId, string title, string data, int orderId);

        /// <summary>
        /// 删除具体某一条TCard记录
        /// </summary>
        /// <param name="Id"></param>
        void DeleteTcard(int Id);
    
        /// <summary>
        /// 更新导入的索引属性信息
        /// </summary>
        /// <param name="tIndexModels"></param>
        void UpdateImportTIndexs(List<ImportTIndexModel> tIndexModels);

        /// <summary>
        /// 导入知识卡片信息
        /// </summary>
        /// <param name="tCardModels">知识卡模型数据集合</param>
        void UpdateImportTCard(List<TCardModel> tCardModels);


        string ConvertToCardDef(string cardViewStr);


        string ConvertToCardView(string defModelstr);

    }
}

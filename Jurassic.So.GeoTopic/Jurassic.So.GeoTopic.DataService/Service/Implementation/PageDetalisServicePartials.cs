using System.Collections.Generic;
using System.Linq;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using AutoMapper;

namespace Jurassic.So.GeoTopic.DataService
{
    public partial class PageDetailsService
    {

        public List<TCardModel> GetLayoutTabs(int topicId)
        {
            var tCards = GT_TopicCard.FindList(t => t.TopicId == topicId, t => t.OrderId, -1);
            if (tCards == null)
                return new List<TCardModel>();
            return tCards.Select(Mapper.Map<GT_TopicCard, TCardModel>).ToList();
        }

        //获取单一tab页面信息
        public TCardModel GetTCardContent(int Id)
        {
            var tCard = GT_TopicCard.Find(t => t.Id == Id);
            var tCardmodel = AutoMapper.Mapper.Map<GT_TopicCard, TCardModel>(tCard);
            if (tCardmodel == null)
                return new TCardModel();

            //获取到数据库Definition这个节点数据，转化成前端Viewmodel展示
            if (!string.IsNullOrEmpty(tCardmodel.Definition))
            {
                var cardDefModel = JsonUtil.JsonToObject(tCardmodel.Definition, typeof(CardDefModel)) as CardDefModel;
                var layoutView = JurassicConvert.ToCardView(cardDefModel);
                var LayoutJson = JsonUtil.ObjectToJson(layoutView);
                tCardmodel.Definition = LayoutJson;
            }
            return tCardmodel;
        }

        /// <summary>
        /// 数据库模型转化成可视化展示模型
        /// </summary>
        /// <param name="defModelstr"></param>
        /// <returns></returns>
        public string ConvertToCardView(string defModelstr)
        {
            if (string.IsNullOrEmpty(defModelstr))
                return string.Empty;
            var cardDefModel = JsonUtil.JsonToObject(defModelstr, typeof(CardDefModel)) as CardDefModel;
            var layoutView = JurassicConvert.ToCardView(cardDefModel);
            return JsonUtil.ObjectToJson(layoutView);
        }







        //public List<Node> GetParma(int Id)
        //{
        //    List<Node> nodes = new List<Node>();
        //    var tCard = GT_TopicCard.Find(t => t.Id == Id);
        //    var tCardmodel = AutoMapper.Mapper.DynamicMap<GT_TopicCard, TCardModel>(tCard);
        //    if (tCardmodel == null)
        //        return nodes;

        //    //获取到数据库Definition这个节点数据，转化成前端Viewmodel展示
        //    if (!string.IsNullOrEmpty(tCardmodel.Definition))
        //    {
        //        var cardDefModel = JsonUtil.JsonToObject(tCardmodel.Definition, typeof(CardDefModel)) as CardDefModel;
        //        var layoutView = JurassicConvert.ToCardView(cardDefModel);

        //        var paraObj = layoutView.component.FirstOrDefault().param;

        //        return paraObj;
        //        //var paraJson = JsonUtil.ObjectToJson(paraObj);
        //        //tCardmodel.Definition = paraJson;
        //    }
        //    return nodes;
        //}


        public void DeleteTcard(int Id)
        {
            var tCard = GT_TopicCard.Find(t => t.Id == Id);
            GT_TopicCard.Delete(tCard);
        }
    }
}

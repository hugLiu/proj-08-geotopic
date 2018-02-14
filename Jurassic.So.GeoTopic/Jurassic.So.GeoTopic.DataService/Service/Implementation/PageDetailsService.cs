using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Enum;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using Newtonsoft.Json;

namespace Jurassic.So.GeoTopic.DataService
{
    public partial class PageDetailsService : IPageDetailsService
    {
        public IGT_TopicEFRepository GT_Topic { get; set; }


        public IGT_TopicIndexEFRepository GT_TopicIndex { get; set; }

        
        //public IGeoDictIndexEFRepository GeoDictIndex { get; set; }
        public IGT_IndexDefinitionEFRepository GT_IndexDefinition { get; set; }


        public IGT_TopicCardEFRepository GT_TopicCard { get; set; }



        public IUserProfileEFRepository UserProfile { get; set; }

        public PageDetailsService(IGT_TopicEFRepository gt_Topic, IGT_TopicIndexEFRepository gt_TopicIndex, IGT_IndexDefinitionEFRepository gt_IndexDefinition, IGT_TopicCardEFRepository gt_TopicCard, IUserProfileEFRepository userProfile)
        {
            GT_Topic = gt_Topic;
            GT_TopicIndex = gt_TopicIndex;
            GT_IndexDefinition = gt_IndexDefinition;
            GT_TopicCard = gt_TopicCard;
            UserProfile = userProfile;
        }

        public List<KTopicModel> GetGeoKTopics()
        {
            var geoKTopics = GT_Topic.GetAll();
            return geoKTopics.Select(AutoMapper.Mapper.Map<GT_Topic, KTopicModel>).ToList();
        }

        public List<TIndexModel> GetTIndexs(string topicId)
        {
            //int id = Convert.ToInt32(topicId);
            //var geoTIndexs = GeoTIndex.GetQueryDb()
            //    .Include("Geo_Dic_tIndex")
            //    .Where(t => t.TopicId == id);
            //return geoTIndexs.Select(AutoMapper.Mapper.Map<Geo_tIndex, TIndexModel>).ToList();

            



            int id = Convert.ToInt32(topicId);

            //var query = from t in Database.Models.GT_TopicIndex join i in Database.Models.GT_IndexDefinition on t.IndexDefinitionCode equals i.Code
            //            where t.TopicId == id
            //            select new
            //            {
            //            };

            //var context=new GeoDbContext();

            using (var context = new GeoDbContext())
            {
                var query =
                from t in context.GT_TopicIndex
                join d in context.GT_IndexDefinition on t.IndexDefinitionId equals d.Id
                where t.TopicId == id
                select new TIndexModel()
                {
                    Id = t.Id,
                    TopicId = id,
                    IndexDefinitionId = t.IndexDefinitionId,
                    IndexDefinitionTitle = d.Title,
                    Value = t.Value
                };
                return query.ToList();
            }
        }




        public List<TCardModel> GetGeoTCards(int topicId)
        {
            var geoTCards = GT_TopicCard.GetQuery().Where(t => t.TopicId == topicId);
            return geoTCards.Select(AutoMapper.Mapper.Map<GT_TopicCard, TCardModel>).ToList();
        }

        public TCardModel GetGeoTCardById(int id)
        {
            int Id = Convert.ToInt32(id);
            var geoTCards = GT_TopicCard.GetQuery().Where(t => t.Id == Id);
            return geoTCards.Select(AutoMapper.Mapper.Map<GT_TopicCard, TCardModel>).ToList().FirstOrDefault();
        }
        public void UpdateGeoKTopics(List<KTopicModel> kTopicModels)
        {
            foreach (var kTopicModel in kTopicModels)
            {
                if (kTopicModel._state == EnumNodeState.added.ToString())
                {
                    AddGeoKTopic(kTopicModel.PId, kTopicModel.Title);
                }
                if (kTopicModel._state == EnumNodeState.modified.ToString())
                {
                    ModifyGeoKTopic(kTopicModel.Id, kTopicModel.Title);
                }
                if (kTopicModel._state == EnumNodeState.removed.ToString())
                {
                    RemoveGeoKTopic(kTopicModel.Id);
                }
            }
        }

        public int AddNewNodeToGeoKTopics(KTopicModel model)
        {
            return AddGeoKTopic(model.PId, model.Title);
        }

        private int AddGeoKTopic(int? pId, string title)
        {

            var pCode = GetNewPCode(pId);

            var geoKTopic = new GT_Topic
            {
                PId = pId,
                Code = pCode,
                Title = title,
                CreatedDate = DateTime.Now,
                CreatedBy = "pmis",
                UpdatedDate = DateTime.Now,
                IsDelete = false
            };

            GT_Topic.Add(geoKTopic);
            return geoKTopic.Id;
        }

        private void ModifyGeoKTopic(int id, string title)
        {
            var geoKTopic = GT_Topic.Find(t => t.Id == id);
            if (geoKTopic == null) return;
            geoKTopic.Title = title;
            GT_Topic.Update(geoKTopic);
        }

        private void RemoveGeoKTopic(int id)
        {
            var geoKTopic = GT_Topic.Find(t => t.Id == id);
            if (geoKTopic == null) return;

            //级联删除roles
            var roles = geoKTopic.webpages_Roles.Where(t => t.GT_Topic.Any(o => o.Id == id)).FirstOrDefault();
            if (roles != null)
            {
                geoKTopic.webpages_Roles.Remove(roles);
            }

            //级联删除index
            var geoTIndexList = GT_TopicIndex.FindList<int>(t => t.TopicId == id, t => t.Id, 1);
            if (geoTIndexList != null)
            {
                foreach (var geoTIndex in geoTIndexList)
                {
                    GT_TopicIndex.Delete(geoTIndex);
                }
            }
            //级联删除TCard
            var getTCardList = GT_TopicCard.FindList<int>(t => t.TopicId == id, t => t.Id, 1);
            if (getTCardList != null)
            {
                foreach (var geoTCard in getTCardList)
                {
                    GT_TopicCard.Delete(geoTCard);
                }
            }
            GT_Topic.Delete(geoKTopic);
        }

        ///// <summary>
        ///// 获取新插入节点的PCode
        ///// </summary>
        ///// <param name="pId">父节点Id</param>
        ///// <returns></returns>
        private string GetNewPCode(int? pId)
        {
            string pCode = "";
            if (pId == null)
            {
                var geoKTopics =
                    GT_Topic.FindList<string>(t => t.Code.Length == 3, t => t.Code, 1);

                if (geoKTopics != null)
                    pCode = geoKTopics.Select(t => t.Code).Max();
                pCode = pCode == ""
                    ? "001"
                    : (Convert.ToInt32(pCode) + 1).ToString().PadLeft(3, '0');

            }
            else
            {
                var geoKTopic = GT_Topic.Find(t => t.Id == pId);
                if (geoKTopic != null)
                    pCode = geoKTopic.Code;
                var code = pCode;

                var geoKTopics =
                    GT_Topic.FindList<string>(
                        t => t.Code.StartsWith(code) && t.Code.Length == code.Length + 3, t => t.Code, 1);

                pCode = geoKTopics != null ? geoKTopics.Select(t => t.Code).Max() : "";
                pCode = pCode == ""
                    ? code + "001"
                    : pCode.Substring(0, pCode.Length - 3) + 
                       (Convert.ToInt32(pCode.Substring(pCode.Length - 3, 3)) + 1).ToString()
                       .PadLeft(3, '0');
            }
            return pCode;
        }

        public List<Dic_tIndexModel> GetDictIndexs()
        {
            var geoDictIndex = GT_IndexDefinition.GetAll();
            return geoDictIndex.Select(AutoMapper.Mapper.Map<GT_IndexDefinition, Dic_tIndexModel>).ToList();
        }

        public void UpdateGeoTIndexs(List<TIndexModel> tIndexModels)
        {
            foreach (var tIndexModel in tIndexModels)
            {
                if (tIndexModel._state == EnumNodeState.added.ToString())
                {
                    AddGeoTIndex(tIndexModel.Id, tIndexModel.TopicId, tIndexModel.IndexDefinitionId, tIndexModel.Value);
                }
                if (tIndexModel._state == EnumNodeState.modified.ToString())
                {
                    ModifyGeoTIndex(tIndexModel.Id, tIndexModel.IndexDefinitionId, tIndexModel.Value);
                }
                if (tIndexModel._state == EnumNodeState.removed.ToString())
                {
                    RemoveGeoGeoTIndex(tIndexModel.Id);
                }
            }
        }

        //更新导入的索引属性信息
        public void UpdateImportTIndexs(List<ImportTIndexModel> tIndexModels)
        {
            //清除原节点上索引项信息
            var topicId = tIndexModels.Select(t => t.TopicId).FirstOrDefault();
            var deleTIndexList = GT_TopicIndex.FindList(t=>t.TopicId== topicId, t=>t.Id,1);
            if ((deleTIndexList!=null) && (deleTIndexList.Count() > 0))
            {
                GT_TopicIndex.DeleteList(deleTIndexList);
            }

            foreach (var impportTindex in tIndexModels)
            {
                var currentIndexDef = GT_IndexDefinition.Find(t => t.Code == impportTindex.IndexDefinitionCode);
                if (currentIndexDef == null)
                {
                    //导入GT_IndexDefinition记录没有对应title值
                    //GT_IndexDefinition.Add(new GT_IndexDefinition()
                    //{
                    //    Code = impportTindex.IndexDefinitionCode,
                    //    CreatedDate = DateTime.Now,
                    //    CreatedBy = "pmis",
                    //    IsDelete = false,
                    //    Title = "",
                    //    UpdatedDate = DateTime.Now
                    //});
                    //var indexDefId = GT_IndexDefinition.Find(t => t.Code == impportTindex.IndexDefinitionCode).Id;
                    //AddGeoTIndex(impportTindex.Id, impportTindex.TopicId, indexDefId, impportTindex.Value);
                }
                else
                {
                    AddGeoTIndex(impportTindex.Id, impportTindex.TopicId, currentIndexDef.Id, impportTindex.Value);
                }
            }
        }

        //更新导入的索引参数信息
        public void UpdateImportTCard(List<TCardModel> tCardModels)
        {
            //存在原来tCard信息删除
            var topicId=tCardModels.Select(t => t.TopicId).FirstOrDefault();
            var deletCardList = GT_TopicCard.FindList(t => t.TopicId == topicId, t => t.Id, 1);
            if (deletCardList!=null&& deletCardList.Count()>0)
            {
                GT_TopicCard.DeleteList(deletCardList);
            }
            //添加插入的tCard信息
            foreach (var tcard in tCardModels)
            {
                AddGeoTCard(tcard.Id,tcard.TopicId,tcard.Title,tcard.Definition,tcard.OrderId);
            }
        }



        private void AddGeoTIndex(int id, int topicId, int indexDefinitionId, string value)
        {
            var geoTIndex = new GT_TopicIndex()
            {
                TopicId = topicId,
                IndexDefinitionId = indexDefinitionId,
                Value = value,
                CreatedDate = DateTime.Now,
                IsDelete = false,
                UpdatedDate=DateTime.Now,
            };
            GT_TopicIndex.Add(geoTIndex);
        }

        private void ModifyGeoTIndex(int id, int indexDefinitionId, string value)
        {
            var geoTIndex = GT_TopicIndex.Find(t => t.Id == id);
            if (geoTIndex == null) return;
            geoTIndex.IndexDefinitionId = indexDefinitionId;
            geoTIndex.Value = value;
            geoTIndex.UpdatedDate=DateTime.Now;
            GT_TopicIndex.Update(geoTIndex);
        }
        private void RemoveGeoGeoTIndex(int id)
        {
            var geoTIndex = GT_TopicIndex.Find(t => t.Id == id);
            if (geoTIndex == null) return;
            GT_TopicIndex.Delete(geoTIndex);
        }

        public void SaveGeoTCard(int Id, int topicId, string title, string data, int orderId)
        {
            CardViewModel cardView = new CardViewModel();
            cardView = JsonConvert.DeserializeObject<CardViewModel>(data);

            CardDefModel cardDef = JurassicConvert.ToCardDef(cardView);
            string json = JsonConvert.SerializeObject(cardDef);

            var geoTCards = GT_TopicCard.FindList(t => t.TopicId == topicId, t => t.OrderId, -1);
            if (geoTCards == null)
            {
                AddGeoTCard(Id, topicId, title, data, orderId);
            }
            else
            {
                var updateGeoTcard = GT_TopicCard.Find(t => t.Id == Id);
                //var updateGeoTcard = geoTCards.Where(t => t.Id == Id).FirstOrDefault();
                //编辑tab页面
                if (updateGeoTcard != null)
                {
                    updateGeoTcard.Definition = json;
                    GT_TopicCard.Update(updateGeoTcard);
                }
                else   //新增的tab页面
                {
                    AddGeoTCard(Id, topicId, title, data, orderId);
                }
            }
        }


        /// <summary>
        /// 前端可视化展示模型转化成数据库保存模型
        /// </summary>
        /// <param name="cardViewStr"></param>
        /// <returns></returns>
        public string ConvertToCardDef(string cardViewStr)
        {
            if (string.IsNullOrEmpty(cardViewStr))
                return string.Empty;
            var cardView = JsonConvert.DeserializeObject<CardViewModel>(cardViewStr);
            CardDefModel cardDef = JurassicConvert.ToCardDef(cardView);
            return JsonConvert.SerializeObject(cardDef);
        }





        public void SaveGeoTCardNoConvert(int Id, int topicId, string title, string data, int orderId)
        {
            var geoTCards = GT_TopicCard.FindList(t => t.TopicId == topicId, t => t.OrderId, -1);
            if (geoTCards == null)
            {
                AddGeoTCard(Id, topicId, title, data, orderId);
            }
            else
            {
                var updateGeoTcard = geoTCards.Where(t => t.Title == title).FirstOrDefault();
                //var updateGeoTcard = geoTCards.Where(t => t.Id == Id).FirstOrDefault();
                //编辑tab页面
                if (updateGeoTcard != null)
                {
                    updateGeoTcard.Title = title;
                    updateGeoTcard.Definition = data;
                    updateGeoTcard.OrderId = orderId;
                    updateGeoTcard.UpdatedDate = DateTime.Now;
                    GT_TopicCard.Update(updateGeoTcard);
                }
                else   //新增的tab页面
                {
                    AddGeoTCard(Id, topicId, title, data, orderId);
                }
            }
        }


        public void UpdateGeoTCard(int Id, int topicId, string title, string data, int orderId)
        {
            var geoTCards = GT_TopicCard.FindList(t => t.TopicId == topicId, t => t.OrderId, -1);
            if (geoTCards == null)
            {
                AddGeoTCard(Id, topicId, title, data, orderId);
            }
            else
            {
                var updateGeoTcard = geoTCards.Where(t => t.Id == Id).FirstOrDefault();
                //编辑tab页面
                if (updateGeoTcard != null)
                {
                    updateGeoTcard.Title = title;
                    //updateGeoTcard.Definition = data;
                    updateGeoTcard.OrderId = orderId;
                    updateGeoTcard.UpdatedDate = DateTime.Now;
                    GT_TopicCard.Update(updateGeoTcard);
                }
                else   //新增的tab页面
                {
                    AddGeoTCard(Id, topicId, title, data, orderId);
                }
            }

        }

        public void AddGeoTCard(int Id, int topicId, string title, string data, int orderId)
        {
            GT_TopicCard geoTCard = new GT_TopicCard()
            {
                TopicId = topicId,
                Title = title,
                Definition = data,
                CreatedDate = DateTime.Now,
                CreatedBy = "pmis",
                IsDelete = false,
                UpdatedDate = DateTime.Now,
                OrderId = orderId
            };
            GT_TopicCard.Add(geoTCard);
        }

        
    }
}

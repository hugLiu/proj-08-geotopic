using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Jurassic.So.GeoTopic.DataService.Enum;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.Database;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public class DicMaintainService : IDicMaintainService
    {
        public IGT_IndexDefinitionEFRepository gt_IndexDefinition { get; set; }
        public IGT_UrlTemplateEFRepository gt_UrlTemplate { get; set; }

        public IGT_RenderUrlEFRepository gt_RenderUrl { get; set; }
        public IGT_TopicIndexEFRepository gt_TopicIndex { get; set; }

        public DicMaintainService(IGT_IndexDefinitionEFRepository gtIndexDefinition, IGT_UrlTemplateEFRepository gtUrlTemplate, IGT_RenderUrlEFRepository gtRenderUrl, IGT_TopicIndexEFRepository gtTopicIndex)
        {
            gt_IndexDefinition = gtIndexDefinition;
            gt_UrlTemplate = gtUrlTemplate;
            gt_RenderUrl = gtRenderUrl;
            gt_TopicIndex = gtTopicIndex;  //把声明数据对象操作接口注册到Service构造函数中进行实例化  Walt add
        }

        public List<PTempModel> GetPTemps()
        {
            var geoPTemp = gt_UrlTemplate.GetAll();
            return geoPTemp.Select(AutoMapper.Mapper.Map<GT_UrlTemplate, PTempModel>).ToList();
        }

        public List<PTempModel> GetPTempsByUrlId(int urlId)
        {
            var geoPTemp = gt_UrlTemplate.GetAll().Where(t => t.RenderUrlId == urlId);
            return geoPTemp.Select(AutoMapper.Mapper.Map<GT_UrlTemplate, PTempModel>).ToList();
        }


        public PTempModel GetPTempsByUrl(string url)
        {
            var currentUrl = gt_RenderUrl.Find(t => t.Url.Trim() == url);
            if(currentUrl==null)
                return new PTempModel();
            var urlTemplate = gt_UrlTemplate.Find(t => t.RenderUrlId == currentUrl.Id);
            if (urlTemplate==null)
                return new PTempModel();
            return   AutoMapper.Mapper.Map<GT_UrlTemplate, PTempModel>(urlTemplate);
        }


        public List<Dic_tIndexModel> GetDictIndexs()
        {
            var geoDictIndex = gt_IndexDefinition.GetAll().OrderByDescending(o=>o.UpdatedDate);
            return geoDictIndex.Select(AutoMapper.Mapper.Map<GT_IndexDefinition, Dic_tIndexModel>).ToList();
        }
        public void UpdateGeoDicTIndexs(List<Dic_tIndexModel> dicTIndexModels)
        {
            foreach (var dicTIndexModel in dicTIndexModels)
            {
                if (dicTIndexModel._state == EnumNodeState.added.ToString())
                {
                    //if (gt_IndexDefinition.GetAll().FirstOrDefault(o => o.Code == dicTIndexModel.Code) != null) return;

                    AddGeoDicTIndex(dicTIndexModel.Title, dicTIndexModel.Code);
                }
                if (dicTIndexModel._state == EnumNodeState.modified.ToString())
                {
                    ModifyGeoDicTIndex(dicTIndexModel.Id, dicTIndexModel.Title,dicTIndexModel.Code);
                }
                if (dicTIndexModel._state == EnumNodeState.removed.ToString())
                {
                    RemoveGeoDicTIndex(dicTIndexModel.Code);
                }
            }
        }

        private void AddGeoDicTIndex(string name,string code)
        {
            var geoDictIndex = new GT_IndexDefinition()
            {
                Title = name,
                Code = code,
                CreatedDate = DateTime.Now,
                CreatedBy = "pmis",
                IsDelete = false,
                UpdatedDate= DateTime.Now
            };
            gt_IndexDefinition.Add(geoDictIndex);
            //try
            //{

            //    gt_IndexDefinition.Add(geoDictIndex);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private void ModifyGeoDicTIndex(int id, string name,string code)
        {
            var geoDictIndex = gt_IndexDefinition.Find(t => t.Id == id);
            if (geoDictIndex == null) return;
            geoDictIndex.Title = name;
            geoDictIndex.Code = code;
            geoDictIndex.UpdatedDate = DateTime.Now;
            gt_IndexDefinition.Update(geoDictIndex);

            //try
            //{
            //    gt_IndexDefinition.Update(geoDictIndex);

            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        private void RemoveGeoDicTIndex(string code)
        {
            var geoDictIndex = gt_IndexDefinition.Find(t => t.Code == code.Trim());
            if (geoDictIndex == null) return;         
            var topicIndexData = gt_TopicIndex.GetAll().Where(o=>o.IndexDefinitionId== geoDictIndex.Id);
            gt_TopicIndex.DeleteList(topicIndexData);
            gt_IndexDefinition.Delete(geoDictIndex);

            //try
            //{

            //    gt_IndexDefinition.Delete(geoDictIndex);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        public void UpdateGeoPTemps(PTempModel tempModel)
        {
            var urlTemplate = gt_UrlTemplate.Find(t => t.RenderUrlId == tempModel.RenderUrlId);
            if (urlTemplate == null)
            {
                AddGeoPTemp(tempModel);
            }
            else
            {
                gt_UrlTemplate.Delete(urlTemplate);
                AddGeoPTemp(tempModel);
            }

            //foreach (var pTempModel in pTempModels)
            //{
            //    if (pTempModel._state == EnumNodeState.added.ToString())
            //    {
            //        AddGeoPTemp(pTempModel);
            //    }
            //    if (pTempModel._state == EnumNodeState.modified.ToString())
            //    {
            //        ModifyGeoPTemp(pTempModel);
            //    }
            //    if (pTempModel._state == EnumNodeState.removed.ToString())
            //    {
            //        RemoveGeoPTemp(pTempModel.Id);
            //    }
            //}
        }

        private void AddGeoPTemp(PTempModel pTempModel)
        {
            var geoGeoPTemp = new GT_UrlTemplate()
            {
                RenderUrlId = pTempModel.RenderUrlId,
                Defintion = pTempModel.Defintion,
                Type = pTempModel.Type,
                CreatedDate = DateTime.Now,
                CreatedBy= "pmis",
                UpdatedDate = DateTime.Now,
                IsDelete = false

            };
            gt_UrlTemplate.Add(geoGeoPTemp);
        }

        //private void ModifyGeoPTemp(PTempModel pTempModel)
        //{
        //    var geoGeoPTemp = gt_UrlTemplate.Find(t => t.Id == pTempModel.Id);
        //    if (geoGeoPTemp == null) return;
        //    geoGeoPTemp.Type = pTempModel.Type;
        //    geoGeoPTemp.RenderUrlId = pTempModel.RenderUrlId;
        //    gt_UrlTemplate.Update(geoGeoPTemp);
        //}

        //private void RemoveGeoPTemp(int id)
        //{
        //    var geoGeoPTemp = gt_UrlTemplate.Find(t => t.Id == id);
        //    if (geoGeoPTemp == null) return;
        //    gt_UrlTemplate.Delete(geoGeoPTemp);
        //}
    }
}
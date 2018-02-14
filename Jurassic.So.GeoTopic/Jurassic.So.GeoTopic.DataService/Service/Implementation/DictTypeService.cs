using System;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.Database.Models;
using System.Collections.Generic;
using System.Linq;
using Jurassic.So.GeoTopic.DataService.Enum;
using Jurassic.So.GeoTopic.DataService.Tool;


namespace Jurassic.So.GeoTopic.DataService.Service.Implementation
{
    public class DictTypeService : IDictTypeService
    {
        public IGT_RenderTypeEFRepository DictTypeContent;
        public IGT_RenderUrlEFRepository DictUrlContent;

        public IGT_UrlTemplateEFRepository UrlTempContent;

        public DictTypeService(IGT_RenderTypeEFRepository dictTypeContent, IGT_RenderUrlEFRepository dictUrlContent, IGT_UrlTemplateEFRepository urlTempContent)
        {
            DictTypeContent = dictTypeContent;
            DictUrlContent = dictUrlContent;
            UrlTempContent = urlTempContent;
        }
        //--------------------type--------------------------
        /// <summary>
        /// 类型的查询
        /// </summary>
        /// <returns></returns>
        public List<Dic_tTypeModel> GetTypeData()
        {
            var types = DictTypeContent.GetAll().OrderByDescending(o=>o.UpdatedDate);
            var data = types.Select(AutoMapper.Mapper.Map<GT_RenderType, Dic_tTypeModel>).ToList();
            return data;
        }
        /// <summary>
        /// 类型的增删改
        /// </summary>
        /// <param name="typeModels"></param>
        public void UpdateDictType (List<Dic_tTypeModel> typeModels,string userName)
        {
            foreach (var TempModel in typeModels)
            {
                if (TempModel._state == EnumNodeState.added.ToString())
                {
                   addDictType(TempModel.Title, TempModel.Code,userName);
                }
                if (TempModel._state == EnumNodeState.modified.ToString())
                {
                   modifyDictType(TempModel.Id, TempModel.Title, TempModel.Code,userName);
                }
                if (TempModel._state == EnumNodeState.removed.ToString())
                {
                   removeDictType(TempModel.Id);
                }
            }          
        }
        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="type"></param>
        public void  addDictType(string title, string code, string userName)
        {
            //var temp = queryCode(code);
            //if (temp)
            //{
            //    return '"' + code + '"' + "已存在！";
            //}
            var model = new GT_RenderType()
            {
                Title = title.Trim(),
                Code = code.Trim(),
                CreatedBy=userName,
                CreatedDate=DateTime.Now,
                UpdatedDate=DateTime.Now
            };
            try
            {
                DictTypeContent.Add(model);
            }
            catch (Exception)
            {
                throw;
            }
            
           
        }
        /// <summary>
        /// 编辑类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public void modifyDictType(int id, string title, string code, string userName)
        {
            //if (DictTypeContent.Find(o=>o.Code==code&&o.Id!=id)!=null)
            //{
            //    return '"'+code+ '"'+ "已存在！";
            //}
            var model = DictTypeContent.Find(o => o.Id == id);
            //if (model == null) return "已删除！";
            model.Title = title;
            model.Code =code;
            model.UpdatedDate = DateTime.Now;
            model.UpdatedBy = userName;
            try
            {
                DictTypeContent.Update(model);
            }
            catch (Exception)
            {
                throw;
            }
            //return "保存成功！";
        }
        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="id"></param>
        public void removeDictType(int typeId)
        {
            var model = DictTypeContent.Find(o => o.Id == typeId);
            //if (model == null) return "已被删除！";
            var urls = DictUrlContent.GetAll().Where(o => o.RenderTypeId == typeId).ToList();
            if (urls.Count > 0)
            {
                foreach (var item in urls)
                {
                    removeDictUrl(item.Id);
                }
            }
            try
            {
                DictTypeContent.Delete(model);              
            }
            catch (Exception)
            {
                throw;
            }
            //return "删除成功！";
        }
        //public bool queryCode(string code)
        //{
        //    var list = DictTypeContent.Find(o => o.Code == code);
        //    if (list!=null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //-----------------url---------------------
        /// <summary>
        /// 查询TypeId=id类型的Url
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Dic_tUrlModel> QueryUrlData(int typeId)
        {
            var urls = DictUrlContent.GetAll().Where(o => o.RenderTypeId == typeId).OrderByDescending(d=>d.UpdatedDate);
            var data = urls.Select(AutoMapper.Mapper.Map<GT_RenderUrl, Dic_tUrlModel>).ToList();         
            return data;
        }
        /// <summary>
        /// 保存Url变更后的数据
        /// </summary>
        /// <param name="urlModels"></param>
        /// <param name="id"></param>
        public void UpdataDictUrl(List<Dic_tUrlModel> urlModels, int typeId,string userName)
        {
            foreach (var TempModel in urlModels)
            {
                if (TempModel._state == EnumNodeState.added.ToString())
                {
                    addDictUrl(TempModel.Url, typeId, TempModel.Description,userName);
                }
                if (TempModel._state == EnumNodeState.modified.ToString())
                {
                    modifyDictUrl(TempModel.Id, TempModel.Url, TempModel.Description,userName);
                }
                if (TempModel._state == EnumNodeState.removed.ToString())
                {
                    removeDictUrl(TempModel.Id);
                }
            }
        }
        public void addDictUrl(string url, int typeId, string description,string userName)
        {
            if (DictTypeContent.GetAll().Select(o => o.Id == typeId).ToList().Count > 0)
            {
                var model = new GT_RenderUrl()
                {
                    RenderTypeId = typeId,
                    Url = url.Trim(),
                    Description = description==null?"":description,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userName,
                    UpdatedDate = DateTime.Now,
                };
                try
                {
                    DictUrlContent.Add(model);
                }
                catch (Exception)
                {
                    throw;
                }                
            }
        }
        public void modifyDictUrl(int id, string url, string description,string userName)
        {
            var model = DictUrlContent.Find(o => o.Id == id);
            if (model == null) return;
            model.Url = url.Trim();
            model.Description = description.Trim();
            model.UpdatedDate = DateTime.Now;
            model.UpdatedBy = userName;
            try
            {
                DictUrlContent.Update(model);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public void removeDictUrl(int id)
        {
            //先要删除存在该url参数模板表记录，UrlId是其外键字段
            var currentTemp=UrlTempContent.FindList(t => t.RenderUrlId == id,t=>t.Id,1);
            if(currentTemp!=null&&currentTemp.Count()>0)
                UrlTempContent.DeleteList(currentTemp);

            var model = DictUrlContent.GetAll().Find(o => o.Id == id);
            try
            {
                DictUrlContent.Delete(model);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public List<Dic_tTypeModel> QueryDType()
        {
            var types = DictTypeContent.GetAll().OrderBy(o=>o.Title);
            return types.Select(AutoMapper.Mapper.Map<GT_RenderType, Dic_tTypeModel>).ToList();
        }

        public List<Dic_tUrlModel> QueryDUrl(int typeId)
        {
            var urls = DictUrlContent.GetAll().Where(o => o.RenderTypeId == typeId).OrderBy(o=>o.Url);
            return urls.Select(AutoMapper.Mapper.Map<GT_RenderUrl, Dic_tUrlModel>).ToList();
        }

        public List<Dic_tUrlModel> QueryAllUrl()
        {
            var urls = DictUrlContent.GetAll().OrderBy(o=>o.Url);
            return urls.Select(AutoMapper.Mapper.Map<GT_RenderUrl, Dic_tUrlModel>).ToList();
        }


        public List<Dic_tUrlModel> QueryAllUrlByTypeCode()
        {
            var typeCode = System.Configuration.ConfigurationManager.AppSettings["TypeCode"];
            var renderType = DictTypeContent.Find(t => t.Code == typeCode);
            if(renderType==null)
                return new List<Dic_tUrlModel>();
            var urls = DictUrlContent.GetAll().Where(t => t.RenderTypeId == renderType.Id);
            return urls.Select(AutoMapper.Mapper.Map<GT_RenderUrl, Dic_tUrlModel>).ToList();
        }


    }
}

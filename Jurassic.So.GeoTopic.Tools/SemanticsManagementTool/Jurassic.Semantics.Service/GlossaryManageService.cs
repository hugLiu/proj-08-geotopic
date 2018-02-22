using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.Semantics.Service
{
    public class GlossaryManageService : IGlossaryManageService
    {
        public GlossaryManageEFPrrovied EfPrrovied { set; get; }

        public GlossaryManageService(GlossaryManageEFPrrovied efPrrovied)
        {
            EfPrrovied = efPrrovied;
        }

        #region 1.得到概念类树形数据+TreeSelect下拉树+GetBpModelList()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strccTerm">概念类名称</param>
        /// <returns>概念类树形数据</returns>
        public List<TermTreeModel> GetTermTree(string strccTerm)
        {
            var arrCcterminfor = EfPrrovied.GetCcTermArrayEf(strccTerm);
            var list = EfPrrovied.GetCctermTreeEf(arrCcterminfor[0], arrCcterminfor[1])
                .Select(c => new TermTreeModel
                {
                    TermClassId = c.TermClassId,
                    CreatedBy = c.CreatedBy,
                    CreatedDate = c.CreatedDate,
                    Description = c.Description,
                    PathTerm = c.PathTerm,
                    IsLeaf = c.IsLeaf,
                    lvl = c.lvl,
                    PId = c.PId,
                    OrderIndex = c.OrderIndex,
                    Source = c.Source,
                    Term = c.Term
                }).OrderBy(o => o.OrderIndex)
                .ToList();
            return list;
        }
        #endregion

        #region 2.获得树节点的来源+GetTermList(string termGuid)+新
        public List<TermSourceModel> GetSourceList(string strccTerm)
        {
            List<TermSourceModel> list = null;
            var arrCcterminfor = EfPrrovied.GetCcTermArrayEf(strccTerm);
            list = EfPrrovied.GetSourcesListEf(arrCcterminfor[0]).Select(AutoMapper.Mapper.Map<TermSourceModel>).ToList();
            return list;
        }
        #endregion

        #region 3.得到译文表数据+GetTermTraslations(Guid termclassid)+新
        public List<TermTranslationModel> GetTermTraslations(Guid id)
        {
            return EfPrrovied.GetTermTranslationsEf(id).Select(AutoMapper.Mapper.Map<TermTranslationModel>).ToList();
        }
        #endregion

        #region 4.删除节点+DeleteById(GetBpTreeDataModel getBpTreemodel)+新


        /// <summary>
        /// 批量删除节点
        /// </summary>
        /// <param name="termClassId">叙词id</param>
        public void DeleteCcTermByIdService(Guid termClassId)
        {
            if (EfPrrovied.IsCcTermExistEf(termClassId) > 0)
            {
                if (EfPrrovied.IsKeywordExistEf(termClassId) > 0)
                {
                    EfPrrovied.DeleteKeywordByIdEf(termClassId);
                }
                if (EfPrrovied.IsTranslationExistEf(termClassId) > 0)
                {
                    EfPrrovied.DeleteTranslationByIdEf(termClassId);
                }
                if (EfPrrovied.IsSemanticsExistEf(termClassId) > 0)
                {
                    EfPrrovied.DeleteSemanticsByIdEf(termClassId);
                }
                EfPrrovied.DeleteCcTermByIdEf(termClassId);
            }
        }
        #endregion

        #region 6.添加子节点+AddChildrenNode(Guid termGuid,CcTermModel newmodel)

        public List<CcTermModel> GetTermList(string cc)
        {
            return EfPrrovied.GetTermList(cc).Select(AutoMapper.Mapper.Map<CcTermModel>).ToList();
        }

        public void AddChildrenNode(Guid termGuid, CcTermModel newmodel, string strCcTerm)
        {
            EfPrrovied.AddNodeEf(termGuid, AutoMapper.Mapper.Map<SD_CCTerm>(newmodel), strCcTerm);
        }
        #endregion

        #region 7.编辑节点+EditTreeById(Guid termGuid, string term)
        public void EditTreeById(Guid termGuid, string term, string path)
        {
            EfPrrovied.EditCcTreeNameByIdEf(termGuid, term, path);
        }
        #endregion

        #region 8.保存描述+SaveMs(string strDescription, Guid termGuid)
        public void SaveMs(string strDescription, Guid termGuid)
        {
            EfPrrovied.SvaeDescriptionEf(strDescription, termGuid);
        }
        #endregion

        #region 9.添加来源+AddSource(TermSourceModel termSourceModel)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="termSourceModel">数据来源实体</param>
        /// <param name="strccterm">概念类名称</param>
        public void AddSource(TermSourceModel termSourceModel, string strccterm)
        {
            var arrCcterminfor = EfPrrovied.GetCcTermArrayEf(strccterm);
            var cccode = arrCcterminfor[0];
            EfPrrovied.AddSourceEf(AutoMapper.Mapper.Map<SD_TermSource>(termSourceModel), cccode);
        }
        #endregion

        #region 10.保存数据来源+SaveSource(string source, Guid termGuid)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">新的来源</param>
        /// <param name="termGuid">叙词id</param>
        public void SaveSource(string source, Guid termGuid)
        {
            EfPrrovied.SaveSourceEf(source, termGuid);
        }
        #endregion

        #region 11.删除数据来源+DeleteTermSourcrRowByName(string source)
        public void DeleteTermSourcrRowByName(string source)
        {
            EfPrrovied.DeleteSourceRow(source);
        }
        #endregion

        #region 12.添加译文表+SaveOtherName(TermTranslationModel tsTranslationModel)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tsTranslationModel">译文实体</param>
        public void SaveOtherName(TermTranslationModel tsTranslationModel)
        {
            SD_TermTranslation newTranslation = new SD_TermTranslation()
            {
                TermClassID = tsTranslationModel.TermClassID,
                CreatedBy = tsTranslationModel.CreatedBy,
                CreatedDate = tsTranslationModel.CreatedDate,
                IsMain = tsTranslationModel.IsMain,
                LangCode = tsTranslationModel.LangCode,
                LastUpdatedBy = tsTranslationModel.LastUpdatedBy,
                LastUpdatedDate = tsTranslationModel.LastUpdatedDate,
                Remark = tsTranslationModel.Remark,
                Translation = tsTranslationModel.Translation,
            };
            EfPrrovied.SaveotherNameEf(AutoMapper.Mapper.Map<SD_TermTranslation>(newTranslation));
        }
        #endregion

        #region 13.删除译文表数据+DeleteRow(Guid tuid)
        public void DeleteRow(Guid tuid, string tran)
        {
            EfPrrovied.DeleteTranslationEf(tuid, tran);
        }
        #endregion

        #region 14.拖拽节点
        /// <summary>
        /// 
        /// </summary>
        /// <param name="semantics">语义关系实体</param>
        /// <param name="dragmodel">拖拽的节点数据实体</param>
        /// <param name="strCcTerm">概念类名称</param>   
        public void DragNodeService(SemanticsModel semantics, CcTermModel dragmodel, string strCcTerm)
        {
            var arrCctermInfor = EfPrrovied.GetCcTermArrayEf(strCcTerm);
            EfPrrovied.DragNodeEf(AutoMapper.Mapper.Map<SD_Semantics>(semantics), AutoMapper.Mapper.Map<SD_CCTerm>(dragmodel), arrCctermInfor[1]);
        }
        #endregion

        public void UpdatePathTerm(Guid termClassId)
        {
            EfPrrovied.UpdatePathTerm(termClassId);
        }

        #region 15.节点排序
        public void SequenceService(Guid nodeId, string parentName, int newOrderIndex)
        {
            EfPrrovied.SequenceNodeEf(nodeId, parentName, newOrderIndex);
        }
        #endregion

        #region 16.来源数组
        /// <summary>
        /// 
        /// </summary>
        /// <param name="termId">叙词id</param>
        /// <returns></returns>
        public List<string> GetSourceListService(string termId)
        {
            var newtermid = Guid.Parse(termId);
            return EfPrrovied.GetsourceListEf(newtermid);
        }
        #endregion

        public List<SemanticsModel> GetTermAlias(Guid id)
        {
            var semanticsProvider = new SemanticsRelationEfProvider();
            return semanticsProvider.GetSemantics(id, "D").Select(AutoMapper.Mapper.Map<SemanticsModel>).ToList();
        }
        /// <summary>
        /// 添加别名
        /// </summary>
        /// <param name="semantics">语义关系模型</param>
        public void AddAlias(SemanticsModel semantics)
        {
            var termProvider = new CcTermEfPrrovied();
            var glossaryProvider = new GlossaryManageEFPrrovied();
            var cc = termProvider.GetCcTermById(semantics.FTermClassId).CCCode;

            //先查看添加的别名在数据库中是否存在
            var newTermClassId = glossaryProvider.GetIdByTerm(semantics.LTerm, cc);
            //若存在只需添加代的关系
            if (newTermClassId != null)
            {
                semantics.LTermClassId = Guid.Parse(newTermClassId);
            }
            //若不存在，需将新词添加到叙词表中 再添加代的关系
            else
            {
                semantics.LTermClassId = Guid.NewGuid();
                var newTerm = new CcTermModel()
                {
                    CCCode = cc,
                    Term = semantics.LTerm,
                    TermClassID = semantics.LTermClassId,
                    CreatedBy = semantics.CreatedBy,
                    CreatedDate = semantics.CreatedDate,
                    LastUpdatedBy = semantics.LastUpdatedBy,
                    OrderIndex = semantics.OrderIndex
                };
                //添加的别名 添加到分类叙词表中 别名的概念类与原词相同
                termProvider.AddEfPro(AutoMapper.Mapper.Map<SD_CCTerm>(newTerm), cc);
            }
            //添加‘D’的关系到语义关系表中
            var semanticsProvider = new SemanticsRelationEfProvider();
            semanticsProvider.SaveSemantics(AutoMapper.Mapper.Map<List<SD_Semantics>>(new List<SemanticsModel>() { semantics }));
        }
        /// <summary>
        /// 删除别名
        /// </summary>
        /// <param name="semantics">语义关系模型</param>
        public void DeleteAlias(SemanticsModel semantics)
        {
            var semanticsProvider = new SemanticsRelationEfProvider();
            //只需要删除该“D”的关系，不删除叙词分类表中信息
            semanticsProvider.DeleteSemantics(AutoMapper.Mapper.Map<List<SD_Semantics>>(new List<SemanticsModel>() { semantics }));
        }

        public List<string> GetidlistService(string termclassid)
        {
            var id = Guid.Parse(termclassid);
            return EfPrrovied.GetidListEf(id);
        }
        /// <summary>
        /// 获得所有的概念类
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllConceptClass()
        {
            var ccdictionary = new Dictionary<string, string>();
            var ccArr = EfPrrovied.GetConceptClass().Where(w => w.CC != null && w.CCCode != "BS")
                .Select(o => new { cc = o.CC.Remove(0, 5), ccode = o.CCCode });
            foreach (var cc in ccArr)
            {
                ccdictionary.Add(cc.ccode, cc.cc);
            }
            return ccdictionary;
        }
    }
}

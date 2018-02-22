using System.Data;
using Jurassic.Com.DB;
using Jurassic.Semantics.Entity.EntityModel;
using Jurassic.Semantics.EntityNew;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;


namespace Jurassic.Semantics.EFProvider
{
    public class GlossaryManageEFPrrovied
    {
        private readonly SemanticsDbContext semanticsDbContext;
        private List<USP_GetTermTree> list = null;
        private List<SMT_BOTTreeView> botlist = null;

        public GlossaryManageEFPrrovied()
        {
            semanticsDbContext = new SemanticsDbContext();
        }
        #region 1.获取概念类树结构的数据+GetCctermTreeEf()+新
        /// <summary>
        /// 获得概念类数据
        /// </summary>
        /// <param name="strCcode">概念类代码</param>
        /// <param name="strSr">概念类关系</param>
        /// <returns>树形的概念类数据</returns>   
        public List<USP_GetTermTree> GetCctermTreeEf(string strCcode, string strSr)
        {
            var strPro = "EXEC USP_GetTermTree @CCCode =" + '\'' + strCcode + '\'' + ",@SR =" + '\'' + strSr + '\'' +
                ",@Term=NULL,@TermClassId=Null";
            //设置操作完成的最大时间
            semanticsDbContext.Database.CommandTimeout = 120;
            list = semanticsDbContext.Database.SqlQuery<USP_GetTermTree>(strPro).OrderBy(o => o.Term).ThenBy(o => o.CreatedDate).ToList();
            return list;
        }
        #endregion

        #region 2.获的树节点的来源+GetSourcesListEf(string guidconCeptClassid)+新
        /// <summary>
        /// 获得概念类的来源
        /// </summary>
        /// <param name="guidconCeptClassid">叙词概念类id</param>
        /// <returns>list类型来源数据</returns>       
        public List<SD_TermSource> GetSourcesListEf(string cccode)
        {
            List<SD_TermSource> sdtermlist = null;//初始化来源集合
            sdtermlist = semanticsDbContext.SD_TermSource.Where(o => o.CCCode == cccode).Distinct().ToList();
            return sdtermlist;
        }
        #endregion

        #region 3.获得译文表和别名表的数据+GetTermTranslationsEf(Guid termGuid, string languageType)+新

        /// <summary>
        /// 获得译文表/别名表数据
        /// </summary>
        /// <param name="termGuid">叙词id</param>
        /// <returns>每一个叙词的译文和别名</returns>
        public List<SD_TermTranslation> GetTermTranslationsEf(Guid termGuid)
        {
            List<SD_TermTranslation> translationList = null;
            translationList = semanticsDbContext.SD_TermTranslation.Where(o => o.TermClassID == termGuid).ToList();
            return translationList;
        }
        #endregion

        #region 4.删除数据+新
        //删除关键词之前的判断
        public int IsKeywordExistEf(Guid termClassid)
        {
            var isKeyword =
               semanticsDbContext.SD_TermKeyword.Count(o => o.TermClassID == termClassid);
            return isKeyword;
        }
        //删除关键词表数据
        public void DeleteKeywordByIdEf(Guid termClassid)
        {
            var sdTermKeyword = semanticsDbContext.SD_TermKeyword.Where(o => o.TermClassID == termClassid);
            semanticsDbContext.SD_TermKeyword.RemoveRange(sdTermKeyword);
            semanticsDbContext.SaveChanges();
        }
        //删除译文表之前的判断
        public int IsTranslationExistEf(Guid termClassid)
        {
            var isTranslation =
               semanticsDbContext.SD_TermTranslation.Count(o => o.TermClassID == termClassid);
            return isTranslation;
        }
        //删除译文表数据
        public void DeleteTranslationByIdEf(Guid termClassid)
        {
            var sdTermTranslation =
                         semanticsDbContext.SD_TermTranslation.Where(o => o.TermClassID == termClassid);
            semanticsDbContext.SD_TermTranslation.RemoveRange(sdTermTranslation);
            semanticsDbContext.SaveChanges();
        }
        //删除语义关系表之前的判断
        public int IsSemanticsExistEf(Guid termClassid)
        {
            var isSemantics =
               semanticsDbContext.SD_Semantics.Count(o => o.FTermClassId == termClassid || o.LTermClassId == termClassid);
            return isSemantics;
        }
        //删除语义关系表数据
        public void DeleteSemanticsByIdEf(Guid termClassid)
        {
            var sdSemantics = semanticsDbContext.SD_Semantics.Where(o => o.FTermClassId == termClassid || o.LTermClassId == termClassid);
            semanticsDbContext.SD_Semantics.RemoveRange(sdSemantics);
            semanticsDbContext.SaveChanges();
        }
        //删除叙词表之前的判断
        public int IsCcTermExistEf(Guid termClassid)
        {
            var isCcTerm = semanticsDbContext.SD_CCTerm.Count(o => o.TermClassID == termClassid);
            return isCcTerm;
        }
        //删除叙词表数据
        public void DeleteCcTermByIdEf(Guid termClassid)
        {
            var sdCcTerm = semanticsDbContext.SD_CCTerm.Where(o => o.TermClassID == termClassid);
            semanticsDbContext.SD_CCTerm.RemoveRange(sdCcTerm);
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 5.删除数据来源+DeleteSourceRow(string sourceName)+新
        /// <summary>
        /// 根据来源名删除对应的叙词来源
        /// </summary>
        /// <param name="sourceName">来源名称</param>
        public void DeleteSourceRow(string sourceName)
        {
            var termsourceModel = semanticsDbContext.SD_TermSource.FirstOrDefault(o => o.Source == sourceName);
            if (termsourceModel != null)
            {
                semanticsDbContext.SD_TermSource.Remove(termsourceModel);
                semanticsDbContext.SaveChanges();
            }
        }
        #endregion

        #region 6.编辑节点+EditTreeNameById(Guid guid, string termName,string path)+新
        /// <summary>
        /// 修改节点名称
        /// </summary>
        /// <param name="termGuid">叙词id</param>
        /// <param name="cctermName">修改后的名称</param>
        /// <param name="path">节点路径</param>
        public void EditCcTreeNameByIdEf(Guid termGuid, string cctermName, string path)
        {
            SD_CCTerm sdCcTerm = semanticsDbContext.SD_CCTerm.FirstOrDefault(o => o.TermClassID == termGuid);//在叙词表中找到实体
            if (sdCcTerm == null) throw new ArgumentNullException("不存在该值！");
            sdCcTerm.Term = cctermName;
            List<SD_Semantics> fSematics = semanticsDbContext.SD_Semantics.Where(o => o.FTermClassId == termGuid).ToList();//在语义表中找到实体
            List<SD_Semantics> lSematics = semanticsDbContext.SD_Semantics.Where(o => o.LTermClassId == termGuid).ToList();
            if (fSematics.Count >= 0 && lSematics.Count == 0)//找到的根节点含有子节点
            {
                sdCcTerm.PathTerm = cctermName;
                fSematics.ForEach(o => o.FTerm = cctermName);//修改根节点下子节点的首词
            }
            if (lSematics.Count > 0)//找到的节点不是根节点
            {
                sdCcTerm.PathTerm = path + "/" + cctermName;
                lSematics.ForEach(o => o.LTerm = cctermName);
                if (fSematics.Count > 0)//找到的节点是含有子节点的节点
                {
                    fSematics.ForEach(o => o.FTerm = cctermName);//修改含有子节点的节点的名称
                }
            }
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 7.保存描述+SaveMsEf(string strDescription, Guid termClassid)+新
        /// <summary>
        /// 修改对叙词的描述
        /// </summary>
        /// <param name="strDescription">对节点的描述</param>
        /// <param name="termClassid">叙词id</param>
        public void SvaeDescriptionEf(string strDescription, Guid termClassid)
        {
            //判断是否存在对应的叙词
            //sdTerm != null表示存在
            SD_CCTerm sdTerm = semanticsDbContext.SD_CCTerm.FirstOrDefault(o => o.TermClassID == termClassid);
            if (sdTerm != null)
            {
                sdTerm.Description = strDescription;//修改描述
                semanticsDbContext.SaveChanges();
            }
        }
        #endregion

        #region 8.添加来源+AddTsEf(SD_TermSource termSourceModel)+新

        /// <summary>
        /// 添加叙词的来源到来源表中
        /// </summary>
        /// <param name="termSourceModel">来源的数据实体</param>
        /// <param name="cccode"></param>      
        public void AddSourceEf(SD_TermSource termSourceModel, string cccode)
        {
            termSourceModel.CCCode = cccode;
            semanticsDbContext.SD_TermSource.Add(termSourceModel);
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 9.保存数据来源+SaveSourceEf(string source, Guid termGuid)+新
        /// <summary>
        /// 保存叙词来源到叙词表中
        /// </summary>
        /// <param name="source">来源名称</param>
        /// <param name="termGuid">叙词id</param>
        public void SaveSourceEf(string source, Guid termGuid)
        {
            SD_CCTerm newSdCcTerm = semanticsDbContext.SD_CCTerm.FirstOrDefault(o => o.TermClassID == termGuid);
            newSdCcTerm.Source = source;//给叙词新的来源
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 10.添加译文表+SaveotherName(SD_TermTranslation sdTermTranslation)+新
        /// <summary>
        /// 添加叙词对应的译文
        /// </summary>
        /// <param name="sdTermTranslation">译文数据实体</param>
        public void SaveotherNameEf(SD_TermTranslation sdTermTranslation)
        {
            var count = semanticsDbContext.SD_TermTranslation.ToList().Count;//已经存在多少条译文数据
            sdTermTranslation.OrderIndex = count + 1;
            semanticsDbContext.SD_TermTranslation.Add(sdTermTranslation);
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 11.删除译文表数据+DeleteTranslationEf(Guid tuid)+新
        /// <summary>
        /// 删除叙词对应的译文数据
        /// </summary>
        /// <param name="termclassid">叙词id</param>
        /// <param name="strtran">叙词对应的译文</param>
        public void DeleteTranslationEf(Guid termclassid, string strtran)
        {
            var sdTerm = semanticsDbContext.SD_TermTranslation.FirstOrDefault(o => o.TermClassID == termclassid && o.Translation == strtran);
            semanticsDbContext.SD_TermTranslation.Remove(sdTerm);
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 12.拖拽节点+DragNodeEf(SD_Semantics sdModel, SD_CCTerm Dragmodel, string strSr)+新
        /// <summary>
        /// 拖拽节点
        /// 1、删除和以前和父节点的关系
        /// 2、建立和新的父节点的关系
        /// 3、更新叙词的关系
        /// </summary>
        /// <param name="sdModel">语义关系实体</param>
        /// <param name="dragmodel">拖拽节点实体</param>
        /// <param name="strSr">概念类名称</param>
        public void DragNodeEf(SD_Semantics sdModel, SD_CCTerm dragmodel, string strSr)
        {
            if (string.IsNullOrEmpty(strSr)) return;
            //判断拖拽的节点是不是根节点
            //countSemantics == 0,是根节点
            var countSemantics = semanticsDbContext.SD_Semantics.Count(o => o.LTermClassId == dragmodel.TermClassID && o.SR == strSr);
            //当拖拽的是根节点是
            if (countSemantics == 0 && sdModel.FTerm != null)
            {
                sdModel.SR = strSr;  //修改关系
                semanticsDbContext.SD_Semantics.Add(sdModel);
                semanticsDbContext.SaveChanges();
            }
            //拖拽的不是根节点
            else
            {
                var oldsdmodel = semanticsDbContext.SD_Semantics.FirstOrDefault(o => o.LTermClassId == dragmodel.TermClassID && o.SR == strSr);
                //投放的位置是根节点
                if (sdModel.FTerm == null)
                {
                    semanticsDbContext.SD_Semantics.Remove(oldsdmodel);
                    semanticsDbContext.SaveChanges();
                }
                //投放的位置不是根节点
                else
                {
                    semanticsDbContext.SD_Semantics.Remove(oldsdmodel);
                    sdModel.SR = strSr;
                    semanticsDbContext.SaveChanges();
                }
            }
        }
        #endregion

        public void UpdatePathTerm(Guid termClassId)
        {
            var helper = new DBHelper("SemanticsDbContext");
            helper.ExecNonQuery("update [dbo].[SD_CCTerm] set PathTerm = dbo.GetPath(TermClassID) where TermClassID=" + "'" + termClassId + "'");
        }

        #region 13.排序+SequenceNodeEf(Guid nodeId, string parentName, int newOrderIndex, string parentPath)+新
        /// <summary>
        /// 节点排序
        /// 1、给根节点排序:获得所有的根节点，进行排序
        /// 2、给根节点下的子节点排序：获得节点下的子节点，进行排序
        /// </summary>
        /// <param name="nodeId">叙词id</param>
        /// <param name="parentName">父节点的名称</param>
        /// <param name="newOrderIndex">节点新的序号</param>
        /// <param name="parentPath">节点新的路径</param>
        public void SequenceNodeEf(Guid nodeId, string parentName, int newOrderIndex)
        {
            var sdSematics = semanticsDbContext.SD_Semantics.FirstOrDefault(o => o.LTermClassId == nodeId);
            var sdCcterm = semanticsDbContext.SD_CCTerm.FirstOrDefault(c => c.TermClassID == nodeId);
            //找到的节点是根节点
            if (sdSematics == null)
            {
                if (sdCcterm != null)
                {
                    sdCcterm.OrderIndex = newOrderIndex;
                }
                semanticsDbContext.SaveChanges();
            }
            else
            {
                if (sdCcterm != null && parentName != null)
                {
                    sdSematics.FTerm = parentName;
                    sdSematics.LTerm = sdCcterm.Term;
                    sdSematics.OrderIndex = newOrderIndex;
                    semanticsDbContext.SD_Semantics.AddOrUpdate(sdSematics);

                    sdCcterm.OrderIndex = newOrderIndex;
                    semanticsDbContext.SD_CCTerm.AddOrUpdate(sdCcterm);
                }
            }
            semanticsDbContext.SaveChanges();
        }
        #endregion

        #region 14.获得来源数组+List<string> GetsourceListEf(Guid cctermGuid)+新
        /// <summary>
        /// 1、将叙词的来源用','进行分割
        /// 2、获得分割数据，组成数组
        /// </summary>
        /// <param name="cctermGuid">叙词id</param>
        /// <returns>分割后的来源数组</returns>
        public List<string> GetsourceListEf(Guid cctermGuid)
        {
            if (semanticsDbContext.SD_CCTerm.Any(o => o.TermClassID == cctermGuid))
            {
                var sdterm =
                    semanticsDbContext.SD_CCTerm.Where(o => o.TermClassID == cctermGuid).Select(o => o.Source).First();
                string[] sdterms;
                if (string.IsNullOrEmpty(sdterm))
                { return new List<string>(); }
                else
                { sdterms = sdterm.Split(new[] { ',', '、' }); }
                var sourceList =
                    semanticsDbContext.SD_TermSource.Where(o => sdterms.Contains(o.Source))
                        .Select(o => o.Source)
                        .ToList();

                return sourceList;
            }
            else return new List<string>();
        }
        #endregion

        #region 16.添加子节点+AddChildNode(Guid termGuid, SD_CCTerm newSdCcTerm)+新
        /// <summary>
        /// 添加节点(添加所给GUID对应节点的存在正向语义关系的节点)
        /// </summary>
        /// <param name="termGuid">叙词id</param>
        /// <param name="newSdCcTerm">叙词数据模型(实体)</param>
        /// <param name="strCcTerm">概念类名称</param>
        public void AddNodeEf(Guid termGuid, SD_CCTerm newSdCcTerm, string strCcTerm)
        {
            var oldSdCcTerm = semanticsDbContext.SD_CCTerm.FirstOrDefault(o => o.TermClassID == termGuid);//根据叙词id找到对应的实体数据
            var terms = GetCcTermArrayEf(strCcTerm);
            newSdCcTerm.CCCode = terms[0];
            semanticsDbContext.SD_CCTerm.Add(newSdCcTerm);
            var count = semanticsDbContext.SaveChanges();
            if (oldSdCcTerm != null && count > 0 && !String.IsNullOrEmpty(terms[1]))
            {
                var newSemantics = new SD_Semantics()
                {
                    SR = terms[1], //概念类之间的关系
                    OrderIndex = newSdCcTerm.OrderIndex, //排序号
                    CreatedDate = newSdCcTerm.CreatedDate, //创建日期
                    CreatedBy = newSdCcTerm.CreatedBy, //创建人
                    FTermClassId = oldSdCcTerm.TermClassID,
                    FTerm = oldSdCcTerm.Term,
                    LTerm = newSdCcTerm.Term,
                    LTermClassId = newSdCcTerm.TermClassID,
                    LastUpdatedBy = newSdCcTerm.LastUpdatedBy, //最后更新人
                    LastUpdatedDate = newSdCcTerm.LastUpdatedDate, //最后更新时间
                    Remark = newSdCcTerm.Remark,
                };
                semanticsDbContext.SD_Semantics.Add(newSemantics);
                semanticsDbContext.SaveChanges();
            }
        }
        #endregion
        #region 17.公用的方法
        /// <summary>
        /// 判断查找的数据实体是否存在
        /// </summary>
        /// <param name="strEntity">实体名称</param>
        /// <param name="strParameter">条件参数1</param>
        /// <param name="termClassid">guid是</param>
        /// <returns></returns>
        public int IsDataExistEf(string strParameter, Guid termClassid)
        {
            var isTranslation =
               semanticsDbContext.SD_TermTranslation.Count(o => o.TermClassID == termClassid && o.LangCode == strParameter);
            return isTranslation;
        }
        /// <summary>
        /// 得到叙词信息的数组
        /// </summary>
        /// <param name="strCcTerm">概念类名</param>
        /// <returns>概念类的信息</returns>
        public string[] GetCcTermArrayEf(string strCcTerm)
        {
            var conceptclass = strCcTerm == "" ? "业务过程" : strCcTerm;
            var ccTermInfor = new string[3];
            ccTermInfor[0] = GetConceptClass().Where(w => w.CC != null).Single(o => o.CC.Remove(0, 5) == conceptclass).CCCode;
            //if (conceptclass == "成果类型")
            //{
            //    ccTermInfor[1] = "R_BP_CS_PT";
            //}
            //else
            //{
                if (GetAllSemanticsType().Any(o => o.CCCode1 == ccTermInfor[0] && o.CCCode2 == ccTermInfor[0]))
                {
                    ccTermInfor[1] = GetAllSemanticsType()
                       .Single(o => o.CCCode1 == ccTermInfor[0] && o.CCCode2 == ccTermInfor[0] && o.SR.Contains("XF")).SR;
                }
                else
                {
                    ccTermInfor[1] = "";
                }
            //}
            return ccTermInfor;
        }
        #endregion
        #region 18.获取id集合
        public List<string> GetidListEf(Guid termClassid)
        {
            var idlist = semanticsDbContext.SD_Semantics.Where(o => o.FTermClassId == termClassid && o.SR == "R_BP_CS_PT").Select(o => o.LTermClassId.ToString()).ToList();
            return idlist;
        }

        #endregion

        /// <summary>
        /// 获取概念类的数据
        /// 1、概念类的guid
        /// 2、概念类的代码cccode
        /// </summary>-
        /// <returns></returns>
        public List<SD_ConceptClass> GetConceptClass()
        {
            return semanticsDbContext.SD_ConceptClass.ToList();
        }
        /// <summary>
        /// 获得语义类型的关系
        /// </summary>
        /// <returns>字符串的数组</returns>
        public List<SD_SemanticsType> GetAllSemanticsType()
        {
            return semanticsDbContext.SD_SemanticsType.ToList();
        }

        public List<SD_CCTerm> GetTermList(string cc)
        {
            return semanticsDbContext.SD_CCTerm.Where(w => w.CCCode.Equals(cc)).ToList();
        }

        public string GetIdByTerm(string term, string cc)
        {
            try
            {
                return
              semanticsDbContext.SD_CCTerm.Where(w => w.Term.Equals(term) && w.CCCode.Equals(cc))
                  .Select(s => s.TermClassID)
                  .FirstOrDefault()
                  .ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

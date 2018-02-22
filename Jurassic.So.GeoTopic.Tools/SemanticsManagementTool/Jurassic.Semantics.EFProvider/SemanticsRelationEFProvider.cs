using Jurassic.Semantics.Entity.EntityModel;
using Jurassic.Semantics.EntityNew;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// 语义关系维护
    /// </summary>
    public class SemanticsRelationEfProvider
    {
        private SemanticsDbContext Context { get; set; }

        public SemanticsRelationEfProvider()
        {
            Context = new SemanticsDbContext();
        }
        /// <summary>
        /// 获得语义关系 正向
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="sr">语义关系</param>
        /// <returns></returns>
        public List<SD_CCTerm> GetSemantics(string term, string sr)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"term or " + "sr");

            //先获得语义正向关联的ID
            var lTermId = Context.SD_Semantics.Where(w => w.FTermClassId.ToString() == term && w.SR.Equals((sr)))
                    .Select((s => s.LTermClassId))
                    .ToList();

            return lTermId.Count != 0
                  ? Context.SD_CCTerm.Where(w => lTermId.Contains(w.TermClassID)).Include("SD_ConceptClass").ToList()
                  : new List<SD_CCTerm>();
        }

        /// <summary>
        /// 获得语义关系 正向
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sr">语义关系</param>
        /// <returns></returns>
        public List<SD_Semantics> GetSemantics(Guid id, string sr)
        {
            if (string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"sr");
            return Context.SD_Semantics.Where(w => w.FTermClassId == id && w.SR.Equals(sr.Trim())).ToList();
        }
        /// <summary>
        /// 获得语义关系 反向
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sr">语义关系</param>
        /// <returns></returns>
        public List<SD_Semantics> GetReverseSemantics(Guid id, string sr)
        {
            if (string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"sr");
            return Context.SD_Semantics.Where(w => w.LTermClassId.Equals(id) && w.SR.Equals((sr.Trim()))).ToList();
        }
        /// <summary>
        /// 获得语义关系 反向
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="sr">语义关系</param>
        /// <returns></returns>
        public List<SD_CCTerm> GetReverseSemantics(string term, string sr)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(sr))
                throw new ArgumentNullException(@"term or " + "sr");

            //先获得语义反向关联的ID
            var fTermId = Context.SD_Semantics.Where(w => w.LTerm.Equals(term) && w.SR.Equals((sr)))
                    .Select((s => s.FTermClassId))
                    .ToList();

            return fTermId.Count != 0
                  ? Context.SD_CCTerm.Where(w => fTermId.Contains(w.TermClassID)).Include("SD_ConceptClass").ToList()
                  : new List<SD_CCTerm>();
        }

        /// <summary>
        /// 根据概念类名称 加载对应的树或是列表（pid全部为空）
        /// </summary>
        /// <param name="cc">概念类的名称</param>
        /// <returns>返回指定概念类的全部叙词并以树结构展示</returns>
        public List<USP_GetTermTree> GetTermTrees(string cc)
        {
            var sr = "R_" + cc + "_XF_" + cc;
            var strPro = "EXEC USP_GetTermTree @CCCode =" + '\'' + cc + '\'' + ",@SR =" + '\'' + sr + '\'' +
                ",@Term=NULL,@TermClassId=Null";
            var list = Context.Database.SqlQuery<USP_GetTermTree>(strPro).OrderBy(o => o.OrderIndex).ThenBy(o => o.CreatedDate).ToList();
            return list;
        }

        /// <summary>
        /// 获得需要维护的概念类                            
        /// </summary>
        /// <returns>返回字典key:概念类code,概念类中文名称</returns>
        public Dictionary<string, string> GetNeedManageCC()
        {
            var results = new Dictionary<string, string>();
            var cccodes = Context.SD_SemanticsType.Where(w => !w.SR.Contains("XF"))
                  .Select(s => s.CCCode1).Distinct().ToList();
            foreach (var cccode in cccodes)
            {
                if (cccode != null && cccode != "BS")
                {
                    results.Add(cccode, Context.SD_ConceptClass.Where(w => w.CCCode.Equals(cccode)).Select(s => s.CC).FirstOrDefault());
                }
            }
            return results;
        }

        /// <summary>
        /// 获得指定概念类需要维护的关系类型列表
        /// </summary>
        /// <param name="cc">概念类</param>
        /// <returns>返回：key:SR,value:对应的概念类（通过概念类来加载对应的树结构）</returns>
        public List<SD_SemanticsType> GetRelationOfCC(string cc)
        {
            cc = cc.Trim(new[] { '\'' });

            var lists = Context.SD_SemanticsType.Where(w => !w.SR.Contains("XF"))// && !w.SR.Contains("PT"))
                  .Where(w => w.CCCode1.Equals(cc)).Distinct().ToList();

            return lists;
        }

        /// <summary>
        /// 添加数据到语义关系表
        /// </summary>
        /// <param name="semantics">语义关系列表</param>
        public void SaveSemantics(List<SD_Semantics> semantics)
        {
            foreach (var semantic in semantics)
            {
                //如果要添加的数据不存在 添加
                if (!Context.SD_Semantics.Any(a => a.FTermClassId.Equals(semantic.FTermClassId) && a.SR.Equals(semantic.SR) && a.LTermClassId.Equals(semantic.LTermClassId)))
                {
                    Context.SD_Semantics.Add(semantic);
                }
                //如果数据存在，判断是否更新orderindex。变更则更新，没变更则不操作
                else
                {
                    var oldSemantics = Context.SD_Semantics.SingleOrDefault(a => a.FTermClassId.Equals(semantic.FTermClassId) && a.SR.Equals(semantic.SR) && a.LTermClassId.Equals(semantic.LTermClassId));
                    //对原来的数据进行重新赋值
                    if (oldSemantics != null && oldSemantics.OrderIndex != semantic.OrderIndex)
                    {
                        oldSemantics.OrderIndex = semantic.OrderIndex;
                        oldSemantics.LastUpdatedBy = semantic.LastUpdatedBy;
                        oldSemantics.LastUpdatedDate = DateTime.Now;
                    }
                }
            }
            Context.SaveChanges();
        }
        public void DeleteSemantics(List<SD_Semantics> semantics)
        {
            foreach (var semantic in semantics)
            {
                if (Context.SD_Semantics.Any(a =>
                            a.FTermClassId.Equals(semantic.FTermClassId) && a.SR.Equals(semantic.SR) &&
                            a.LTerm.Equals(semantic.LTerm)))
                {
                    var oldSemantics = Context.SD_Semantics.SingleOrDefault(a =>
                                a.FTermClassId.Equals(semantic.FTermClassId) && a.SR.Equals(semantic.SR) &&
                                a.LTerm.Equals(semantic.LTerm));
                    Context.SD_Semantics.Remove(oldSemantics);
                }
            }
            Context.SaveChanges();
        }
        public void DeleteSemantics(string ptid,string sr, string term)
        {
            var id = Guid.Parse(ptid);
            if (Context.SD_Semantics.Any(a =>(a.FTermClassId==id ||a.LTermClassId==id)&&
                       (a.FTerm.Equals(term) || a.LTerm.Equals(term)) && a.SR.Equals(sr)))
            {
                var oldSemantics = Context.SD_Semantics.Where(a => (a.FTermClassId == id || a.LTermClassId == id) &&
                               (a.FTerm.Equals(term) || a.LTerm.Equals(term)) && a.SR.Equals(sr));
                Context.SD_Semantics.RemoveRange(oldSemantics);
            }
            Context.SaveChanges();
        }
    }
}

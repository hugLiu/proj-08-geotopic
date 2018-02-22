using System.Linq;
using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.Sooil.IServiceBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jurassic.Semantics.Service
{
    public class PtContextManageService : IPtContextManageService
    {
        private readonly PtContextEfProvider _efProvider;
        private readonly SemanticsRelationEfProvider _sProvider;

        public PtContextManageService(PtContextEfProvider efProvider, SemanticsRelationEfProvider sProvider)
        {
            _efProvider = efProvider;
            _sProvider = sProvider;
        }

        /// <summary>
        /// 获得与pt存在关系的其他概念类和语义关系类型
        /// </summary>
        /// <returns></returns>
        public List<PtRelations> GetPtRelations()
        {
            var result = new List<PtRelations>();
            var relations = _efProvider.GetPtRelations();
            foreach (var r in relations)
            {
                var field = r.CCCode1 != "PT" ? r.CCCode1 : r.CCCode2;
                if (r.SR.Contains("SY")) field = "U" + field;
                result.Add(new PtRelations() { SR = r.SR, Field = field });
            }
            return result;
        }
        /// <summary>
        /// 获得某一概念类的分组统计（在pt上下文中）
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public IEnumerable GetTermGroup(string pt, string field)
        {
            if (field == null) throw new ArgumentNullException("field");
            return _efProvider.GetTermGroup(pt, field);
        }

        /// <summary>
        /// 获得pt上下文
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="filterItems"></param>
        /// <param name="field"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SMT_PTContextView> FilterPtContext(string pt, string filterItems, string field, int pageIndex, int pageSize)
        {
            if (string.IsNullOrEmpty(filterItems) || string.IsNullOrEmpty(field))
            {
                return _efProvider.GetPtContext(pt, pageIndex, pageSize);
            }
            var whereCondition = GetWhereExpression(filterItems, field);
            return _efProvider.FilterPtContext(pt, whereCondition, pageIndex, pageSize);
        }

        /// <summary>
        /// 获得pt上下文，可带过滤条件 的个数
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="filterItems"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public int GetFilterPtContextCount(string pt, string filterItems, string field)
        {
            if (string.IsNullOrEmpty(filterItems) || string.IsNullOrEmpty(field))
            {
                return _efProvider.GetPtContextCount(pt);
            }
            var whereCondition = GetWhereExpression(filterItems, field);
            return _efProvider.GetFilterPtContextCount(pt, whereCondition);
        }

        /// <summary>
        /// 保存修改的pt上下文关系
        /// </summary>
        /// <param name="ptId"></param>
        /// <param name="pt"></param>
        /// <param name="sr"></param>
        /// <param name="ccId"></param>
        /// <param name="ccTerm"></param>
        /// <param name="userName"></param>
        public void SavePtContext(string ptId, string pt, string sr, string ccId, string ccTerm, string userName)
        {
            var provider = new SemanticsRelationEfProvider();
            var semantics = new List<SD_Semantics>();
            sr = sr.Trim();
            var index = sr.IndexOf("PT", StringComparison.Ordinal);
            if (!string.IsNullOrEmpty(ccId) && !string.IsNullOrEmpty(ccTerm))
            {
                var ccids = ccId.Split('|');
                var ccTerms = ccTerm.Split(Convert.ToChar('|'));
                for (int i = 0; i < ccids.Length; i++)
                {
                    var fid = ptId;
                    var lid = ccids[i];
                    var fterm = pt;
                    var lterm = ccTerms[i];
                    if (index > 3) { fid = ccids[i]; lid = ptId; fterm = ccTerms[i]; lterm = pt; }
                    var semantic = new SD_Semantics()
                    {
                        FTermClassId = Guid.Parse(fid),
                        LTermClassId = Guid.Parse(lid),
                        FTerm = fterm,
                        LTerm = lterm,
                        SR = sr,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = userName,
                        LastUpdatedDate = DateTime.Now,
                        OrderIndex = i
                    };
                    semantics.Add(semantic);
                }
            }
            provider.SaveSemantics(semantics);
        }

        public List<TreeModel> GetTermTree(string cc)
        {
            if (cc.Equals("BS"))
            {
                var gProvider = new GlossaryManageEFPrrovied();
                return gProvider.GetTermList(cc).Select(s => new TreeModel(s.TermClassID.ToString(), s.Term, null)).ToList();
            }
            var provider = new SemanticsRelationEfProvider();
            var data = provider.GetTermTrees(cc).Select(AutoMapper.Mapper.Map<TermTreeModel>).ToList();
            return data.Select(d => new TreeModel(d.TermClassId.ToString(), d.Term, d.PId.ToString())).ToList();
        }

        /// <summary>
        /// 使用表达式树获得动态拼接where条件
        /// </summary>
        /// <param name="filterItems">过滤值</param>
        /// <param name="field">过滤字段</param>
        /// <returns>返回表达式</returns>
        private Expression<Func<SMT_PTContextView, bool>> GetWhereExpression(string filterItems, string field)
        {
            var items = filterItems.Replace("空白", string.Empty).Split(',');
            var filter = new List<Filter>();
            foreach (var item in items)
            {
                filter.Add(new Filter() { Operation = Op.Equals, PropertyName = field, Value = item });
            }
            var filters = new FilterCollection { filter };
            return LambdaExpressionBuilder.GetExpression<SMT_PTContextView>(filters);
        }

        public void DeletePtSemantics(string ptid,string sr, string term)
        {
            _sProvider.DeleteSemantics(ptid,sr, term);
        }

        public object GetPtRelationsOfCc(string ptid, string sr)
        {
            try
            {
                var index = sr.IndexOf("PT", StringComparison.Ordinal);
                if (index > 3) //反向
                {
                    return _sProvider.GetReverseSemantics(Guid.Parse(ptid), sr).Select(s => new { id = s.FTermClassId, text = s.FTerm }).ToList();
                }
                //正向
                return _sProvider.GetSemantics(Guid.Parse(ptid), sr).Select(s => new { id = s.LTermClassId, text = s.LTerm }).ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}

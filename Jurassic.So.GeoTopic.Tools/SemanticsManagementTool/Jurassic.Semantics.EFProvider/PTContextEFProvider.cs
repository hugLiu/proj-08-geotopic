using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jurassic.Semantics.EntityNew;
using System.Linq.Dynamic;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// 成果类型上下文的维护
    /// </summary>
    public class PtContextEfProvider
    {
        private SemanticsDbContext Context { get; set; }
        public PtContextEfProvider()
        {
            Context = new SemanticsDbContext();
        }

        /// <summary>
        /// 获得成果类型上下文
        /// </summary>
        /// <param name="pt">查询的成果类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">一页总条数</param>
        /// <returns></returns>
        public List<SMT_PTContextView> GetPtContext(string pt, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;
            var take = pageSize;
            return Context.SMT_PTContextView.Where(w => w.PT.Contains(pt) || pt == "")
                .OrderBy(o => o.PT)
                .Skip(skip)
                .Take(take)
                .ToList();
        }
        /// <summary>
        /// 获得与PT有关系的 语义关系类型以及对应的概念类
        /// </summary>
        /// <returns></returns>
        public List<SD_SemanticsType> GetPtRelations()
        {
            return Context.SD_SemanticsType.Where(w => w.SR.Contains("PT")).ToList();
        }

        /// <summary>
        /// 获得总条数
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public int GetPtContextCount(string pt)
        {
            return Context.SMT_PTContextView.Count(w => w.PT.Contains(pt) || pt == "");
        }

        /// <summary>
        /// 获得叙词的聚合
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public IEnumerable GetTermGroup(string pt, string field)
        {
            var returnField = string.Format("new ({0})", field);
            return Context.SMT_PTContextView.Where(w => w.PT.Contains(pt)).Select(returnField).Distinct();
        }

        /// <summary>
        /// 获得过滤后的PT上下文
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="conditions"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SMT_PTContextView> FilterPtContext(string pt, Expression<Func<SMT_PTContextView, bool>> conditions, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;
            var take = pageSize;
            return Context.SMT_PTContextView.Where(w => w.PT.Contains(pt)).Where(conditions)
                .OrderBy(o => o.PT)
                .Skip(skip)
                .Take(take)
                .ToList(); ;
        }

        /// <summary>
        /// 获得总条数
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public int GetFilterPtContextCount(string pt, Expression<Func<SMT_PTContextView, bool>> conditions)
        {
            return Context.SMT_PTContextView.Where(w => w.PT.Contains(pt)).Where(conditions).Count();
        }
    }
}

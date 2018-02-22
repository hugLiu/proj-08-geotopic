using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Jurassic.Com.DB;
using Jurassic.Com.Tools;
using Jurassic.Semantics.Entity.EntityModel;
using Jurassic.Semantics.EntityNew;

namespace Jurassic.Semantics.EFProvider
{
    public class KeyWordProvider
    {
        private readonly SemanticsDbContext _dbContext;
        public KeyWordProvider()
        {
            _dbContext = new SemanticsDbContext();
        }
        /// <summary>
        /// 读取BP和PT的组合树
        /// </summary>
        /// <param name="id">节点名称 null 则加载全部</param>
        /// <param name="showNoKeyWordPt">是否只显示没有添加关键的成果类型</param>
        /// <returns></returns>
        public List<USP_BPAndPTTree> GetBpAndPtTree(string id, bool showNoKeyWordPt)
        {
            var results = new List<USP_BPAndPTTree>();

            IDataParameter idParameter = new SqlParameter("@Id", SqlDbType.UniqueIdentifier);
            if (string.IsNullOrEmpty(id) || id == "null")
            {
                idParameter.Value = DBNull.Value;
            }
            else
            {
                idParameter.Value = Guid.Parse(id);
            }

            IDataParameter flagParameter = new SqlParameter("@IsNoKeywordPT", SqlDbType.Bit);
            flagParameter.Value = showNoKeyWordPt;

            var helper = new DBHelper("SemanticsDbContext");
            DataSet ds = helper.RunProcedureDs("[dbo].[USP_GetBPAndPTTree]", idParameter, flagParameter);

            if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var node = DataHelper.ReaderToModel<USP_BPAndPTTree>(row);
                    results.Add(node);
                }
            }
            return results.OrderBy(o=>o.Term).ToList();
        }

        /// <summary>
        /// 将最终编辑完成的关键字插入到数据库中
        /// </summary>
        /// <param name="id">当前选中的ID</param>
        /// <param name="userName">当前登录用户名</param>
        /// <param name="keywordsAndOrder">关键字及排序组合</param>
        public void InsertKeyWord(string id, string userName, Dictionary<string, int> keywordsAndOrder)
        {
            var guid = Guid.Parse(id);

            // 用新的关键词覆盖替换  注意OrderIndex
            foreach (var k in keywordsAndOrder)
            {
                //判断数据中如果已存在该数据，就跳过
                if (_dbContext.SD_TermKeyword.Any(w => w.TermClassID.Equals(guid) && w.Keyword.Equals(k.Key))) continue;
                //添加到数据库中
                var termKeyWord = new SD_TermKeyword
                {
                    TermClassID = guid,
                    Keyword = k.Key,
                    OrderIndex = k.Value,
                    CreatedBy = userName,
                    CreatedDate = DateTime.Now
                };
                _dbContext.SD_TermKeyword.Add(termKeyWord);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// 将最终编辑完成的关键字插入到数据库中
        /// </summary>
        /// <param name="id">当前选中的ID</param>
        /// <param name="words">选中的关键字的集合</param>
        public void DeleteKeyWord(string id, List<string> words)
        {
            var guid = Guid.Parse(id);

            var sdTermKeyword = _dbContext.SD_TermKeyword.Where(o => o.TermClassID == guid && words.Contains(o.Keyword));
            _dbContext.SD_TermKeyword.RemoveRange(sdTermKeyword);
            _dbContext.SaveChanges();

            _dbContext.SaveChanges();
        }

        /// <summary>
        ///更新关键字（主要是由于关键的orderindex发生改变 添加更新人及更新日期）
        /// </summary>
        /// <param name="id">当前选中的ID</param>
        /// <param name="userName">当前登录用户名</param>
        /// <param name="keywordsAndOrder">关键字及排序组合</param>
        public void UpdateKeyWord(string id, string userName, Dictionary<string, int> keywordsAndOrder)
        {
            var guid = Guid.Parse(id);

            // 用新的关键词覆盖替换  注意OrderIndex
            foreach (var k in keywordsAndOrder)
            {
                //判断数据中如果已存在该数据，就更新
                if (_dbContext.SD_TermKeyword.Any(w => w.TermClassID.Equals(guid) && w.Keyword.Equals(k.Key)))
                {
                    //获得数据库中已经存在的数据
                    var oldKeyWords = _dbContext.SD_TermKeyword.SingleOrDefault(w => w.TermClassID.Equals(guid) && w.Keyword.Equals(k.Key));
                    //对原来的数据进行重新赋值
                    if (oldKeyWords != null)
                    {
                        oldKeyWords.TermClassID = guid;
                        oldKeyWords.Keyword = k.Key;
                        oldKeyWords.OrderIndex = k.Value;
                        oldKeyWords.LastUpdatedBy = userName;
                        oldKeyWords.LastUpdatedDate = DateTime.Now;
                    }
                    _dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// get keywords by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>keywords</returns>
        public List<SD_TermKeyword> GetKeyWords(string id)
        {
            var _id = Guid.Parse(id);
            return _dbContext.SD_TermKeyword.Where(w => w.TermClassID.Equals(_id)).ToList();
        }
    }
}
               
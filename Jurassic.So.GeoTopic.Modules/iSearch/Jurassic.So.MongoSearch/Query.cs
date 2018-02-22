using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure.Model;
using Jurassic.So.Infrastructure.Operation;
using Jurassic.So.ISearch;
using Jurassic.So.ISearch.IOModel;
using Jurassic.So.ISearch.IOModel.InputParams;
using Jurassic.So.BusinessObjects;

namespace Jurassic.So.Search
{
    public class Query : Manager, IQuery
    {
        public QueryProvider ServiceProvider { get; set; }

        /// <summary>
        /// 信息搜索
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<QueryResult> Match(QueryCondition requestParams)
        {
            QueryResult response = new QueryResult();
            response.Count = 1000;
            response.MetaDatas = new MetaDataCollection(){ new MetaData() { { "iiid", "111" }, { "s:url", "adp:/vvv/vvv" }, { "ep.o.well", "井1" } }, new MetaData() { { "iiid", "222" }, { "s:url", "adp:/eee/ee" } } };
            response.Groups = new GroupCollection() { { "contributor", new Dictionary<string, int>() { { "jacky", 10 } } }, { "organazition", new Dictionary<string, int>() { { "中石油", 444 } } } };
            return await Task.FromResult(response);
        }

        /// <summary>
        /// 根据给定的标签，返回所有包含该标签的标签
        /// </summary>
        /// <param name="tagnames"></param>
        /// <returns></returns>
        public async Task<TagInfoCollection> GetTagInfo(params string[] tagnames)
        {
            var result = new TagInfoCollection();
            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取元数据的统计信息
        /// </summary>
        /// <returns></returns>
        public async Task<StatisticsInfo> Statistics()
        {
            var result = new StatisticsInfo();
            return await Task.FromResult(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !Disposed)
            {
                //ServiceProvider.Dispose();
            }
            Disposed = true;
            ServiceProvider = null;
        }
    }
}

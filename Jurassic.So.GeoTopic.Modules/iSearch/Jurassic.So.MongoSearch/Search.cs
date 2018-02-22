using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure.Model;
using Jurassic.So.ISearch.IOModel;
using Jurassic.So.ISearch.IOModel.InputParams;
using Jurassic.So.BusinessObjects;

namespace Jurassic.So.Search
{
    public class Search : ISearch.ISearch
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<QueryResult> Query(QueryCondition request)
        {
            QueryResult response = new QueryResult();
            response.Count = 1000;
            response.MetaDatas = new MetaDataCollection() { new MetaData() { { "iiid", "111" }, { "s:url", "adp:/vvv/vvv" }, { "ep.o.well", "井1" } }, new MetaData() { { "iiid", "222" }, { "s:url", "adp:/eee/ee" } } };
            response.Groups = new GroupCollection() { { "contributor", new Dictionary<string, int>() { { "jacky", 10 } } }, { "organazition", new Dictionary<string, int>() { { "中石油", 444 } } } };
            return await Task.FromResult(response);
        }
    }
}

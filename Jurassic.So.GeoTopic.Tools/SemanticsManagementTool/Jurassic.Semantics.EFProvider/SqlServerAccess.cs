using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.EFProvider
{
    public class SqlServerAccess 
    {
        private SemanticsDbContext Context { get; set; }

        public SqlServerAccess()
        {
            Context = new SemanticsDbContext();
        }
        public List<ES_SemanticsTermView> GetGlossaries(PagerInfo info)
        {
            return Context.ES_SemanticsTermView.OrderBy(o=>o.TermClassId).Skip((info.Page - 1) * info.PageSize).Take(info.PageSize).ToList();
        }
        public List<ES_EPsOfPTView> GetPtContext(PagerInfo info)
        {
            return  Context.ES_EPsOfPTView.OrderBy(o => o.TermClassId).Skip((info.Page - 1) * info.PageSize).Take(info.PageSize).ToList();
        }

        public int GetGlossariesCount()
        {
            return  Context.ES_SemanticsTermView.Count();
        }
        public int GetPtContextCount()
        {
            return Context.ES_EPsOfPTView.Count();
        }
    }
}

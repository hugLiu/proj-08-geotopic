using Jurassic.So.GeoTopic.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class UserBehaviorEFRepository : EntityFrameworkRepository<GT_UserBehavior>, IUserBehaviorEFRepository
    {
        public UserBehaviorEFRepository(GeoDbContext context) : base(context){}

        public IQueryable<GT_UserBehavior> GetQueryDb()
        {
            return this.GetQuery();
        }
    }
}

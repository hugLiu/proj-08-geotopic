using Jurassic.So.GeoTopic.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.Models;
using System.Data.Entity;

namespace Jurassic.So.GeoTopic.Database.Service
{
   public class UserProfileEFRepository: EntityFrameworkRepository<UserProfile>, IUserProfileEFRepository
    {
        public UserProfileEFRepository(GeoDbContext context) : base(context) { }
        public IQueryable<UserProfile> GetQueryDb()
        {
            return this.GetQuery();
        }
    }
}

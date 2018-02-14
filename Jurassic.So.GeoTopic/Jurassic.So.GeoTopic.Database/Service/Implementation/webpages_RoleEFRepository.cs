using Jurassic.So.GeoTopic.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class webpages_RoleEFRepository: EntityFrameworkRepository<webpages_Roles>, Iwebpages_RoleEFRepository
    {
        public webpages_RoleEFRepository(GeoDbContext context) : base(context) { }

        public IQueryable<webpages_Roles> GetQueryDb()
        {
            return this.GetQuery();
        }
    }
}

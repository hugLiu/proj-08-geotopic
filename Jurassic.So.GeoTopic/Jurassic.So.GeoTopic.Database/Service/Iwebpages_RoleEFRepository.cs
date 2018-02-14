using Jurassic.So.GeoTopic.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.Data;

namespace Jurassic.So.GeoTopic.Database.Service
{
    public interface Iwebpages_RoleEFRepository : IRepository<webpages_Roles>
    {
        IQueryable<webpages_Roles> GetQueryDb();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class webpages_MembershipEFRepository : EntityFrameworkRepository<webpages_Membership>, Iwebpages_MembershipEFRepository
    {
        public webpages_MembershipEFRepository(GeoDbContext context) : base(context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class webpages_OAuthMembershipEFRepository : EntityFrameworkRepository<webpages_OAuthMembership>, Iwebpages_OAuthMembershipEFRepository
    {
        public webpages_OAuthMembershipEFRepository(GeoDbContext context) : base(context) { }
    }
}

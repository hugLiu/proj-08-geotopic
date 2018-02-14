using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class GT_UrlTemplateEFRepository : EntityFrameworkRepository<GT_UrlTemplate>, IGT_UrlTemplateEFRepository
    {
        public GT_UrlTemplateEFRepository(GeoDbContext context) : base(context) { }
    }
}

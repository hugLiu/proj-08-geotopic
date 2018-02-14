using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class GT_RenderTypeEFRepository : EntityFrameworkRepository<GT_RenderType>, IGT_RenderTypeEFRepository
    {
        public GT_RenderTypeEFRepository(GeoDbContext context) : base(context) { }
    }
}

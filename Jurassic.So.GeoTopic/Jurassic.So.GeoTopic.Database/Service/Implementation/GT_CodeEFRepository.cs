using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
   public class GT_CodeEFRepository : EntityFrameworkRepository<GT_Code>, IGT_CodeEFRepository
    {
        public GT_CodeEFRepository(GeoDbContext context) : base(context) { }
    }
}

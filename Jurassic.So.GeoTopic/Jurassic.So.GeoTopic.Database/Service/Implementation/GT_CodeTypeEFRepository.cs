using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class GT_CodeTypeEFRepository : EntityFrameworkRepository<GT_CodeType>, IGT_CodeTypeEFRepository
    {
        public GT_CodeTypeEFRepository(GeoDbContext context) : base(context) { }
    }
}

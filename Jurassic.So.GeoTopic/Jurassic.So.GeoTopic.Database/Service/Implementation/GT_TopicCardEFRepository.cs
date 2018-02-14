using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class GT_TopicCardEFRepository : EntityFrameworkRepository<GT_TopicCard>, IGT_TopicCardEFRepository
    {
        public GT_TopicCardEFRepository(GeoDbContext context) : base(context) { }
    }
}

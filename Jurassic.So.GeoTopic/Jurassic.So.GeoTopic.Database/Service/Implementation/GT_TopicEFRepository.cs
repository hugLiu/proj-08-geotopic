using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class GT_TopicEFRepository : EntityFrameworkRepository<GT_Topic>,IGT_TopicEFRepository
    {
        public GT_TopicEFRepository(GeoDbContext context) : base(context) { }
        public IQueryable<GT_Topic> GetQueryDb()
        {
            return this.GetQuery();
        }
    }
}

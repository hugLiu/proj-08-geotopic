using Jurassic.So.GeoTopic.Database.EntityFramework;
using Jurassic.So.GeoTopic.Database.Models;

namespace Jurassic.So.GeoTopic.Database.Service.Implementation
{
    public class GT_RemarkEFRepository : EntityFrameworkRepository<GT_Remark>, IGT_RemarkEFRepository
    {
        public GT_RemarkEFRepository(GeoDbContext context) : base(context) { }
    }
}

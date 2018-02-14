using Jurassic.So.GeoTopic.Database.EntityFramework;
using System.Linq;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.Data;

namespace Jurassic.So.GeoTopic.Database.Service
{
    public interface IUserProfileEFRepository: IRepository<UserProfile> 
    {
        IQueryable<UserProfile> GetQueryDb();
    }
}

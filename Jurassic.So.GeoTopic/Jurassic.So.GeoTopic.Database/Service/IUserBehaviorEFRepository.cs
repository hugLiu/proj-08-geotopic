﻿using Jurassic.So.GeoTopic.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Data;

namespace Jurassic.So.GeoTopic.Database.Service
{
    public interface IUserBehaviorEFRepository : IRepository<GT_UserBehavior>
    {
        IQueryable<GT_UserBehavior> GetQueryDb();
    }
}

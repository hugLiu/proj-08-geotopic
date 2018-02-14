using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic.WebFrame.Providers;

namespace GTAPI.Security
{
    public static class DataSecurity
    {
        public static bool UserDataValidation(string userId,string dataId)
        {
            return AuthorizeComm.IsAuthData(userId, dataId);
        }
    }
}
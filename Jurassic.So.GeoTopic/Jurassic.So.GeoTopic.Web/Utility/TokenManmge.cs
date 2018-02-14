using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic.WebFrame.Providers;

namespace Jurassic.So.GeoTopic.Web.Utility
{
    /// <summary>
    /// WebApp动态授权的帮助类，授权的维护和管理。
    /// 原方法是维护多个授权，现在的是只有在web配置中一套授权，所以沿用了以前的结构织使用一套授权。
    /// </summary>
    public static class TokenManmge
    {
        private static Dictionary<string, string> UserTokens = new Dictionary<string, string>();


        private static string GetToken()
        {
            string issUser = System.Configuration.ConfigurationManager.AppSettings["ISSUser"];
            string issKey = System.Configuration.ConfigurationManager.AppSettings["ISSKey"];
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"];
            var token = AuthorizeComm.GetAccessTokenHeaders(apiUrl, issUser, issKey);
            return token.access_token;
        }

        /// <summary>
        /// WebApp中获取Token 的统一方法
        /// </summary>
        /// <returns></returns>
        public static string GetTokenService()
        {
            string issUser = System.Configuration.ConfigurationManager.AppSettings["ISSUser"];
            if (!UserTokens.ContainsKey(issUser))
                UserTokens.Add(issUser, GetToken());
            return UserTokens[issUser];
        }

        /// <summary>
        /// 清除Token
        /// </summary>
        public static void CleanToken(string token)
        {
            //注意：这里和搜油1.0不同，这里是从配置文件中拿固定的授权，不是可以给用户配置，会有多个授权的管理。
            //需要请参考搜油1.0
            string issUser = System.Configuration.ConfigurationManager.AppSettings["ISSUser"];
            //string issKey = System.Configuration.ConfigurationManager.AppSettings["ISSKey"];
            if (UserTokens.ContainsKey(issUser) && UserTokens[issUser].Equals(token))
                UserTokens.Remove(issUser);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public class GeoTopicService : IGeoTopicService
    {
        public IGT_TopicEFRepository TopicDbContext { get; set; }
        public IUserProfileEFRepository UserProfile { get; set; }

        public GeoTopicService(IGT_TopicEFRepository topicDbContext, IUserProfileEFRepository userProfile)
        {
            TopicDbContext = topicDbContext;
            UserProfile = userProfile;
        }


        public IEnumerable<KTopicModel> GetTopics(Dictionary<string, string> pram, string[] code, string[] title, string userId)
        {
            //这里树的过滤条件比较多，基本实现一次查询，除了title的时候需要查子节点，所以需要单独获取一次code。
            //这里数据量有可能会很大，要是逻辑继续增多，应该考虑储存过程。
            IQueryable<GT_Topic> geoKTopics=null;
            var _userId = int.Parse(userId);

            var query = TopicDbContext.GetQuery().Include("GT_TopicIndex").Include("GT_TopicIndex.GT_IndexDefinition").Include("webpages_Roles.UserProfile")
                .Where(t => t.webpages_Roles.Any(g =>g.UserProfile.Any(h=>h.UserId==_userId)));
                
            if (pram == null || pram.Count == 0)
            {
               geoKTopics = query;
            }                
            else
            {
                string[] arr = pram.Select(pr => pr.Key.Trim() + pr.Value.Trim()).ToArray();
                geoKTopics = query.Where(o => o.GT_TopicIndex.Any(g => arr.Contains(g.GT_IndexDefinition.Code + g.Value)));
            }
            if (code != null && code.Length >0)
                geoKTopics = SelectTreeCode(code, geoKTopics);
            else
            {
                if (title != null && title.Length > 0)
                {
                    string[] codes = geoKTopics.Where(o => title.Contains(o.Title)).Select(o => o.Code).ToArray();
                    geoKTopics = SelectTreeCode(codes, geoKTopics);
                }
            }

            return geoKTopics.Select(Mapper.Map<GT_Topic, KTopicModel>).ToList();
        }

        private IQueryable<GT_Topic> SelectTreeCode(string[] codes, IQueryable<GT_Topic> rs)
        {
            for (int i = 0; i < codes.Count(); i++)
            {
                int len = codes[i].Length;
                rs = rs.Where(o => codes.Contains(o.Code.Substring(0, len)));
            }
            return rs;
        }

    }
}

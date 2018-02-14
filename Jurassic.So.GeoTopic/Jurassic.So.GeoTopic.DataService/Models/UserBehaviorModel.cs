
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public static class UserBehaviorType
    {
        public static string Visited = "visited";
        public static string Download = "download";
        public static string Favorite = "favorite";
    }

    public class UserBehaviorModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string BehaviorType { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }
    }
}

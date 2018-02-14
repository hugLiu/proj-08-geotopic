
using System;
using System.Collections.Generic;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class RemarkModel
    {
        public int Id { get; set; }
        public string Scoap { get; set; }
        public string NatureKey { get; set; }
        public string Comment { get; set; }
        public int? UserId { get; set; }
        public string UserPhotoUrl { get; set; }
        public List<UserProfileModel> UserProfile1 { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

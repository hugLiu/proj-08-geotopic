using System;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    /// <summary>
    /// 参数模板
    /// </summary>
    public class PTempModel
    {
        //public int Id { get; set; }
        //public int Pid { get; set; }
        //public int Urlid { get; set; }
        //public string Name { get; set; }
        //public string Key { get; set; }
        //public string Value { get; set; }
        //public string Type { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public string Creator { get; set; }
        //public bool IsDelete { get; set; }
        //public string _state { get; set; }


        public int Id { get; set; }
        public int RenderUrlId { get; set; }
        public string Defintion { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDelete { get; set; }
        public string _state { get; set; }

    }
}
using GeoJSON.Net;

namespace Jurassic.Sooil.IServiceBase
{
    /// <summary>
    /// 用于ES创建索引
    /// </summary>
    public class Coverage:JsonBase
    {
        public Coverage()
        {
            this.Region = string.Empty;
            this.Datum = string.Empty;
            this.Scale = string.Empty;
        }
        public string Region { get; set; }
        public string Datum { get; set; }
        public string Scale { get; set; }
        public GeoJSONObject Spatial { get; set; }
    }
}

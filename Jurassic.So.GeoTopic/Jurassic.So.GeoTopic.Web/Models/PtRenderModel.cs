using System.Collections.Generic;
namespace Jurassic.So.GeoTopic.Web.Models
{
    /// <summary>
    /// FilesItem
    /// </summary>
    public class FilesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        public string FileUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

    }


    /// <summary>
    /// PtRender返回类
    /// </summary>
    public class PtRenderItem
    {
        public string Iiid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<FilesItem> Files { get; set; }

    }

    public class PtRenderModel
    {
        public List<PtRenderItem> PtRender { get; set; }

    }
}
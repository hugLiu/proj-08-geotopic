using System;
using System.Runtime.Serialization;

namespace Jurassic.ServiceModels
{
    /// <summary>数据格式</summary>
    [Serializable]
    public enum DataFormat
    {
        /// <summary>默认</summary>
        [MimeType(MimeTypeConst.Stream, true)]
        Default,
        /// <summary>Word文档</summary>
        [MimeType(MimeTypeConst.DOC, true)]
        DOC,
        /// <summary>DOCX</summary>
        [MimeType(MimeTypeConst.DOCX, true)]
        DOCX,
        /// <summary>PPT</summary>
        [MimeType(MimeTypeConst.PPT, true)]
        PPT,
        /// <summary>PPTX</summary>
        [MimeType(MimeTypeConst.PPTX, true)]
        PPTX,
        /// <summary>XLS</summary>
        [MimeType(MimeTypeConst.XLS, true)]
        XLS,
        /// <summary>XLSX</summary>
        [MimeType(MimeTypeConst.XLSX, true)]
        XLSX,
        /// <summary>HTML</summary>
        [MimeType(MimeTypeConst.HTML)]
        HTML,
        /// <summary>TEXT</summary>
        [MimeType(MimeTypeConst.TEXT)]
        TEXT,
        /// <summary>XML</summary>
        [MimeType(MimeTypeConst.XML)]
        XML,
        /// <summary>PNG</summary>
        [MimeType(MimeTypeConst.PNG, true)]
        PNG,
        /// <summary>JPG</summary>
        [MimeType(MimeTypeConst.JPG, true)]
        JPG,
        /// <summary>BMP</summary>
        [MimeType(MimeTypeConst.BMP, true)]
        BMP,
        /// <summary>TIF</summary>
        [MimeType(MimeTypeConst.TIF, true)]
        TIF,
        /// <summary>JSON</summary>
        [MimeType(MimeTypeConst.JSON)]
        JSON,
        /// <summary>链接</summary>
        [MimeType(MimeTypeConst.Url)]
        URL,
        /// <summary>数据集</summary>
        [MimeType(MimeTypeConst.DataSet)]
        DataSet,
        /// <summary>GDB</summary>
        [MimeType(MimeTypeConst.GDB, true)]
        GDB,
        /// <summary>3GX</summary>
        [MimeType(MimeTypeConst._3GX)]
        _3GX,
    }
}

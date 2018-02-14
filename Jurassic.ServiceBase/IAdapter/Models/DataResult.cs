using Jurassic.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Adapter
{
    /// <summary>成果的数据项</summary>
    [Serializable]
    [DataContract]
    public class DataResult
    {
        /// <summary>数据格式</summary>
        [DataMember]
        public DataFormat Format { get; set; }

        /// <summary>
        /// 返回的数据内容，
        /// 如果<c>Format</c>为<c>DataFormat.DOC</c>、<c>DataFormat.DOCX</c>
        /// 、<c>DataFormat.PPT</c>、<c>DataFormat.PPTX</c>、<c>DataFormat.XLS</c>
        /// 、<c>DataFormat.XLSX</c>、<c>DataFormat.PNG</c>、
        /// 、<c>DataFormat.JPG</c>、<c>DataFormat.BMP</c>、<c>DataFormat.TIF</c>
        /// 、<c>DataFormat.GDB</c>，返回值应该为<c>System.IO.Stream</c>;
        /// 其它的为<c>System.String</c>，注意：字符串应为utf-8编码
        /// </summary>
        [DataMember]
        public object Value { get; set; }

        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}

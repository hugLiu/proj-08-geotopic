using System.Collections.Generic;

namespace Jurassic.Sooil.IServiceBase
{
    public class KeyWord
    {
        public KeyWord()
        {
           this.Translates = new List<string>();
           this.Similars = new List<string>();
        }

        /// <summary>
        /// 叙词
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// 翻译词
        /// </summary>
        public List<string>  Translates { get; set; }

        /// <summary>
        /// 同义词
        /// </summary>
        public List<string> Similars { get; set; }

        
    }
}

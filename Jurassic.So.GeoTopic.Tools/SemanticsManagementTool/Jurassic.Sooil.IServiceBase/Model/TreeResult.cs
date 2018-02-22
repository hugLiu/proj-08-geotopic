using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class TreeResult
    {
        public TreeResult()
        {
            this.TreeItems = new List<TreeItem>();
        }
        public IList<TreeItem> TreeItems { get; set; }
     
        public void AddRange(IList<TreeItem> collection)
        {
            if (collection != null && collection.Any())
            {
                foreach (var item in collection)
                {
                    this.TreeItems.Add(item);
                }
            }
        }
    }
    public class TreeItem
    {
        public TreeItem() { }
        public TreeItem(string id, string pathTerm, string term, int orderIndex, string source, string pid)
        {
            ID = id;
            PathTerm = pathTerm;
            Term = term;
            OrderIndex = orderIndex;
            Source = source;
            PID = pid;
        }
        public TreeItem(string id, string term, string pid)
        {
            ID = id;
            PathTerm = string.Empty;
            Term = term;
            OrderIndex = 0;
            PID = pid;
        }
        public TreeItem(string id, string term,string pathTerm, string pid)
        {
            ID = id;
            PathTerm = pathTerm;
            Term = term;
            OrderIndex = 0;
            PID = pid;
        }
        public TreeItem(string term)
        {
            ID = string.Empty;
            PathTerm = string.Empty;
            Term = term;
            OrderIndex = 0;
            PID = string.Empty;
        }
        public string ID { get; set; }
        /// <summary>
        /// add by :shiyaru
        /// date :   2015/10/14
        /// </summary>
        public string Term { get; set; }
        public string PathTerm { get; set; }
        /// <summary>
        /// 2015/11/19 
        /// TreeResult结构添加Source和OrderIndex两个字段
        /// </summary>
        public string Source { get; set; }
        public int? OrderIndex { get; set; }
        public string PID { get; set; }

    }
}

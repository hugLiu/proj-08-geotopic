using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.Database.Models
{
    public class CardDefModel
    {
        public IList<CellModel> cells { get; set; }
        public RootModel layout { get; set; }
    }
    public class RootModel
    {
        public string style { get; set; }
        public IList<Row> Rows { get; set; }
    }
    public class Row
    {
        public string style { get; set; }
        public IList<Col> Cols { get; set; }
    }
    public class Col
    {
        public string style { get; set; }
        public IList<Row> Rows { get; set; }
        public string CellId { get; set; }
    }

    public class CellModel
    {
        public string id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string param { get; set; }
    }

    //public class Params
    //{
    //    public string method { get; set; }
    //    public string username { get; set; }
    //    public Dictionary<string, IList<Dictionary<string, string>>> filter { get; set; }
    //    public Dictionary<string,string> table { get; set; }
    //    public Dictionary<string,string> tags { get; set; }
    //}
}

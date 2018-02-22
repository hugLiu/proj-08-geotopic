using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase//Jurassic.Sooil.ElasticSearch.ESModel
{
    public class Group
    {
        public Group()
        {

        }
        public Group(string name,string searchField)
        {
            this.Name = name;
            this.SearchField = searchField;
        }
        public string Name { get; set; }
        public string SearchField { get; set; }
    }
}

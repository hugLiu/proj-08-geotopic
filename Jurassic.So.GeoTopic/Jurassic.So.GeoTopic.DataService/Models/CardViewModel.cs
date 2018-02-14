using System.Collections.Generic;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class CardViewModel
    {
        public string name { get; set; }
        public List<string> ways { get; set; }
        public List<Component> component { get; set; }
    }

    public class Component
    {
        public int index { get; set; }
        public int rowNum { get; set; }
        public int columnNum { get; set; }
        public Node param { get; set; }
    }

    public class Node
    {
        public string id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string param { get; set; }
    }
}
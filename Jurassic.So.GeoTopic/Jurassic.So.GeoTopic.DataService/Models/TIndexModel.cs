namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class TIndexModel
    {
        //public int Id { get; set; }
        //public int TopicId { get; set; }
        //public int DicNameId { get; set; }
        //public string Name { get; set; }
        //public string Value { get; set; }
        //public bool IsDelete { get; set; }
        //public string _state { get; set; }


        public int Id { get; set; }
        public int TopicId { get; set; }
        public int IndexDefinitionId { get; set; }
        public string IndexDefinitionTitle { get; set; }
        public string Value { get; set; }
        public bool IsDelete { get; set; }
        public string _state { get; set; }

    }
}
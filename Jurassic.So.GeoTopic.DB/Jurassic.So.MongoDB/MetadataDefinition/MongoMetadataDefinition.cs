using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jurassic.So.Index
{
    /// <summary>Mongo元数据定义</summary>
    internal class MongoMetadataDefinition
    {
        /// <summary>文档ID</summary>
        [BsonId]
        public ObjectId Id { get; set; }
        /// <summary>名称</summary>
        /// <remarks>系统内部使用的名称</remarks>
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("mapping")]
        public Mapping Mapping { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("required")]
        public bool Required { get; set; }
        [BsonElement("type")]
        public TagType Type { get; set; }
        [BsonElement("format")]
        public string Format { get; set; }
        [BsonElement("innerTag")]
        public bool InnerTag { get; set; }
        [BsonElement("groupname")]
        public string GroupName { get; set; }
        public string RegExp { get; set; }
        [BsonElement("uitype")]
        public UiType UiType { get; set; }
        [BsonElement("items")]
        public Options Items { get; set; }
        [BsonElement("grouporder")]
        public int GroupOrder { get; set; }
        [BsonElement("itemorder")]
        public int ItemOrder { get; set; }
        [BsonElement("showindetail")]
        public bool Showindetail { get; set; }
    }
}

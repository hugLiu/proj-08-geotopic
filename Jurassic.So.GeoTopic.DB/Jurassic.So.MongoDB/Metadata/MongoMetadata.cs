using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.MongoDB
{
    /// <summary>Mongo元数据定义</summary>
    [Serializable]
    public class MongoMetadata : BsonDocument
    {
        /// <summary>构造函数</summary>
        public MongoMetadata() { }
        /// <summary>构造函数</summary>
        public MongoMetadata(IDictionary<string, object> values) : base(values) { }
        /// <summary>适配器URL</summary>
        [BsonIgnore]
        public string Url
        {
            get { return (string)this[MetadataConsts.SourceUrl]; }
            set { base[MetadataConsts.SourceUrl] = value; }
        }
        /// <summary>索引数据ID</summary>
        [BsonIgnore]
        public string IIId
        {
            get { return (string)this[MetadataConsts.IIId]; }
            set { base[MetadataConsts.IIId] = value; }
        }
    }
}

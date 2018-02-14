using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using System;
using System.Linq;
using TDoc = Jurassic.So.Index.MongoMetadataDefinition;
using System.Collections.Generic;

namespace Jurassic.So.MongoDB
{
    /// <summary>Mongo索引数据访问</summary>
    public class MongoMetadataDefinitionRepository : IMetadataDefinitionRepository
    {
        /// <summary>构造函数</summary>
        public MongoMetadataDefinitionRepository()
        {
            this.Accessor = new MongoAccess<TDoc>(MongoConsts.Collection_MetaDataDefinition);
        }
        /// <summary>Mongo索引数据访问器</summary>
        private MongoAccess<TDoc> Accessor { get; set; }
        public MetadataDefinitionCollection GetAll()
        {
            return this.Accessor.GetMany(null, null, null)
                .MapTo<MetadataDefinition>()
                .ToList(new MetadataDefinitionCollection());
        }
    }
}

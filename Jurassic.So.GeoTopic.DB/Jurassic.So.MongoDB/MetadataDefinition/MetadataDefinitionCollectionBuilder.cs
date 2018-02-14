using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.MongoDB
{
    /// <summary>元数据定义建表工具</summary>
    public class MetadataDefinitionCollectionBuilder
    {
        /// <summary>建库</summary>
        public void Run()
        {
            var repository = new MongoMetadataDefinitionRepository();
            CreateCollection(repository);
            InsertSeeds(repository);
        }
        /// <summary>建库</summary>
        private void CreateCollection(MongoMetadataDefinitionRepository repository)
        {

        }
        /// <summary>插入种子数据</summary>
        private void InsertSeeds(MongoMetadataDefinitionRepository repository)
        {

        }
    }
}

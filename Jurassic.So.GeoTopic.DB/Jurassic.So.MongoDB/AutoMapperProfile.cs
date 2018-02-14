using AutoMapper;
using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.Index;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.MongoDB
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<MongoMetadataDefinition, MetadataDefinition>();
        }
    }
}

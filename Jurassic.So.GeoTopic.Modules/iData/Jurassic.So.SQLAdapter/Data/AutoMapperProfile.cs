using AutoMapper;
using Jurassic.PKS.Service;

namespace Jurassic.So.Adapter
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<MetadataDefinition, MetadataTag>()
                .ForMember(t => t.Enabled, opt => opt.Ignore())
                ;
        }
    }
}

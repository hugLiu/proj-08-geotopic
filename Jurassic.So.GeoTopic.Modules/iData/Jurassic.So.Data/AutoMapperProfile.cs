using System.Linq;
using AutoMapper;
using Jurassic.PKS.Service.Adapter;
using Jurassic.So.Business;
using Jurassic.So.Data.Entities;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Data.Center
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<GT_AdapterInfo, AdapterInfo>()
                .ForMember(t => t.Id, opt => opt.MapFrom(s => s.AdapterName))
                .ForMember(t => t.DataSourceName, opt => opt.MapFrom(s => s.DataSourceName))
                .ForMember(t => t.DataSourceType, opt => opt.MapFrom(s => s.DataSourceType))
                .ForMember(t => t.Scopes, opt => opt.MapFrom(s => s.GT_SpiderScope))
                ;
            CreateMap<GT_SpiderScope, Scope>()
                .ForMember(t => t.Name, opt => opt.MapFrom(s => s.SpiderScope))
                .ForMember(t => t.IncrementType, opt => opt.MapFrom(s => s.IncrementType))
                ;
        }
    }
}

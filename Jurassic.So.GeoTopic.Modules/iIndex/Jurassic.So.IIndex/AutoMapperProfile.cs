using AutoMapper;
using System.Collections.Generic;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Index
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<IndexInfoRequest, IndexInfo>()
                .ForMember(t => t.Metadatas, opt => opt.MapFrom(s => s.Metadatas.Cast<JObject>().JsonToMetadataList()))
                //.ReverseMap()
                ;
        }
    }
}

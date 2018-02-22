using AutoMapper;
using Jurassic.So.Business;
using System.Linq;
using Newtonsoft.Json.Linq;
using Jurassic.PKS.WebAPI.Models.Index;
using Jurassic.PKS.Service.Index;
using Jurassic.PKS.Service;

namespace Jurassic.So.Index.Mongo
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<IndexInfoRequest, IndexInfo>()
                .ForMember(t => t.Metadatas, opt => opt.ResolveUsing(new MetadatasResolver()))
               //.ForMember(t => t.Metadatas, opt => opt.MapFrom(s => new MetadataCollection(s.Metadatas.Cast<JObject>().Select(e => new JsonMetadata(e)))))
               ;
        }
        /// <summary>元数据解析器</summary>
        private class MetadatasResolver : IValueResolver<IndexInfoRequest, IndexInfo, MetadataCollection>
        {
            public MetadataCollection Resolve(IndexInfoRequest source, IndexInfo destination, MetadataCollection member, ResolutionContext context)
            {
                return source.Metadatas.Cast<JObject>().KMDJsonToMetadataList();
            }
        }
        //private class MetadatasResolver : IValueResolver<IndexInfoRequest, IndexInfo, MetadataCollection>
        //{
        //    public MetadataCollection Resolve(IndexInfoRequest source, IndexInfo destination, MetadataCollection member, ResolutionContext context)
        //    {
        //        var metadatas = new MetadataCollection();
        //        foreach (JObject metadata in source.Metadatas)
        //        {
        //            var metadata2 = new JsonMetadata();
        //            foreach (var pair in KMD.DefaultKmdConfiguration)
        //            {
        //                var value = metadata.SelectTokens(pair.Value.Mapping.Get);
        //                if (value == null) continue;
        //                metadata2.SetValue(pair.Value.Name, value.FirstOrDefault()?.As<JValue>()?.Value);
        //            }
        //            metadatas.Add(metadata2);
        //        }
        //        return metadatas;
        //    }
        //}
    }
}

using AutoMapper;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Search;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<MatchRequest, MatchCondition>()
                   .ForMember(t => t.Filter, opt => opt.ResolveUsing(new MatchJsonResolver()));
            //.ForMember(t => t.Filter, opt => opt.MapFrom(s => s.Filter));

            CreateMap<SearchRequest, SearchCondition>()
                  .ForMember(t => t.Filter, opt => opt.ResolveUsing(new SearchJsonResolver()));
            // .ForMember(t => t.Filter, opt => opt.MapFrom(s => s.Filter.FromExpressionJson()));
        }

        /// <summary>Match方法 请求参数filter和表达式的转化</summary>
        private class MatchJsonResolver : IValueResolver<MatchRequest, MatchCondition, IConditionalExpression>
        {
            public IConditionalExpression Resolve(MatchRequest source, MatchCondition destination, IConditionalExpression destMember,
                ResolutionContext context)
            {
                return source.Filter.FromExpressionJson();
            }
        }
        //private class MatchJsonResolver : IValueResolver<MatchRequest, MatchCondition, string>
        //{
        //    public string Resolve(MatchRequest source, MatchCondition destination, string destMember,
        //        ResolutionContext context)
        //    {
        //        return source.Filter.ToJson();
        //    }
        //}
        /// <summary>Search方法 请求参数filter和表达式的转化</summary>
        private class SearchJsonResolver : IValueResolver<SearchRequest, SearchCondition, IConditionalExpression>
        {
            public IConditionalExpression Resolve(SearchRequest source, SearchCondition destination, IConditionalExpression destMember,
                ResolutionContext context)
            {
                return source.Filter.FromExpressionJson();
            }
        }
    }
}

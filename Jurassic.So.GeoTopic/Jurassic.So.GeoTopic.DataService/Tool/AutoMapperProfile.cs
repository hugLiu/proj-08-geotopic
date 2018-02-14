using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.Database;

namespace Jurassic.So.GeoTopic.DataService.Tool
{
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// 创建模型映射
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<GT_Topic, KTopicModel>()
                .ForMember(t=>t._state,s=>s.Ignore())            
                .ForMember(t => t.fullPath, s => s.Ignore());

            //CreateMap<GT_TopicIndex, TIndexModel>()
            //    .ForMember(s => s.IndexDefinitionCode, t => t.MapFrom(e => e.GT_IndexDefinition.Code))
            //    .ForMember(s => s.Value, t => t.MapFrom(e => e.GT_IndexDefinition.Title));

            CreateMap<GT_IndexDefinition, Dic_tIndexModel>()
                .ForMember(t => t.CreateDate, s => s.Ignore())
                .ForMember(t => t.Creator, s => s.Ignore())
                .ForMember(t => t._state, s => s.Ignore());


            CreateMap<GT_UrlTemplate, PTempModel>()
                .ForMember(t => t._state, s => s.Ignore());

            CreateMap<UserProfile, UserProfileModel>();
            CreateMap<GT_TopicCard, TCardModel>();
            CreateMap<GT_RenderType, Dic_tTypeModel>()
                .ForMember(t => t.CreateDate, s => s.Ignore())
                .ForMember(t => t.Creator, s => s.Ignore())
                .ForMember(t => t._state, s => s.Ignore());

            CreateMap<GT_RenderUrl, Dic_tUrlModel>()
                .ForMember(t => t._state, s => s.Ignore())
                .ForMember(t => t.RenderTypeTitle, s => s.Ignore());
            CreateMap<webpages_Roles, webpages_RolesModel>();

            CreateMap<GT_Remark, RemarkModel>()
                .ForMember(t => t.UserPhotoUrl, s => s.Ignore());

            CreateMap<GT_UserBehavior, UserBehaviorModel>()
                .ReverseMap()
                .ForMember(dest => dest.UserProfile, src => src.Ignore())
                ;

            CreateMap<GT_Code, CodeModel>()
             .ForMember(t => t._state, s => s.Ignore());

            CreateMap<GT_CodeType, CodeTypeModel>()
             .ForMember(t => t._state, s => s.Ignore());
        }


    }
}

using AutoMapper;
using Jurassic.PKS.WebAPI.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Submission;

namespace Jurassic.So.Submission.Mock
{
    /// <summary>AutoMapper映射配置</summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>构造函数</summary>
        public AutoMapperProfile()
        {
            CreateMap<SubmissionInfoRequest, SubmissionInfo>()
                .ForMember(t => t.Action, opt => opt.MapFrom(s => s.Action))
                .ForMember(t => t.NatureKey, opt => opt.MapFrom(s => s.NatureKey))
                .ForMember(t => t.FileIDs, opt => opt.MapFrom(s => s.FileIDs))
                .ForMember(t => t.KMD, opt => opt.MapFrom(s => new KMD(s.KMD.ToJson(null))))
                .ForMember(t => t.Option, opt => opt.MapFrom(s => s.Option))
                ;
        }


    }
}

using System;
using AutoMapper;
using Jurassic.So.Data.Entities;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Jurassic.PKS.Service.Adapter;

namespace Jurassic.So.SpiderTool.Service.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<GT_AdapterInfo, AdapterInfoModel>()
                .ForMember(s => s.AdapterId, t => t.MapFrom(e => e.Id))
                .ForMember(s => s.CreateDate, t => t.MapFrom(e => e.CreatedDate))
                .ForMember(s => s.PlanInfoId, s => s.Ignore())
                .ForMember(s => s.SpiderCommand, s => s.Ignore())
                .ForMember(s => s.TaskCount, s => s.Ignore())
                .ForMember(s => s.TaskRunningCount, s => s.Ignore())
                .ForMember(s => s.ExecuteTimes, s => s.Ignore())
                .ForMember(s => s.ProcessedPercent, s => s.Ignore())
                ;

            CreateMap<AdapterInfo, GT_AdapterInfo>()
                .ForMember(s => s.Id, t => t.Ignore())
                .ForMember(s => s.AdapterName, t => t.MapFrom(e => e.Id))
                .ForMember(s => s.DataSourceName, t => t.MapFrom(e => e.DataSourceName))
                .ForMember(s => s.DataSourceType, t => t.MapFrom(e => e.DataSourceType))
                .ForMember(t => t.Desc, s => s.Ignore())
                .ForMember(t => t.SDomain, s => s.Ignore())
                .ForMember(t => t.AdapterURL, s => s.Ignore())
                .ForMember(t => t.Status, s => s.Ignore())
                .ForMember(t => t.InvokeType, s => s.Ignore())
                .ForMember(t => t.Hangup, s => s.Ignore())
                .ForMember(t => t.CreatedDate, s => s.Ignore())
                .ForMember(t => t.IsInvalid, s => s.Ignore())
                .ForMember(t => t.OrderIndex, s => s.Ignore())
                .ForMember(t => t.Remark, s => s.Ignore())
                .ForMember(t => t.ClassName, s => s.Ignore())
                .ForMember(t => t.ConfigAddress, s => s.Ignore())
                .ForMember(t => t.GT_SpiderScope, s => s.Ignore())
                .ForMember(t => t.SpiderSize, s => s.Ignore())
                .ForMember(t => t.GT_PlanInfo, s => s.Ignore())
                .ForMember(t => t.AdapterType, s => s.Ignore())
                .ForMember(t => t.CreatedBy, s => s.Ignore())
                .ForMember(t => t.UpdatedDate, s => s.Ignore())
                .ForMember(t => t.UpdatedBy, s => s.Ignore())
                ;

            CreateMap<GT_SpiderScope, SpiderScopeModel>()
                .ForMember(s => s.AdapterId, t => t.MapFrom(e => e.AdapterId))
                .ForMember(s => s.SpiderScope, t => t.MapFrom(e => e.SpiderScope))
                .ForMember(s => s.CreateDate, t => t.MapFrom(e => e.CreatedDate))
                .ForMember(t => t.AdapterName, s => s.Ignore())
                .ForMember(t => t.AnalyseTitle, s => s.Ignore())
                .ForMember(t => t.AnalyseDataType, s => s.Ignore())
                .ForMember(t => t.AnalyseDescription, s => s.Ignore())
                .ForMember(t => t.AnalyseFT, s => s.Ignore())
                .ForMember(t => t.AutoTag, s => s.Ignore())
                .ForMember(t => t.Description, s => s.Ignore())
                .ForMember(t => t.TaskMode, s => s.Ignore())
                .ForMember(t => t.AsTask, s => s.Ignore())
                .ForMember(t => t.SpiderCommand, s => s.Ignore())
                .ForMember(t => t.ProcessedPercent, s => s.Ignore())
                ;

            CreateMap<GT_PlanInfo, SchedulePlan>()
                .ForMember(t => t.Creator, s => s.MapFrom(e => e.CreatedBy))
                .ForMember(t => t.DayRun, s => s.Ignore())
                .ForMember(t => t.WeekRun, s => s.Ignore())
                .ForMember(t => t.MonthRun, s => s.Ignore())
                .ForMember(t => t.SelfRun, s => s.Ignore())
                .ForMember(t => t.LblPlanMsg, s => s.Ignore())
                .ForMember(t => t.Creator, s => s.Ignore())
                .ForMember(t => t.CreatedDate, s => s.Ignore())
                ;
            CreateMap<SchedulePlan, GT_PlanInfo>()
                .ForMember(t => t.CreatedBy, s => s.MapFrom(e => e.Creator))
                .ForMember(t => t.IsInvalid, s => s.Ignore())
                .ForMember(t => t.OrderIndex, s => s.Ignore())
                .ForMember(t => t.GT_AdapterInfo, s => s.Ignore())
                .ForMember(t => t.UpdatedDate, s => s.Ignore())
                .ForMember(t => t.UpdatedBy, s => s.Ignore())
                ;
        }
    }
}

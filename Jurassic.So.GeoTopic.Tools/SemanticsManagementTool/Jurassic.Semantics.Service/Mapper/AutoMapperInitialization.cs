using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using AutoMapper;
using Jurassic.Semantics.Entity.EntityModel;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.Semantics.Entity;
using Jurassic.Sooil.IServiceBase;

namespace Jurassic.Semantics.Service.Mapper
{
    public class AutoMapperInitialization
    {
        public static void Initialization()
        {
            AutoMapper.Mapper.CreateMap<ESPTContext, ES_EPsOfPTView>()
                 .ForMember(l => l.TermClassId, r => r.MapFrom(m => Guid.Parse(m.Id)));
            AutoMapper.Mapper.CreateMap<ES_EPsOfPTView, ESPTContext>()
                 .ForMember(l => l.Id, r => r.MapFrom(m => m.TermClassId.ToString()))
                 .ForMember(l => l.PT, r => r.MapFrom(m => m.PT ?? string.Empty))
                 .ForMember(l => l.UPT, r => r.MapFrom(m => m.UPT ?? string.Empty))
                 .ForMember(l => l.BP, r => r.MapFrom(m => m.BP ?? string.Empty))
                 .ForMember(l => l.UBP, r => r.MapFrom(m => m.UBP ?? string.Empty))
                 .ForMember(l => l.BD, r => r.MapFrom(m => m.BD ?? string.Empty))
                 .ForMember(l => l.UBD, r => r.MapFrom(m => m.UBD ?? string.Empty))
                 .ForMember(l => l.BA, r => r.MapFrom(m => m.BA ?? string.Empty))
                 .ForMember(l => l.UBA, r => r.MapFrom(m => m.UBA ?? string.Empty))
                 .ForMember(l => l.BT, r => r.MapFrom(m => m.BT ?? string.Empty))
                 .ForMember(l => l.UBT, r => r.MapFrom(m => m.UBT ?? string.Empty))
                 .ForMember(l => l.DS, r => r.MapFrom(m => m.DS ?? string.Empty))
                 .ForMember(l => l.UDS, r => r.MapFrom(m => m.UDS ?? string.Empty))
                 .ForMember(l => l.TL, r => r.MapFrom(m => m.TL ?? string.Empty))
                 .ForMember(l => l.GN, r => r.MapFrom(m => m.GN ?? string.Empty))
                 .ForMember(l => l.BS, r => r.MapFrom(m => m.BS ?? string.Empty))
                 .ForMember(l => l.BOT, r => r.MapFrom(m => m.BOT ?? string.Empty))
                 .ForMember(l => l.BF, r => r.MapFrom(m => m.BF ?? string.Empty));

            AutoMapper.Mapper.CreateMap<Glossaries, ES_SemanticsTermView>()
                .ForMember(l => l.ConceptClass, r => r.MapFrom(m => m.CC))
                .ForMember(l => l.TermClassId, r => r.MapFrom(m => Guid.Parse(m.GId)))
                .ForMember(l => l.Term, r => r.MapFrom(m => m.TermSequence));

            AutoMapper.Mapper.CreateMap<ES_SemanticsTermView, Glossaries>()
                .ForMember(l => l.CC, r => r.MapFrom(m => m.ConceptClass ?? string.Empty))
                .ForMember(l => l.GId, r => r.MapFrom(m => m.TermClassId.ToString()))
                .ForMember(l => l.TermSequence, r => r.MapFrom(m => m.Term ?? string.Empty))
                .ForMember(l => l.KeyWord, r => r.MapFrom(m => m.Keyword ?? string.Empty));

            AutoMapper.Mapper.CreateMap<ConceptClassmodel, SD_ConceptClass>();
            AutoMapper.Mapper.CreateMap<SD_ConceptClass, ConceptClassmodel>();
            AutoMapper.Mapper.CreateMap<BO_BOAlias, BoAliasModel>();
            AutoMapper.Mapper.CreateMap<BoAliasModel, BO_BOAlias>();
            AutoMapper.Mapper.CreateMap<BO_Relation, BoRelationModel>()
                .ForMember(s => s.Name1, d => d.MapFrom(m => m.BO_BaseInfo.Name))
                 .ForMember(s => s.Name2, d => d.MapFrom(m => m.BO_BaseInfo1.Name))
                  .ForMember(s => s.RelTypeName, d => d.MapFrom(m => m.RelTypeCode));

            AutoMapper.Mapper.CreateMap<BoRelationModel, BO_Relation>();

            AutoMapper.Mapper.CreateMap<SD_SemanticsType, SemanticsTypemodel>()
                 .ForMember(s => s.CC1name, d => d.MapFrom(m => m.CCCode1))
                 .ForMember(s => s.CC2name, d => d.MapFrom(m => m.CCCode2));

            AutoMapper.Mapper.CreateMap< SemanticsTypemodel,SD_SemanticsType>()
             .ForMember(s => s.CCCode1, d => d.MapFrom(m => m.CC1name))
             .ForMember(s => s.CCCode2, d => d.MapFrom(m => m.CC2name));

            AutoMapper.Mapper.CreateMap<BO_BaseInfo, BoBaseInfoModel>()
                .ForMember(s => s.SpaceLocationType, d => d.MapFrom(m => m.SpaceLocation == null ? "" : m.SpaceLocation.SpatialTypeName))
                .ForMember(s => s.SpaceLocationArea, d => d.MapFrom(m => m.SpaceLocation == null ? null : m.SpaceLocation.AsText()))
              .ForMember(s => s.Alias, d => d.MapFrom(m => string.Join(",", m.BO_BOAlias.Select(s => s.Alias).ToList())));

            AutoMapper.Mapper.CreateMap<BoBaseInfoModel, BO_BaseInfo>()
                //这里先按照点类型来对待，之后需要读取坐标类型来进行转化;
                .ForMember(s => s.SpaceLocation,
                    d =>
                        d.MapFrom(
                            m =>
                                m.SpaceLocationArea == ""
                                    ? null
                                    : DbGeometry.PointFromText(m.SpaceLocationArea, DbGeometry.DefaultCoordinateSystemId)));

            AutoMapper.Mapper.CreateMap<SemanticsTypemodel, SD_SemanticsType>();

            AutoMapper.Mapper.CreateMap<CcTermModel, SD_CCTerm>();
            AutoMapper.Mapper.CreateMap<SD_CCTerm, CcTermModel>();

            AutoMapper.Mapper.CreateMap<USP_GetTermTree, TermTreeModel>();
            AutoMapper.Mapper.CreateMap<TermTreeModel, USP_GetTermTree>();

            AutoMapper.Mapper.CreateMap<SD_Semantics, SemanticsModel>();

            AutoMapper.Mapper.CreateMap<SemanticsModel, SD_Semantics>();

            AutoMapper.Mapper.CreateMap<SD_TermSource, TermSourceModel>();
            AutoMapper.Mapper.CreateMap<TermSourceModel, SD_TermSource>();

            AutoMapper.Mapper.CreateMap<SD_TermTranslation, TermTranslationModel>();
            AutoMapper.Mapper.CreateMap<TermTranslationModel, SD_TermTranslation>();

            AutoMapper.Mapper.CreateMap<USP_BPAndPTTree, BPAndPTTreeModel>()
            .ForMember(s => s.IsPT, d => d.ResolveUsing<BoolResolver>().FromMember(x => x.IsPT))
             .ForMember(s => s.TermClassId, d => d.MapFrom(m => m.Id));

            AutoMapper.Mapper.CreateMap<SD_TermKeyword, TermKeyWords>();
        }
    }
    public class BoolResolver : ValueResolver<bool, string>
    {
        protected override string ResolveCore(bool source)
        {
            return source ? "Y" : "N";
        }
    }
}

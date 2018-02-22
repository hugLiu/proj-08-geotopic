using AutoMapper;
using Jurassic.PKS.Service.Semantics;
using Jurassic.So.Semantics.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Semantics.SQL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SD_ConceptClass, ConceptClass>();
            CreateMap<SD_SemanticsType, SemanticsType>();
            CreateMap<SD_CCTerm, TermInfo>()
                .ForMember(d => d.CC, opt => opt.MapFrom(s => s.SD_ConceptClass.CC));
        }
    }
}

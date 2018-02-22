using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.Service
{
    public class SemanticsTypeService : ISemanticsTypeService
    {
        public SemanticsTypeEFProvider SemanticsTypeEfProvider { get; set; }

        public SemanticsTypeService(SemanticsTypeEFProvider semanticsTypeEfProvider)
        {
            SemanticsTypeEfProvider = semanticsTypeEfProvider;
        }
        public List<SemanticsTypemodel> GetSemanticsTypeList()
        {
            return SemanticsTypeEfProvider.GetSemanticsTypeList().Select(AutoMapper.Mapper.Map<SemanticsTypemodel>).ToList();
        }
        public void Add(SemanticsTypemodel model)
        {
            SemanticsTypeEfProvider.Add(AutoMapper.Mapper.Map<SD_SemanticsType>(model));
        }
        public void Edit(string sr, SemanticsTypemodel newSdSemanticsType)
        {
            SemanticsTypeEfProvider.Edit(sr, AutoMapper.Mapper.Map<SD_SemanticsType>(newSdSemanticsType));
        }
        public void Delete(string sr)
        {
            SemanticsTypeEfProvider.Delete(sr);
        }
    }
}

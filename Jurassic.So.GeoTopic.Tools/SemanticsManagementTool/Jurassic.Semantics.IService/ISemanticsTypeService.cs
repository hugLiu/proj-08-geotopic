using Jurassic.Semantics.IService.ViewModel;
using System.Collections.Generic;

namespace Jurassic.Semantics.IService
{
   public interface ISemanticsTypeService
    {
        List<SemanticsTypemodel> GetSemanticsTypeList();

        void Add(SemanticsTypemodel model);

        void Edit(string sr, SemanticsTypemodel newSemanticsType);

        void Delete(string sr);

    }
}

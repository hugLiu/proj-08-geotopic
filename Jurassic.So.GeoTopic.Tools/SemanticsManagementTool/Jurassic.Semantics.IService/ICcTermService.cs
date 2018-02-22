using System;
using System.Collections.Generic;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface ICcTermService
    {
        List<CcTermModel> CcTermList(string CCCode);
        void CcTermDelete(Guid deleteGuid);
        void CcTermEdit(Guid editGuid, CcTermModel newCcTermModel);
        void CcTermAdd(CcTermModel newCcModel,string text);
    }
}

using System;
using System.Collections.Generic;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface IConceptClassService
    {
        List<ConceptClassmodel> GetConceptClassList();
        void Add(ConceptClassmodel model);
        void Edit(string cccode, ConceptClassmodel conceptClassmodel);
        void Delete(string cccode);
    }
}

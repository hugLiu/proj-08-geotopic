using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.Semantics.Service
{
    public class ConceptClassService : IConceptClassService
    {
        public ConceptClassEfPrrovied ConceptClassEfPrrovied { get; set; }

        public ConceptClassService(ConceptClassEfPrrovied conceptClassEfPrrovied)
        {
            ConceptClassEfPrrovied = conceptClassEfPrrovied;
        }

        public List<ConceptClassmodel> GetConceptClassList()
        {
            return ConceptClassEfPrrovied.GetConceptClassList()
                    .Select(AutoMapper.Mapper.Map<SD_ConceptClass, ConceptClassmodel>)
                    .ToList();
        }

        public void Add(ConceptClassmodel model)
        {
            ConceptClassEfPrrovied.Add(AutoMapper.Mapper.Map<SD_ConceptClass>(model));
        }

        public void Edit(string cccode, ConceptClassmodel newSdConceptClass)
        {
            ConceptClassEfPrrovied.Edit(cccode, AutoMapper.Mapper.Map<SD_ConceptClass>(newSdConceptClass));
        }

        public void Delete(string cccode)
        {
            ConceptClassEfPrrovied.Delete(cccode);
        }
    }
}

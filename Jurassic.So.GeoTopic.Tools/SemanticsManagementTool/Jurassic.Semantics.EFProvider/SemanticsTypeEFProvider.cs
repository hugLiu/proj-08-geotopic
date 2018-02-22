using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;

namespace Jurassic.Semantics.EFProvider
{
    public class SemanticsTypeEFProvider
    {
        private SemanticsDbContext semanticsDbContext;

        public SemanticsTypeEFProvider()
        {
            semanticsDbContext = new SemanticsDbContext();
        }

        public List<SD_SemanticsType> GetSemanticsTypeList()
        {
            return semanticsDbContext.SD_SemanticsType.ToList();
        }

        public void Add(SD_SemanticsType model)
        {
            semanticsDbContext.SD_SemanticsType.Add(model);
            semanticsDbContext.SaveChanges();
        }


        public string GetCC(string ccName)
        {
            string cc = string.Empty;
            var conceptClasslist = semanticsDbContext.SD_ConceptClass.ToList();
            foreach (SD_ConceptClass conceptClass in conceptClasslist)
            {
                if (conceptClass.CC == ccName)
                {
                    cc = conceptClass.CCCode;
                    break;
                }
            }
            return cc;
        }

        public void Edit(string sr, SD_SemanticsType newSdSemanticsType)
        {
            SD_SemanticsType semanticsType = semanticsDbContext.SD_SemanticsType.FirstOrDefault(e => e.SR == sr);
            if (semanticsType != null)
            {
                semanticsType.SR = newSdSemanticsType.SR;
                semanticsType.CCCode1 = newSdSemanticsType.CCCode1;
                semanticsType.CCCode2 = newSdSemanticsType.CCCode2;
                semanticsType.Description = newSdSemanticsType.Description;
            }
            semanticsDbContext.SaveChanges();
        }

        public void Delete(string sr)
        {
            SD_Semantics semantics = semanticsDbContext.SD_Semantics.FirstOrDefault(se => se.SR == sr);

            SD_SemanticsType semanticsType = semanticsDbContext.SD_SemanticsType.FirstOrDefault(se => se.SR == sr);
            if (semantics == null && semanticsType != null)
            {
                semanticsDbContext.SD_SemanticsType.Remove(semanticsType);
                semanticsDbContext.SaveChanges();
            }
        }






    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;


namespace Jurassic.Semantics.EFProvider
{
    public class ConceptClassEfPrrovied
    {
        private readonly SemanticsDbContext _semanticsDbContext;

        public ConceptClassEfPrrovied()
        {
            _semanticsDbContext = new SemanticsDbContext();
        }

        /// <summary>
        /// 得到叙词概念类的数据
        /// </summary>
        /// <returns></returns>
        public List<SD_ConceptClass> GetConceptClassList()
        {
            return _semanticsDbContext.SD_ConceptClass.ToList();
        }
        public void Add(SD_ConceptClass model)
        {
            _semanticsDbContext.SD_ConceptClass.Add(model);
            _semanticsDbContext.SaveChanges();
        }

        public void Edit(string cccode, SD_ConceptClass newSdConceptClass)
        {
            SD_ConceptClass oldSdConceptClass =
                _semanticsDbContext.SD_ConceptClass.FirstOrDefault(sd => sd.CCCode == cccode);
            if (oldSdConceptClass != null)
            {
                oldSdConceptClass.CC = newSdConceptClass.CC;
                oldSdConceptClass.CCCode = newSdConceptClass.CCCode;
                oldSdConceptClass.Source = newSdConceptClass.Source;
                oldSdConceptClass.Tag = newSdConceptClass.Tag;
                oldSdConceptClass.Type = newSdConceptClass.Type;

                _semanticsDbContext.SaveChanges();
            }

        }

        public void Delete(string cccode)
        {
            var sdCon = _semanticsDbContext.SD_ConceptClass.FirstOrDefault(sd => sd.CCCode == cccode);
            if (sdCon != null)
            {
                try
                {
                    _semanticsDbContext.SD_ConceptClass.Remove(sdCon);
                    _semanticsDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("该概念类已被使用，不能删除！");
                }
            }
        }
    }
}

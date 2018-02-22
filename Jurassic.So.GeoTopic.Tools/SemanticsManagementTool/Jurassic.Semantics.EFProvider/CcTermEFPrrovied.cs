using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;


namespace Jurassic.Semantics.EFProvider
{
    public class CcTermEfPrrovied
    {
        private readonly SemanticsDbContext _semanticsDbContext;
        private List<SD_CCTerm> sdlist = null;

        public CcTermEfPrrovied()
        {
            _semanticsDbContext = new SemanticsDbContext();
        }


        #region 获取数据

        public SD_CCTerm GetCcTermById(Guid id)
        {
            return _semanticsDbContext.SD_CCTerm.FirstOrDefault(o => o.TermClassID == id);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="guid">根据TermClassId找到对应的数据</param>
        /// <returns></returns>
        public List<SD_CCTerm> GetCcTermsData(string CCCode)
        {
            List<SD_CCTerm> sdlist = null;
            if (CCCode == "")
            {
                sdlist = _semanticsDbContext.SD_CCTerm.Where(o => o.CCCode == "BOT").ToList();
            }
            else
            {

                sdlist = _semanticsDbContext.SD_CCTerm.Where(o => o.CCCode == CCCode).ToList();
            }
            return sdlist;
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除对应的数据
        /// 1、要删除的数据有主外键联系时,先做判断，如果有关联的主外键，则进行不删除；
        /// 2、没有关联的主外键关系，进行删除；
        /// 3、删除的依据是：TermClassId
        /// </summary>
        /// <param name="deleteGuid">删除的主要依据</param>
        public void DeleteEfPro(Guid deleteGuid)
        {
            SD_CCTerm sdCcTerm = _semanticsDbContext.SD_CCTerm.FirstOrDefault(cc => cc.TermClassID == deleteGuid);
            SD_Semantics sdse =
                _semanticsDbContext.SD_Semantics.FirstOrDefault(
                    se => se.FTermClassId == deleteGuid || se.LTermClassId == deleteGuid);
            SD_TermTranslation sdTerm =
                _semanticsDbContext.SD_TermTranslation.FirstOrDefault(st => st.TermClassID == deleteGuid);
            SD_TermKeyword tk = _semanticsDbContext.SD_TermKeyword.FirstOrDefault(t => t.TermClassID == deleteGuid);

            if (sdCcTerm != null && sdse == null && sdTerm == null && tk == null)
            {
                _semanticsDbContext.SD_CCTerm.Remove(sdCcTerm);
                _semanticsDbContext.SaveChanges();
            }
        }
        #endregion

        #region 修改数据

        public void EditEfPro(Guid editGuid, SD_CCTerm newCTermModel)
        {
            SD_CCTerm oldSdCcTerm = _semanticsDbContext.SD_CCTerm.FirstOrDefault(sc => sc.TermClassID == editGuid);
            if (oldSdCcTerm != null)
            {
                oldSdCcTerm.Term = newCTermModel.Term;
                oldSdCcTerm.Source = newCTermModel.Source;
                oldSdCcTerm.OrderIndex = newCTermModel.OrderIndex;
                oldSdCcTerm.CreatedDate = newCTermModel.CreatedDate;
                oldSdCcTerm.CreatedBy = newCTermModel.CreatedBy;
                _semanticsDbContext.SaveChanges();
            }
        }

        #endregion

        #region 添加数据

        public void AddEfPro(SD_CCTerm newSdCcmodel, string text)
        {
            newSdCcmodel.OrderIndex = _semanticsDbContext.SD_CCTerm.Count(o => o.CCCode == text) + 1;
            //newSdCcmodel.CCCode = sdlist.Select(o => o.CCCode).First();
            _semanticsDbContext.SD_CCTerm.Add(newSdCcmodel);
            _semanticsDbContext.SaveChanges();
        }

        #endregion
    }
}

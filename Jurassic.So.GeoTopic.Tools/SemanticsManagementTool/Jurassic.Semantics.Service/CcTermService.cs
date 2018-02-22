using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.Semantics.EFProvider;

namespace Jurassic.Semantics.Service
{
    public class CcTermService : ICcTermService
    {
        public CcTermEfPrrovied _CcTermEfPrrovied { get; set; }

        public CcTermService(CcTermEfPrrovied conTermEfPrrovied)
        {
            _CcTermEfPrrovied = conTermEfPrrovied;
        }

        #region 获取数据
        public List<CcTermModel> CcTermList(string CCCode)
        {
            string[] getCcTerms = GetCcTermCom.TermChangeToConceptId(CCCode);

            List<CcTermModel> Cclist = null;
            if (CCCode != "")
            {
                Cclist =
                    _CcTermEfPrrovied.GetCcTermsData(getCcTerms[0])
                        .Select(AutoMapper.Mapper.Map<SD_CCTerm, CcTermModel>)
                        .ToList();
            }
            if (CCCode == "")
            {
                Cclist = _CcTermEfPrrovied.GetCcTermsData("IC/BOT")
                                     .Select(AutoMapper.Mapper.Map<SD_CCTerm, CcTermModel>)
                                       .ToList();
            }
            return Cclist;
        }
        #endregion

        #region 删除数据
        public void CcTermDelete(Guid deleteGuid)
        {
            _CcTermEfPrrovied.DeleteEfPro(deleteGuid);
        }
        #endregion

        #region 编辑/修改数据
        public void CcTermEdit(Guid editGuid, CcTermModel newCcmodel)
        {
            _CcTermEfPrrovied.EditEfPro(editGuid, AutoMapper.Mapper.Map<SD_CCTerm>(newCcmodel));
        }
        #endregion

        #region 添加数据
        public void CcTermAdd(CcTermModel newCcModel, string text)
        {
            _CcTermEfPrrovied.AddEfPro(AutoMapper.Mapper.Map<SD_CCTerm>(newCcModel),text);
        }
        #endregion
    }
}

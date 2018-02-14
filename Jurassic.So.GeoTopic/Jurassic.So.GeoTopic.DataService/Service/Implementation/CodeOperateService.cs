using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Enum;
using Jurassic.So.GeoTopic.DataService.Models;

namespace Jurassic.So.GeoTopic.DataService
{
    public class CodeOperateService: ICodeOperateService
    {
        public IGT_CodeEFRepository gt_Code { get; set; }

        public IGT_CodeTypeEFRepository gt_CodeType { get; set; }


        public CodeOperateService(IGT_CodeEFRepository gtCode, IGT_CodeTypeEFRepository gtCodeType)
        {
            gt_Code = gtCode;
            gt_CodeType = gtCodeType;
        }



        /// <summary>
        /// 获取所有的码表类型
        /// </summary>
        /// <returns></returns>
        public List<CodeTypeModel> GetAllCodeType()
        {
            var codeTypes=gt_CodeType.GetAll();
            if (codeTypes == null)
                return new List<CodeTypeModel>();
            return codeTypes.Select(AutoMapper.Mapper.Map<GT_CodeType, CodeTypeModel>).ToList();
            //return AutoMapper.Mapper.Map<List<GT_CodeType>,List<CodeTypeModel>>(codeTypes);
        }


        /// <summary>
        /// 获取码表记录
        /// </summary>
        /// <param name="codeTypeId"></param>
        /// <returns></returns>
        public List<CodeModel> GetCodes(int? codeTypeId)
        {
            var currentCodes = new List<CodeModel>();
            var allCodes = gt_Code.GetAll();
            if(allCodes==null)
                return currentCodes;
            if (codeTypeId == null)
            {
                currentCodes = allCodes.Select(AutoMapper.Mapper.Map<GT_Code, CodeModel>).ToList();
            }
            else
            {
                var nowCode = allCodes.FindAll(t => t.CodeTypeId == codeTypeId);
                currentCodes = nowCode.Select(AutoMapper.Mapper.Map<GT_Code, CodeModel>).ToList();
            }
            return currentCodes;
        }



        /// <summary>
        /// 编辑码表信息
        /// </summary>
        /// <param name="codeModel">传入码表对象</param>
        public void UpdateCode(List<CodeModel>  codeModels)
        {
            foreach (var code in codeModels)
            {
                if (code._state== EnumNodeState.added.ToString())
                {
                    AddCode(code);
                }
                if (code._state == EnumNodeState.modified.ToString())
                {
                    ModifyCode(code);
                }
                if (code._state == EnumNodeState.removed.ToString())
                {
                    DeleteCode(code);
                }
            }
        }


        /// <summary>
        /// 添加码表记录
        /// </summary>
        /// <param name="codeModel"></param>
        private void AddCode(CodeModel codeModel)
        {
            var gtCode=new GT_Code()
            {
                Code = codeModel.Code,
                Title = codeModel.Title,
                CodeTypeId = codeModel.CodeTypeId,
                CreatedBy = "pmis",
                CreatedDate = DateTime.Now,
                UpdatedBy = "pmis",
                UpdatedDate = DateTime.Now
            };
            gt_Code.Add(gtCode);
        }

        /// <summary>
        /// 更新码表记录
        /// </summary>
        /// <param name="codeModel"></param>
        private void ModifyCode(CodeModel codeModel)
        {
            var codeModify = gt_Code.Find(t => t.Id == codeModel.Id);
            if(codeModify==null) return;
            codeModify.Code = codeModel.Code;
            codeModify.Title = codeModel.Title;
            codeModify.CodeTypeId = codeModel.CodeTypeId;
            codeModify.UpdatedDate=DateTime.Now;

            gt_Code.Update(codeModify);
        }
        /// <summary>
        /// 删除码表记录
        /// </summary>
        /// <param name="codeModel"></param>
        private void DeleteCode(CodeModel codeModel)
        {
            var codeDelete = gt_Code.Find(t => t.Id == codeModel.Id);
            if(codeDelete==null)    return;
            gt_Code.Delete(codeDelete);
        }
    }
}

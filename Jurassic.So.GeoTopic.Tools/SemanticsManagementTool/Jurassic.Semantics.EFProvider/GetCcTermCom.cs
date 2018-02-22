using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.EFProvider
{
    public class GetCcTermCom
    {
        //public static string[] StrConceptId;
        //public static string[] StrCccode;
        //public static string[] StrSr;
        //public static string[] StrCc;
        public static string[] TermChangeToConceptId(string comboxvalue)
        {
            //GlossaryManageEFPrrovied glossaryManage = new GlossaryManageEFPrrovied();
            //StrConceptId=glossaryManage.GetConceptClass().Select(o => o.ConceptClassID.ToString()).ToArray();
            //StrCccode=glossaryManage.GetConceptClass().Select(o => o.CCCode).ToArray();
            //StrCc = glossaryManage.GetConceptClass().Select(o => o.CC.Remove(0,5)).ToArray();  
            //StrSr=glossaryManage.GetSr();
            string[] getTree = new string[3];
            if (comboxvalue == "")
            {
                getTree[0] = "IC/BP";
                getTree[1] = "R_BP_XF_BP";
                getTree[2] = "B207CE55-8F0A-41ED-9072-7155509E3269";
            }
            if (comboxvalue == "体裁")
            {
                getTree[0] = "IC/GN";
                getTree[1] = "R_GN_XF_GN";
                getTree[2] = "4BE5CDB0-B3B1-49D6-B8CD-07A57A13D505";
            }
            if (comboxvalue == "对象")
            {
                getTree[0] = "IC/BO";
                getTree[1] = "R_BO_XF_BO";
                getTree[2] = "C407A7D1-19B5-42C5-AEC2-4C38A8F22717";
            }
            if (comboxvalue == "作者")
            {
                getTree[0] = "IC/AU";
                getTree[1] = "";
                getTree[2] = "ADEBC4C3-EB77-4C82-B006-5BF0E4F69DD3";
            }
            if (comboxvalue == "业务过程")
            {
                getTree[0] = "IC/BP";
                getTree[1] = "R_BP_XF_BP";
                getTree[2] = "B207CE55-8F0A-41ED-9072-7155509E3269";
            }
            if (comboxvalue == "业务特征")
            {
                getTree[0] = "IC/BF";
                getTree[1] = "R_BF_XF_BF";
                getTree[2] = "17B7EB92-115A-4781-8FEC-79305828113D";
            }
            if (comboxvalue == "业务域")
            {
                getTree[0] = "IC/BD";
                getTree[1] = "";
                getTree[2] = "01657AE9-32E6-4A6E-A07A-7B59A9C71828";
            }
            if (comboxvalue == "岗位")
            {
                getTree[0] = "IC/PO";
                getTree[1] = "";
                getTree[2] = "E80347FA-90DF-4F64-A16C-7DB47E01E303";
            }
            if (comboxvalue == "业务对象类型")
            {
                getTree[0] = "IC/BOT";
                getTree[1] = "";
                getTree[2] = "B561E0E6-8CCA-4641-BAD7-FE982F1640CA";
            }
            if (comboxvalue == "专业类别")
            {
                getTree[0] = "IC/PRO";
                getTree[1] = "R_PRO_XF_PRO";
                getTree[2] = "DAB5FB80-1B71-47FC-8A5F-FE9FD682DF1C";
            }
            if (comboxvalue == "成果类型")
            {
                getTree[0] = "IC/PT";
                getTree[1] = "R_BP_CS_PT";
                getTree[2] = "44D80981-6B49-4F36-A5FF-FF1C3DC22764";
            }
            return getTree;
        }
    }
}

using System.Collections.Generic;
using Jurassic.So.Data.Entities;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Jurassic.PKS.Service.Adapter;

namespace Jurassic.So.SpiderTool.IService
{
    public interface IAdapterManageService
    {
        AdapterInfoModelPager GetAdapterInfoPageList(string adapterName, string status, string hangup, int pageIndex, int pageSize);

        List<SpiderScopeModel> GetDataTypeList(string adapterId, string dataType);

        /// <summary>
        /// 挂起适配器
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="userName">操作人员</param>
        void HangupAdapter(string adapterId, string userName);

		int RegisterAdapter(AdapterInfo adapterInfoCom, GT_AdapterInfo ds_AI, List<Scope> adapterDataTypesCom, string userName);

        string UnRegisterAdapter(string adapterId);

        List<SpiderScopeModel> GetDataTypeDuplicateList();

        List<SpiderScopeModel> GetAdapterInfoListFromDataType(string adapterSpiderScope);

        void ExchangeAdapterTypePriority(string spiderScope, string adapterId1, string adapterId2);
    }
}

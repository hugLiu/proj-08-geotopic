using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    /// <summary>
    /// 业务对象维护
    /// </summary>
    public interface IBoManageService
    {
        /// <summary>
        /// 添加业务对象
        /// </summary>
        /// <param name="bo"></param>
        void AddBo(BoBaseInfoModel bo);

        /// <summary>
        /// 更新业务对象
        /// </summary>
        /// <param name="bo"></param>
        void UpdateBo(BoBaseInfoModel bo);

        /// <summary>
        /// 删除业务对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alias"></param>
        void DeleteBo(string id, string alias);

        /// <summary>
        /// 获得指定bot的业务对象根节点并分页
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="pid"></param>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<BoBaseInfoModel> GetBoByBot(string bot,string pid,string key,int pageIndex,int pageSize);

        /// <summary>
        /// 获得指定bot的业务对象并分页
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<BoBaseInfoModel> GetByBot(string botName, string name, int pageIndex, int pageSize);
        /// <summary>
        /// 获得指定bot和pid的业务对象，包括根节点
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        List<BoBaseInfoModel> GetBoByBot(string botName, string pid);

        BoBaseInfoModel GetBoById(string id);

        /// <summary>
        /// 获得指定bot的业务对象根节点的总记录条数
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="key"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        int GetCountByBotAndId(string botName,string key, string pid);

        /// <summary>
        /// 获得指定概念类所有信息的总条数
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        int GetCountByBot(string botName, string key);
    }
}

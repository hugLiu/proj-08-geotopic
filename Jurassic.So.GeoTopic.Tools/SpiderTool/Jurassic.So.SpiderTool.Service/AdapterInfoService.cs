using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.So.Data.Entities;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;

namespace Jurassic.So.SpiderTool.Service
{
    public class AdapterInfoService : IAdapterInfoService
    {
        private readonly DataServiceDBContext _context;

        public AdapterInfoService()
        {
            _context = new DataServiceDBContext();
        }

        public List<AdapterInfoModel> GetAdapterPageInfos(int pageIndex, int pageSize, ref int total)
        {

            var data = _context.GT_AdapterInfo
                .OrderByDescending(t => t.CreatedDate)
                .Select(t => new AdapterInfoModel
                {
                    AdapterId = t.Id.ToString(),
                    AdapterName = t.AdapterName,
                    DataSourceName = t.DataSourceName,
                    DataSourceType = t.DataSourceType,
                    CreateDate = t.CreatedDate.Value,
                    SpiderSize = t.SpiderSize,
                    PlanInfoId = _context.GT_PlanInfo.FirstOrDefault(s => s.AdapterId == t.Id)==null ? "" :  _context.GT_PlanInfo.FirstOrDefault(s => s.AdapterId == t.Id).Id.ToString()
                })
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();
            total = _context.GT_AdapterInfo.Count();
            return data;
        }

        /// <summary>
        /// 获取适配下具体的爬取范围，并实现爬取范围条件过滤
        /// </summary>
        /// <param name="adapterId">适配器ID</param>
        /// <param name="spiderScope">爬取范围</param>
        /// <returns></returns>
        public List<SpiderScopeModel> GetSpiderScopeList(string adapterId, string spiderScope)
        {
            if (string.IsNullOrWhiteSpace(adapterId))
                return null;

            List<GT_SpiderScope> dataTypesList = new List<GT_SpiderScope>();
            if (!string.IsNullOrWhiteSpace(spiderScope))//当传入的数据类型参数不为空值
            {
                spiderScope = spiderScope.Trim();
                dataTypesList =
                    _context.GT_SpiderScope.Where(p => p.AdapterId.ToString() == adapterId && p.SpiderScope.Contains(spiderScope)).ToList();
            }
            else
            {
                dataTypesList = _context.GT_SpiderScope.Where(p => p.AdapterId.ToString() == adapterId).ToList();
            }



            return dataTypesList.Select(entity => new SpiderScopeModel
            {
                AdapterId = entity.AdapterId,
                SpiderScope = entity.SpiderScope,
                IncrementType = entity.IncrementType,
                Priority = entity.Priority ?? 0,
                Description = entity.Desc,
                TaskMode = entity.TaskMode,
                AsTask = Convert.ToInt32(entity.AsTask),
                SpiderCommand = 1,
                ProcessedPercent = 0,
                LastSpiderStatus = entity.LastSpiderStatus,
                ExecuteTimes = entity.ExecuteTimes
            }).ToList();

        }

        public void UpdateSpiderScopeSelectAll(string adapterId)
        {
            var gtSpiderScope = _context.GT_SpiderScope.Where(t => t.AdapterId.ToString() == adapterId);
            foreach (var spiderScope in gtSpiderScope)
            {
                spiderScope.AsTask = true;
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// 更新爬取范围选中状态
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="scopes">爬取范围集合</param>
        public void UpdateSpiderScopeSelected(string adapterId, List<string> scopes)
        {
            var gtSpiderScope = _context.GT_SpiderScope.Where(t => t.AdapterId.ToString() == adapterId);
            foreach (var spiderScope in gtSpiderScope)
            {
                spiderScope.AsTask = scopes.Contains(spiderScope.SpiderScope);
            }
            _context.SaveChanges();
        }

        public void UpdateSpiderSize(string adapterId, int spiderSize)
        {
            var gtAdapterInfo = _context.GT_AdapterInfo
                .FirstOrDefault(t => t.Id.ToString() == adapterId);
            if (gtAdapterInfo != null)
            {
                gtAdapterInfo.SpiderSize = spiderSize;
                _context.SaveChanges();
            }
        }
    }
}



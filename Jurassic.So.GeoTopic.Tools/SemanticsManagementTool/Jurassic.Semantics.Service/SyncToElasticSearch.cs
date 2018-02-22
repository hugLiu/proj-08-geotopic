using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.Sooil.IServiceBase;

namespace Jurassic.Semantics.Service
{
    public class SyncToElasticSearch : ISyncToElasticSearch
    {
        private readonly SqlServerAccess _sqlaAccess = new SqlServerAccess();
        private readonly int _pagesize = Convert.ToInt32(AppConfigHelper.GetValueByKey("pageSize"));
        /// <summary>
        ///  重新索引分类叙词表
        /// </summary>
        /// <returns></returns>
        public string ReIndexGlossary()
        {
            var esIndex = new ElasticSearchAccess<Glossaries>();

            var path = GetDefaultPath("SemanticsModel.json");
            if (path == null) return "path error";
            var type = "glossary";
            var result = string.Empty;
            var total = _sqlaAccess.GetGlossariesCount();
            esIndex.BuildModelBase(type, path);
            //关闭刷新
            esIndex.CloseRefresh();
            for (int i = 1; i <= total / _pagesize + 1; i++)
            {
                var info = new PagerInfo(i, _pagesize);
                var allDocs = _sqlaAccess.GetGlossaries(info);
                result = esIndex.Index(allDocs.Select(AutoMapper.Mapper.Map<Glossaries>).ToList(), type);
            }
            //开启刷新
            esIndex.OpenRefresh();
            //运行期间优化
            esIndex.Optimize(type);
            return result;
        }

        /// <summary>
        /// 重新索引PT上下文
        /// </summary>
        /// <returns></returns>
        public string ReIndexPtContext()
        {
            var esIndex = new ElasticSearchAccess<ESPTContext>();

            var path = GetDefaultPath("PTContext.json");
            if (path == null) return "path error";
            var type = "ptcontext";
            var result = string.Empty;
            var total = _sqlaAccess.GetPtContextCount();
            esIndex.BuildModelBase(type, path);
            //关闭刷新
            esIndex.CloseRefresh();
            for (int i = 1; i <= total / _pagesize + 1; i++)
            {
                var info = new PagerInfo(i, _pagesize);
                var allDocs = _sqlaAccess.GetPtContext(info);
                result = esIndex.Index(allDocs.Select(AutoMapper.Mapper.Map<ESPTContext>).ToList(), type);
            }
            //开启刷新
            esIndex.OpenRefresh();
            //运行期间优化
            esIndex.Optimize(type);
            return result;
        }

        /// <summary>
        /// 数据模型持久化
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>返回默认的文件位置路径</returns>
        private static string GetDefaultPath(string fileName)
        {
            var dll = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "");
            var bin = Path.GetDirectoryName(dll);
            return bin != null ? Path.Combine(bin, fileName) : null;
        }
    }
}

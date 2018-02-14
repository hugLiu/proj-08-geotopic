using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.Adapter;
using Jurassic.ServiceModels;

namespace Jurassic.Adapter
{
    /// <summary>适配器包装</summary>
    public class AdapterWrapper : AdapterBase
    {
        /// <summary>构造函数</summary>
        public AdapterWrapper() { }
        /// <summary>构造函数</summary>
        public AdapterWrapper(IAdapter adapter)
        {
            this.Instance = adapter;
        }
        /// <summary>包装实例</summary>
        protected IAdapter Instance { get; set; }
        /// <summary>适配器信息</summary>
        public override AdapterInfo GetAdapterInfo()
        {
            return this.Instance.GetAdapterInfo();
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="incrementValue">增量值，允许为空，如果有值必须是符合该域的增量类型的值</param>
        /// <param name="pager">分页参数，允许为null，为null表示爬取全部，否则只爬取某页数据</param>
        /// <returns>爬取结果</returns>
        public override SpiderResult Spider(string scope, string incrementValue, Pager pager)
        {
            return this.Instance.Spider(scope, incrementValue, pager);
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="incrementValue">增量值，允许为空，如果有值必须是符合该域的增量类型的值</param>
        /// <param name="pager">分页参数，允许为null，为null表示爬取全部，否则只爬取某页数据</param>
        /// <returns>爬取结果</returns>
        public override async Task<SpiderResult> SpiderAsync(string scope, string incrementValue, Pager pager)
        {
            return await this.Instance.SpiderAsync(scope, incrementValue, pager);
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="natureKey">成果键</param>
        /// <returns>成果的内容项集合</returns>
        public override DataSchemaCollection Retrieve(string scope, string natureKey)
        {
            return this.Instance.Retrieve(scope, natureKey);
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="natureKey">成果键</param>
        /// <returns>成果的内容项集合</returns>
        public override async Task<DataSchemaCollection> RetrieveAsync(string scope, string natureKey)
        {
            return await this.Instance.RetrieveAsync(scope, natureKey);
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        /// <param name="ticket">成果的数据项票据</param>
        /// <param name="pager">分页参数</param>
        /// <returns>成果的数据项结果</returns>
        public override DataResult GetData(string ticket, Pager pager)
        {
            return this.Instance.GetData(ticket, pager);
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        /// <param name="ticket">成果的数据项票据</param>
        /// <param name="pager">分页参数</param>
        /// <returns>成果的数据项结果</returns>
        public override async Task<DataResult> GetDataAsync(string ticket, Pager pager)
        {
            return await this.Instance.GetDataAsync(ticket, pager);
        }
    }
}
using Jurassic.So.Business;
using Jurassic.So.Data.Entities;
using Jurassic.So.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using Jurassic.So.Index;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service;
using Jurassic.So.Adapter;

namespace Jurassic.So.Data
{
    /// <summary>适配器模拟</summary>
    public class MockAdapter : AdapterWrapper
    {
        /// <summary>构造函数</summary>
        public MockAdapter(IServiceMockConfig config)
        {
            this.Config = config;
        }
        /// <summary>配置</summary>
        private IServiceMockConfig Config { get; set; }
        /// <summary>载入适配器</summary>
        public void Load(GT_AdapterInfo adapterInfo)
        {
            if (adapterInfo.InvokeType == 0)
            {
                //var configFile = this.Config.RootPath + adapterInfo.AdapterURL.TrimStart('\\') + AdapterConsts.ConfigFile;
                //this.Instance = new Adapter(configFile);
                this.Instance = new SQLAdapter();
            }
            else
            {
                this.Instance = (AdapterBase)CreateRemoteAdapterInstance(adapterInfo.AdapterURL);
            }
        }
        /// <summary>创建远程适配器</summary>
        private IAdapter CreateRemoteAdapterInstance(string wcfUrl)
        {
            EndpointAddress address = new EndpointAddress(wcfUrl);
            BasicHttpBinding ws = new BasicHttpBinding();
            ws.MaxBufferSize = 2147483647;
            ws.MaxBufferPoolSize = 2147483647;
            ws.MaxReceivedMessageSize = 2147483647;
            ws.ReaderQuotas.MaxStringContentLength = 2147483647;
            ws.CloseTimeout = new TimeSpan(0, 30, 0);
            ws.OpenTimeout = new TimeSpan(0, 30, 0);
            ws.ReceiveTimeout = new TimeSpan(0, 30, 0);
            ws.SendTimeout = new TimeSpan(0, 30, 0);
            ws.Security.Mode = BasicHttpSecurityMode.None;
            ChannelFactory<IAdapter> factory = new ChannelFactory<IAdapter>(ws, address);
            IAdapter client = factory.CreateChannel();
            return client;
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        public override DataSchemaCollection Retrieve(string scope, string natureKey)
        {
            return RetrieveAsync(scope, natureKey).GetAwaiter().GetResult();
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        public override Task<DataSchemaCollection> RetrieveAsync(string scope, string natureKey)
        {
            var result = new DataSchemaCollection();
            //if (adpUrl.DataType == "Test")
            {
                var formats = natureKey.Split('_');
                for (var i = 0; i < formats.Length; i++)
                {
                    var format = formats[i].ToUpper();
                    var index = i.ToString();
                    var item = new DataSchema();
                    item.Name = $"格式{format}模拟数据{index}";
                    item.Ticket = $"{natureKey}_凭证{index}";
                    item.Major = (i == 0);
                    var format2 = char.IsNumber(format.Last()) ? format.Substring(0, format.Length - 1) : format;
                    item.Format = format2.ToDataFormat();
                    item.Total = 1;
                    item.Unit = "";
                    result.Add(item);
                }
            }
            return Task.FromResult(result);
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public override DataResult GetData(string ticket, Pager pager)
        {
            return GetDataAsync(ticket, pager).GetAwaiter().GetResult();
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public override Task<DataResult> GetDataAsync(string ticket, Pager pager)
        {
            //if (adpUrl.DataType == "Test")
            {
                var tindex = ticket.LastIndexOf("_凭证");
                var index = ticket.Substring(tindex + 3);
                var i = int.Parse(index);
                var formats = ticket.Substring(0, tindex).Split('_');
                var format = formats[i];
                var isUrlFlag = format.Last();
                var format2 = char.IsNumber(isUrlFlag) ? format.Substring(0, format.Length - 1) : format;
                var isFirst = true;
                if (i > 0 && formats.Take(i).Any(e => e.StartsWith(format2, StringComparison.OrdinalIgnoreCase)))
                {
                    isFirst = false;
                }
                var format3 = format2;
                if (format3.Equals("DataSet", StringComparison.OrdinalIgnoreCase))
                {
                    format3 = "DataTable";
                }
                var fileName = isFirst ? $"Mock.{format3}" : $"Mock1.{format3}";
                var isUrl = isUrlFlag == '1';
                var result = new DataResult();
                if (format2.Equals("DataTable", StringComparison.OrdinalIgnoreCase))
                {
                    format2 = "DataSet";
                }
                result.Format = format2.ToDataFormat();
                if (isUrl)
                {
                    result.Format = DataFormat.URL;
                    var host = HttpContext.Current == null ? "http://localhost:1668/" : HttpContext.Current.Request.Url.GetDomainUrl();
                    //var data = new UrlDataResult();
                    //data.HttpVerb = HttpMethod.Get.Method;
                    //data.Url = (host + this.Config.DataDir + "/" + fileName);
                    //result.Value = data;
                    result.Value = (host + this.Config.DataDir + "/" + fileName);
                }
                else
                {
                    var attribute = result.Format.ToMimeType();
                    if (attribute.IsStreamOutput)
                    {
                        result.Value = File.ReadAllBytes(this.Config.DataPath + fileName);
                    }
                    else
                    {
                        result.Value = File.ReadAllText(this.Config.DataPath + fileName);
                    }
                }
                return Task.FromResult(result);
            }
            //return base.GetData(adpUrl, ticket, pager);
        }
    }
}
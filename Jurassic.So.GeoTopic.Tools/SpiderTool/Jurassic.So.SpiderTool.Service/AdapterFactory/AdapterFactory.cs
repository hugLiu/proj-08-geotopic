using Jurassic.PKS.Service.Adapter;
using Jurassic.So.Data.Entities;
using System;
using System.ServiceModel;

namespace Jurassic.So.SpiderTool.Service
{
    public static class AdapterFactory
    {
        /// <summary>
        /// 适配器生成
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static IAdapter CreateAdapter(GT_AdapterInfo info)
        {
            switch (info.InvokeType)
            {
                case InvokeType.WebApiInvoke:
                    return CreateInstanceFromWebApi(info.AdapterURL);
                case InvokeType.WcfInvoke:
                    return CreateInstanceFromWcf(info.AdapterURL);
                default:
                    return null;
            }
        }

        private static IAdapter CreateInstanceFromWcf(string wcfUrl)
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

        private static IAdapter CreateInstanceFromWebApi(string apiUrl)
        {
            IAdapter client = new WrapedAdapterService(apiUrl);
            return client;
        }
    }
}

using Jurassic.So.SpiderTool.IService.Processers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;

namespace Jurassic.So.SpiderTool.Service.Processer
{
    /// <summary>
    /// 基于SingalR的处理器工厂
    /// </summary>
    public class SignalRProcesserFactory : ProcesserFactory
    {
        private static readonly Lazy<SignalRProcesserFactory> _instance = new Lazy<SignalRProcesserFactory>(() => new SignalRProcesserFactory(GlobalHost.ConnectionManager.GetHubContext<ProcesserHub>().Clients));

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        /// <summary>
        /// 基于SignalR处理器工厂的单例
        /// </summary>
        public static SignalRProcesserFactory Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private SignalRProcesserFactory(IHubConnectionContext clients)
        {
            Clients = clients;
        }
        protected override void AfterRegister(ProcesserBase processer)
        {
            processer.ProgressChanged +=processer_ProgressChanged;
        }

        void processer_ProgressChanged(object sender, EventArgs e)
        {
            Clients.All.reportProgress(sender);
        }
    }
}
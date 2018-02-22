using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using Jurassic.So.SpiderTool.IService.Processers;

namespace Jurassic.So.SpiderTool.Service.Processer
{
    public class ProcesserHub : Hub
    {
        public IList<ProcesserBase> GetAllProcessers()
        {
            return SignalRProcesserFactory.Instance.GetAllProcessers();   
        }
    }
}
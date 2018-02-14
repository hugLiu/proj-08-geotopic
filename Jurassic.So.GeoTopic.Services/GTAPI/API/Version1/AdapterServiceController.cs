using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jurassic.PKS.Service.Adapter;

namespace GTAPI.API.Version1
{
    public class AdapterServiceController : AdapterServiceBaseController
    {
        public AdapterServiceController(IAdapter adapterService) : base(adapterService)
        {
        }
    }
}

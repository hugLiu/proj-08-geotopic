using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.PKS.Service.GF;
using System.Collections.Generic;
using System;
using Jurassic.PKS.Service;
using Jurassic.PKS.WebAPI.GF;
using Jurassic.PKS.WebAPI.Models;
using Newtonsoft.Json;
using Jurassic.So.Infrastructure;

namespace GTAPI.API.Version1
{
    /// <summary>
    /// 3GX服务API
    /// </summary>
    public class GGGXServiceController : WebAPIController
    {
        private IGGGX ThirdGXService { get; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        public GGGXServiceController(IGGGX gggx)
        {
            this.ThirdGXService = gggx;
        }
        /// <summary>
        /// 根据过滤条件获取3GX属性信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns><c>XmlDocument</c>的json字符串，符合3GX数据规范</returns>
        [HttpPost]
        public IHttpActionResult GetFeatures(FeatureFilter filter)
        {
            var result = ThirdGXService.GetFeatures(filter);
            return new ApiStreamResult(MimeTypeConst._3GX, result.ToStream(), this);
        }
        /// <summary>
        /// 获取业务对象属性枚举值
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="parameter">NS.参数名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> GetDomain(string ft, string parameter)
        {
            return ThirdGXService.GetDomain(ft, parameter);
        }
        /// <summary>
        /// 根据条件获取3GX数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns><c>XmlDocument</c>的json字符串，符合3GX数据规范</returns>
        [HttpGet]
        public async Task<string> GetFeatureById(string fid, string crs)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取指定特征类型所包含的业务对象集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BOCollection> GetBOsByFT(string ft)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取指定业务对象类型所包含的业务对象集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> GetBOsByBOT(string bot)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取指定BO的特征类型集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> GetFTsByBO(string bot, string bo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// for test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<FTCCollection> GetFTCList()
        {
            return await Task.FromResult(ThirdGXService.GetFTCList());
        }
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "3GX服务"
            };
        }
        /// <summary>获得服务能力数据</summary>
        protected override async Task<ServiceCapabilities> GetServiceCapabilities()
        {
            var ftcs = ThirdGXService.GetFTCList();
            var bots = ThirdGXService.GetAllBOT();
            return await Task.FromResult(new GGGXServiceCapabilities { FTCs = ftcs, BOTs = bots });
        }
    }
}

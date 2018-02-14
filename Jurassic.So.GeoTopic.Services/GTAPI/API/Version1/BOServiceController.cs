using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.GF;
using Jurassic.PKS.Service.Interfaces.GF;
using Jurassic.PKS.WebAPI.GF;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.WebAPI.Models;

namespace GTAPI.API.Version1
{
    /// <summary>
    /// 对象服务API
    /// </summary>
    public class BOServiceController : WebAPIController
    {
        private IBO BOService { get; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        public BOServiceController(IBO bo)
        {
            this.BOService = bo;
        }
        /// <summary>
        /// 根据业务对象正式名或别名以及业务对象类型获取正式业务实体信息
        /// </summary>
        /// <param name="name">业务对象正式名或别名</param>
        /// <param name="bot">业务对象类型</param>
        /// <returns>如果找到，返回<c>BO</c>对象，否则返回<c>null</c></returns>
        [HttpGet]
        public async Task<BO> GetBOByName(string name, string bot)
        {
            return BOService.GetBOByName(name, bot);
        }

        /// <summary>
        /// 根据业务对象Id获取正式业务实体信息
        /// </summary>
        /// <param name="boid">业务对象Id</param>
        /// <returns>如果找到，返回<c>BO</c>对象，否则返回<c>null</c></returns>
        [HttpGet]
        public async Task<BO> GetBOById(string boid)
        {
            return BOService.GetBOById(boid);
        }

        /// <summary>
        /// 根据业务对象别名和应用域获取正式业务对象信息
        /// </summary>
        /// <param name="alias">业务对象别名</param>
        /// <param name="appdomain">业务域</param>
        /// <returns>如果找到，返回<c>BO</c>对象，否则返回<c>null</c></returns>
        [HttpGet]
        public async Task<BO> GetBOByAlias(string alias, string appdomain)
        {
            return BOService.GetBOByAlias(alias, appdomain);
        }

        /// <summary>
        /// 根据业务对象ID获取业务对象别名
        /// </summary>
        /// <param name="boid">业务对象ID</param>
        /// <param name="appdomains">应用域列表。如果为<c>null</c>，返回所有可能的应用域别名</param>
        /// <returns>别名列表，<c>Alias</c>集合</returns>
        [HttpPost]
        public async Task<AliasCollection> GetBOAliasByID(GetBOAliasRequest request)
        {
            var boid = request.BOID;
            var appdomain = request.AppDomain.ToArray();
            return BOService.GetBOAliasByID(boid, appdomain);
        }

        /// <summary>
        /// 根据业务对象ID和数据分类获取3GX数据
        /// </summary>
        /// <param name="boid">业务对象ID</param>
        /// <param name="category"><c>GGGXDataCategory</c>对象，要返回的数据类别</param>
        /// <returns><c>XmlDocument</c>的一个实例，该实例符合3GX数据规范</returns>
        [HttpGet]
        public IHttpActionResult Get3GXById(string boid, GGGXDataCategory category)
        {
            var result = BOService.Get3GXById(boid, category);
            return new ApiStreamResult(MimeTypeConst._3GX, result.ToStream(), this);
        }

        /// <summary>
        /// 获取指定对象附近的临近对象
        /// </summary>
        /// <param name="boid">指定业务对象ID</param>
        /// <param name="distince">临近半径范围，单位为米</param>
        /// <param name="bot">临近业务对象类型</param>
        /// <param name="filter">符合filter规范的json字符串</param>
        /// <returns><c>NearBOCollection</c>实例</returns>
        [HttpPost]
        public async Task<NearBOCollection> GetNearBOById(GetNearBOByIdRequest request)
        {
            var boid = request.BOID;
            var distance = request.Distance;
            var bot = request.BOT;
            var filter = FilterToJson(request.Filter);
            return BOService.GetNearBOById(boid, distance, bot, filter);
        }

        /// <summary>
        /// 获取指定点坐标附近的临近对象
        /// </summary>
        /// <param name="pointWKT">指定符合Point定义的WKT格式坐标</param>
        /// <param name="distince">临近半径范围，单位为米</param>
        /// <param name="bot">临近业务对象类型</param>
        /// <param name="filter">符合filter规范的json字符串</param>
        /// <returns><c>NearBOCollection</c>实例</returns>
        [HttpPost]
        public async Task<NearBOCollection> GetNearBOByLocation(GetNearBOByLocationRequest request)
        {
            var spaceLoaction = request.SpaceLocation;
            var distance = request.Distance;
            var bot = request.BOT;
            var filter = FilterToJson(request.Filter);
            return BOService.GetNearBOByLocation(spaceLoaction, distance, bot, filter);
        }

        /// <summary>
        /// 获取指定业务对象名称/类型附近的临近对象
        /// </summary>
        /// <param name="boName">指定业务对象名称（包括正式名和别名）</param>
        /// <param name="boType">指定业务对象类型</param>
        /// <param name="bot">临近业务对象类型</param>
        /// <param name="filter">符合filter规范的json字符串</param>
        /// <returns><c>NearBOCollection</c>实例</returns>
        [HttpPost]
        public async Task<NearBOCollection> GetNearBOByName(GetNearBOByNameRequest request)
        {
            var boName = request.BOName;
            var boType = request.BOType;
            var distance = request.Distance;
            var bot = request.BOT;
            var filter = FilterToJson(request.Filter);
            return BOService.GetNearBOByName(boName, boType, distance, bot, filter);
        }

        /// <summary>
        /// 获取指定业务对象类型/类别的业务对象
        /// </summary>
        /// <param name="bot">返回的业务对象类型</param>
        /// <param name="filter">符合filter规范的json字符串</param>
        /// <returns>业务对象集合<c>BOCollection</c></returns>
        [HttpPost]
        public async Task<BOCollection> GetBOListByBOT(GetBOListByBOTRequest request)
        {
            var bot = request.BOT;
            var filter = FilterToJson(request.Filter);
            return BOService.GetBOListByType(bot, filter);
        }

        /// <summary>
        /// 获取指定业务对象类型和符合属性过滤条件的业务对象
        /// </summary>
        /// <param name="bot">返回的业务对象类型</param>
        /// <param name="wktBBox">符合WKT的格式空间范围</param>
        /// <param name="filter">符合filter规范的json字符串</param>
        /// <returns>业务对象集合<c>BOCollection</c></returns>
        [HttpPost]
        public async Task<BOCollection> GetBOListByFilter(GetBOListByFilterRequest request)
        {
            var bot = request.BOT;
            var bbox = request.BBox;
            var filter = FilterToJson(request.Filter);
            return BOService.GetBOListByFilter(bot, bbox, filter);
        }

        /// <summary>
        /// 根据业务对象树模板获取业务对象列表
        /// </summary>
        /// <param name="template">树模板<c>BOTreeTemplate</c></param>
        /// <returns>业务对象集合<c>BOCollection</c></returns>
        [HttpPost]
        public async Task<TreeBOCollection> GetBOTree(BOTreeTemplate template)
        {
            return BOService.GetBOTree(template);
        }
        [HttpGet]
        public async Task<TermBOCollection> GetCCTermOfBO()
        {
            return BOService.GetCCTermOfBO();
        }

        /// <summary>
        /// 根据业务对象类型和应用域获取属性定义信息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns>返回属性定义集合</returns>
        [HttpPost]
        public async Task<List<PropertySchema>> GetPropertySchema(GetPropertySchemaRequest request)
        {
            var bot = request.BOT;
            var appdomain = request.AppDomain;
            var names = request.Names;
            return BOService.GetPropertySchema(bot, appdomain, names);
        }

        /// <summary>
        /// 根据filter获得3GX数据
        /// </summary>
        /// <param name="bot">对象类型</param>
        /// <param name="bos">对象列表</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Get3GXByFilter(Get3GxByFilterRequest request)
        {
            var result = BOService.Get3GXByFilter(request.BOT, request.BOs, FilterToJson(request.Filter), request.Category);
            return new ApiStreamResult(MimeTypeConst._3GX, result.ToStream(), this);
        }

        private string FilterToJson(object objFilter)
        {
            var filter = string.Empty;
            if (objFilter != null && objFilter.ToJson() != "{}")
                filter = objFilter.ToJson();
            return filter;
        }

        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            var dbInfo = BOService.GetDbInfo();
            return new BOServiceInfo()
            {
                CRS = dbInfo.CRS,
                CSParam = dbInfo.CSParam,
                DBSerName = dbInfo.DBSerName,
                Description = "对象服务"
            };
        }
        /// <summary>获得服务能力数据</summary>
        protected override async Task<ServiceCapabilities> GetServiceCapabilities()
        {
            var appdomains = BOService.GetAppDomains();
            return await Task.FromResult(new BOServiceCapabilities { AppDomains = appdomains });
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.PKS.Service.Semantics;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.WebAPI.Semantics;
using Jurassic.PKS.Service;
using Jurassic.So.Semantics.Entities;
using PanGu;
using System;
using System.Linq;

namespace GTAPI.API.Version1
{
    /// <summary>语义服务API</summary>
    public class SemanticsServiceController : WebAPIController
    {
        /// <summary>语义服务</summary>
        private ISemantics SemanticsService { get; }

        /// <summary>构造函数</summary>
        public SemanticsServiceController(ISemantics semanticsService)
        {
            this.SemanticsService = semanticsService;
        }
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo
            {
                Description = "语义服务主要用于叙词分词、获得同义词，翻译词，概念类的树层次结构等"
            };
        }
        /// <summary>简单语义分词</summary>
        [HttpPost]
        public List<KeyWord> Segment(SegmentRequest request)
        {
            return this.SemanticsService.Segment(request.Sentence, request.Option);
        }
        /// <summary>根据叙词名称获得指定概念类的叙词信息</summary>
        [HttpPost]
        public async Task<List<TermInfo>> GetTermInfo(GetTermInfoRequest request)
        {
            return await this.SemanticsService.GetTermInfo(request.Term, request.Cc);
        }

        [HttpGet]
        public async Task<List<string>> GetTranslationById([FromUri]GetTranslationByIDRequest request)
        {
            return await this.SemanticsService.GetTranslationById(request.Id, request.LangCode, request.OnlyMain);
        }

        /// <summary>根据叙词id返回对应的子树的层级结构，并限定返回叙词的概念类</summary>
        [HttpGet]
        public async Task<TreeResult> Hierarchy([FromUri]HierarchyRequest request)
        {
            if (request == null)
            {
                return null;
            }
            return await this.SemanticsService.Hierarchy(request.Id, request.Cc, request.DeepLevel);
        }

        /// <summary>获取所有的概念类</summary>
        [HttpGet]
        public async Task<List<ConceptClass>> GetCC()
        {
            return await this.SemanticsService.GetCC();
        }

        /// <summary>获取所有语义关系类型</summary>
        [HttpGet]
        public async Task<List<SemanticsType>> GetSemanticsType()
        {
            return await this.SemanticsService.GetSemanticsType();
        }

        /// <summary>获取语义关系</summary>
        [HttpGet]
        public async Task<List<TermInfo>> Semantics([FromUri]SemanticsRequest request)
        {
            return await this.SemanticsService.Semantics(request.Term, request.SR, request.Direction);
        }

        /// <summary>获得指定类型词库</summary>
        [HttpPost]
        public async Task<List<string>> GetDictionary(GetDictionaryRequest request)
        {
            List<WordResult> result;
            if (request == null) result = await this.SemanticsService.GetDictionary(null);
            else result = await this.SemanticsService.GetDictionary(request.Cc);
            return result.Select(t => t.Term).ToList();
        }

        /// <summary>获取盘古分词词库</summary>
        public async Task<List<WordAttribute>> GetDic4PanGu()
        {
            var result = await this.SemanticsService.GetDictionary(null);
            var waList = new List<WordAttribute>();
            foreach (var item in result)
            {
                waList.Add(new WordAttribute { Word = item.Term, Pos = (POS)Enum.Parse(typeof(POS), item.Cc) });
            }
            return waList;
        }
    }
}
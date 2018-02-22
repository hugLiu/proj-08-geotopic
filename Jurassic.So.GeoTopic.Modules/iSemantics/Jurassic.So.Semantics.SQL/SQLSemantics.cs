using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.PKS.Service.Semantics;
using Jurassic.So.Infrastructure;
using Jurassic.So.Semantics.Entities;
using System.Globalization;
using PanGu;
using PanGu.Match;
using Jurassic.So.Semantics.SQL.Util;

namespace Jurassic.So.Semantics.SQL
{
    public class SQLSemantics : ISemantics
    {
        private readonly SemanticsSQLProvider _sqlProvider;
        private readonly PanGuSegmantProvider _segmentProvider;
        public SQLSemantics()
        {
            _sqlProvider = new SemanticsSQLProvider();
            _segmentProvider = new PanGuSegmantProvider();
        }

        public List<KeyWord> Segment(string sentence, SegmentOption option)
        {
            var dict = SingletonDict.Instance;
            List<WordInfo> words;
            if (option.MatchRule == "MinWords")
            {
                var parameter = new MatchParameter {Redundancy = 2};
                words = _segmentProvider.DoSegment(sentence, null, parameter);               
            }
            else
            {
                words = _segmentProvider.DoSegment(sentence);
            }

            words = words.Distinct(new WordAttributeComparer()).ToList();

            var keywords = new List<KeyWord>();
            if (option.Cc?.Count > 0)
            {
                keywords.AddRange(from item in words
                                  let cc = item.Pos.ToString().Split(new[] { ", " }, StringSplitOptions.None).ToList()
                                  where option.Cc.Intersect(cc).Any()
                                  select new KeyWord
                                  {
                                      Term = item.Word,
                                      Cc = option.Cc.Intersect(cc).ToList()
                                  });
            }
            else
            {
                keywords.AddRange(words.Select(item => new KeyWord
                {
                    Term = item.Word,
                    Cc = item.Pos.ToString().Split('|').ToList()
                }));
            }

            foreach (var keyword in keywords)
            {
                //包含同义词
                if (option.IncludeAlias)
                    if (dict.AliasDict.ContainsKey(keyword.Term))
                        keyword.Aliases = dict.AliasDict[keyword.Term];
                //包含翻译词
                if (option.IncludeTrans)
                    if (dict.TransDict.ContainsKey(keyword.Term))
                        keyword.Translates = dict.TransDict[keyword.Term];
            }
            return keywords;
        }

        public async Task<List<TermInfo>> GetTermInfo(List<string> terms, string cc)
        {
            if (terms == null || terms.Count == 0) throw new ArgumentNullException(nameof(terms));
            if (cc == null) throw new ArgumentNullException(nameof(cc));
            var result = new List<TermInfo>();
            foreach (var term in terms)
            {
                var dalResult = await _sqlProvider.GetTermInfo(term, cc);
                result.AddRange(dalResult.Select(t => t.MapTo<TermInfo>()).ToList());
            }
            return result;
        }

        public async Task<List<string>> GetTranslationById(string id, string langcode, bool onlymain)
        {
            var guid = Guid.Empty;
            if (!id.IsNullOrEmpty())
                guid = id.ToGuid();
            return await _sqlProvider.GetTranslationById(guid, langcode, onlymain);
        }

        public async Task<TreeResult> Hierarchy(string id, string cc, int deeplevel)
        {
            var guid = Guid.Empty;
            //如果id为空，返回该概念类的整棵树
            if (!id.IsNullOrEmpty())
                guid = id.ToGuid();
            if (cc == null) throw new ArgumentNullException(nameof(cc));
            //修改，使支持扩展
            var relation = "R_" + cc + "_XF_" + cc;
            return await _sqlProvider.GetWholeTree(guid, null, cc, relation, deeplevel);
        }

        public async Task<List<ConceptClass>> GetCC()
        {
            var result = await _sqlProvider.GetCC();
            return result.Select(t => t.MapTo<ConceptClass>()).ToList();
        }

        public async Task<List<SemanticsType>> GetSemanticsType()
        {
            var result = await _sqlProvider.GetSemanticsType();
            return result.Select(t => t.MapTo<SemanticsType>()).ToList();
        }

      
        public async Task<List<TermInfo>> Semantics(string term, string sr, string direction)
        {
            term = await _sqlProvider.Formal(term);
            sr = sr.ToString(CultureInfo.InvariantCulture);
            List<SD_CCTerm> result;
            switch (direction)
            {
                case "forward": result = await _sqlProvider.GetSemantics(term, sr); break;
                case "backward": result = await _sqlProvider.GetReverseSemantics(term, sr); break;
                default: throw new ArgumentException(@"direction");
            }
            return result.Select(t => t.MapTo<TermInfo>()).ToList();
        }

        public async Task<List<WordResult>> GetDictionary(List<string> cc)
        {
            if (cc == null || cc.Count == 0)
            {
                return await _sqlProvider.GetWholeDict();
            }
            return await _sqlProvider.GetDictByCc(cc);
        }
    }
}
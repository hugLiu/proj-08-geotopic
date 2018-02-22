using PanGu;
using PanGu.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Semantics.SQL
{
    /// <summary>
    /// 盘古分词
    /// </summary>
    public class PanGuSegmantProvider
    {
        private readonly Segment _segment;
        public PanGuSegmantProvider()
        {
            _segment = new Segment();
        }

        public List<WordInfo> DoSegment(string sentence)
        {
            return _segment.DoSegment(sentence).ToList();
            
        }

        public List<WordInfo> DoSegment(string sentence, MatchOptions options)
        {
            return _segment.DoSegment(sentence, options).ToList();
        }

        public List<WordInfo> DoSegment(string sentence, MatchOptions options, MatchParameter parameters)
        {
            return _segment.DoSegment(sentence, options, parameters).ToList();
        }
    }
}

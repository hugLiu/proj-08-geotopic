using Jurassic.So.Data.Entities;

namespace Jurassic.So.SpiderTool.Service.Processer
{
    public class SpiderParam
    {
        public SpiderParam(string userName, GT_SpiderScope spiderScope)
        {
            this.User = userName;
            this.GtSpiderScope = spiderScope;
        }
        public string User { get; set; }

        public GT_SpiderScope GtSpiderScope { get; set; }
    }
}

using System.Threading.Tasks;

namespace Jurassic.Semantics.IService
{
    public interface ISyncToElasticSearch
    {
        /// <summary>
        /// 重新索引分类叙词表
        /// </summary>
        /// <returns></returns>
        string ReIndexGlossary();
        /// <summary>
        /// 重新索引PT上下文
        /// </summary>
        /// <returns></returns>
        string ReIndexPtContext();


    }
}

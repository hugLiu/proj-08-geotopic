
namespace Jurassic.Semantics.IService.ViewModel
{
    public class TreeModel : ModelBase
    {
        /// <summary>
        /// 父id
        /// </summary>
        public string pid { get; set; }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="_id">叙词id</param>
        /// <param name="_text">叙词内容</param>
        /// <param name="_pid">叙词父id</param>
        public TreeModel(string _id, string _text, string _pid)
        {
            id = _id;
            text = _text;
            pid = _pid;
        }
    }
    /// <summary>
    /// 基础模型
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 文本展示
        /// </summary>
        public string text { get; set; }
    }
}

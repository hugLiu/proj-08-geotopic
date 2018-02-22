
namespace Jurassic.Sooil.IServiceBase
{
    /// <summary>
    /// 输入参数，指定字段及其权重
    /// </summary>
    public class SFieldWeight
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 权重值
        /// </summary>
        public double Weight { get; set; }
    }
}

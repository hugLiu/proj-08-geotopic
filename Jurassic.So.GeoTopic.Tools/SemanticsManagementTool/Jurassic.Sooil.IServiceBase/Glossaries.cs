
namespace Jurassic.Sooil.IServiceBase
{
    /// <summary>
    /// ES中分类叙词表的模型
    /// </summary>
    public class Glossaries
    {
        public Glossaries()
        {
            KeyWord= string.Empty;
        }
        public string GId { get; set; }
        public string CC { get; set; }
        public string TermSequence { get; set; }
        public string KeyWord { get; set; }

    }
}


namespace Jurassic.Sooil.IServiceBase
{
    public class BO2 : TypeValue
    {
        public BO2()
        {
        }
        public BO2(string type, string value)
            : base(type, value)
        {
        }
        public BO2(string type, string clazz, string value)
            : base(type, value)
        {
            this.Class = clazz;
        }
        public string Class { get; set; }
    }
}

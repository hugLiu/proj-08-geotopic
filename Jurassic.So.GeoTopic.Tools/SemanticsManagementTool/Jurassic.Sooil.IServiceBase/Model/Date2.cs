using System;
using System.Globalization;

namespace Jurassic.Sooil.IServiceBase
{
    public class Date2
    {
        public Date2()
        {
        }
        private string value;
        private string type;
        private string operation;
        public Date2(DateTime value, DateOperator operation)
        {
            this.Value = value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Operation = operation;
        }

        public Date2(string value, DateOperator operation)
        {
            this.Value = Convert.ToDateTime(value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Operation = operation;
        }
        public Date2(string type, string value, DateOperator operation)
        {
            this.Type = type;
            this.Value = Convert.ToDateTime(value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            this.Operation = operation;
        }
        //用来标注日期的类型，比如 创建的日期，修改的日期等
        public string Type { get; set; }
        public DateOperator Operation { get; set; }
        public string Value
        {
            get { return value; }
            set { this.value = Convert.ToDateTime(value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); }
        }

    }
}

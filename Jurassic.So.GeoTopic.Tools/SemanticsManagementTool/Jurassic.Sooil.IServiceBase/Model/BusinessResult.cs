using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class BusinessResult<T>
    {
        // Fields
        private static readonly BusinessResult<T> _success = new BusinessResult<T>(true);

        // Methods
        public BusinessResult(params string[] errors)
            : this((IEnumerable<string>)errors)
        {
        }

        private BusinessResult(bool success)
        {
            this.Succeeded = success;
            this.Errors = new string[0];
        }

        public BusinessResult(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                errors = new string[] { "未知错误" };
            }
            this.Succeeded = false;
            this.Errors = errors;
        }

        public static BusinessResult<T> Failed(params string[] errors)
        {
            return new BusinessResult<T>(errors);
        }

        // Properties
        public IEnumerable<string> Errors { get; private set; }

        public bool Succeeded { get; private set; }

        public static BusinessResult<T> Success
        {
            get
            {
                return _success;
            }
        }

        public T Data { get; set; }
    }
}

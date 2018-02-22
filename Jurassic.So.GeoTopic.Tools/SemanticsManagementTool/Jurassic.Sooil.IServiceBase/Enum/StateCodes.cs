using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public enum StateCodes
    {
        Success = 0,
        ArgumentError = 1,
        Unknown = -1,
        ValidateError = -2,
        Unauthorized = 401
    }
}

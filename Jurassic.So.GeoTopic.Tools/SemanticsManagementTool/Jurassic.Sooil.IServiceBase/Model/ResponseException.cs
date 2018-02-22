using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class ResponseException : Exception
    {
        public ResponseException(string message, string debugMessage)
            : base(message)
        {
            DebugMessage = debugMessage;
            StateCode = StateCodes.ValidateError;
        }

        public string DebugMessage { get; set; }

        public StateCodes StateCode { get; private set; }
    }
}

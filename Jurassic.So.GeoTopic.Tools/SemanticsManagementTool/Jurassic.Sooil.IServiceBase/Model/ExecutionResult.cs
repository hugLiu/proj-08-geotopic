using System.Collections.Generic;
using Jurassic.Sooil.IServiceBase.Properties;

namespace Jurassic.Sooil.IServiceBase
{

    public class ExecutionResult
    {
        private static readonly ExecutionResult _success = new ExecutionResult(true);

        public ExecutionResult(params string[] errors)
            : this((IEnumerable<string>)errors)
        {
        }

        private ExecutionResult(bool success)
        {
            this.Succeeded = success;
            this.Errors = new string[0];
        }

        public ExecutionResult(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                errors = new string[] { Resources.DefaultError };
            }
            this.Succeeded = false;
            this.Errors = errors;
        }

        public static ExecutionResult Failed(params string[] errors)
        {
            return new ExecutionResult(errors);
        }

        public IEnumerable<string> Errors { get; private set; }

        public bool Succeeded { get; private set; }

        public static ExecutionResult Success
        {
            get
            {
                return _success;
            }
        }
    }
}



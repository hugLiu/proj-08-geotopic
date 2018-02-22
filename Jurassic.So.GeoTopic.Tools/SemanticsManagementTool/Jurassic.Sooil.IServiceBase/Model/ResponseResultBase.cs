using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class ResponseResultBase
    {
        public virtual int Flag { get; protected set; }

        public virtual string Message { get; protected set; }

        public virtual string Code { get; protected set; }

        protected ResponseResultBase(StateCodes stateCode, string debugMessage, string message)
        {
            if (stateCode == StateCodes.Success)
            {
                throw new ArgumentException("message", "stateCode must be not StateCodes.Success.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("message");
            }

            if (string.IsNullOrWhiteSpace(debugMessage))
            {
                throw new ArgumentNullException("debugMessage");
            }

            Flag = (int)stateCode;
            Message = message; //调试信息，报给用户的错误
            Code = debugMessage;   //系统错误信息，给开发人员看
        }

        protected ResponseResultBase()
        {
            Flag = (int)StateCodes.Success;
        }

        public static ResponseResultBase Success()
        {
            return new ResponseResultBase();
        }
        public static async Task<ResponseResultBase> SuccessAsync()
        {
            return await Task.FromResult(new ResponseResultBase());
        }

        public static async Task<ResponseResult<TData>> SuccessAsync<TData>(TData data)
        {
            return await Task.FromResult(new ResponseResult<TData>(data));
        }

        public static ResponseResultBase Failed(StateCodes stateCode, string debugMessage, string message)
        {
            return new ResponseResultBase(stateCode, debugMessage, message);
        }

        public async static Task<ResponseResultBase> FailedAsync(StateCodes stateCode, string debugMessage, string message)
        {
            return await Task.FromResult(new ResponseResultBase(stateCode, debugMessage, message));
        }


    }
}

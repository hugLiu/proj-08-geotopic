using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public sealed class ResponseResult<TData> : ResponseResultBase
    {
        public override int Flag
        {
            get { return base.Flag; }
            protected set { base.Flag = value; }
        }

        public override string Message
        {
            get { return base.Message; }
            protected set { base.Message = value; }
        }

        public override string Code
        {
            get { return base.Code; }
            protected set { base.Code = value; }
        }

        public TData Data { get; set; }

        public ResponseResult(TData data)
            : base()
        {
            Data = data;
        }
        protected ResponseResult(StateCodes stateCode, string message, string code)
            : base(stateCode, message, code)
        {

        }
        public static ResponseResult<TData> Success(TData data)
        {
            return new ResponseResult<TData>(data);
        }

        public static async Task<ResponseResult<TData>> SuccessAsync(TData data)
        {
            return await Task.FromResult(new ResponseResult<TData>(data));
        }

        public async static Task<ResponseResult<TData>> FailedAsync(StateCodes stateCode, string message, string code)
        {
            return await Task.FromResult(new ResponseResult<TData>(stateCode, message, code));
        }

        public static Task<ResponseResult<TData>> Failed(StateCodes stateCode, string message, string code)
        {
            return Task.FromResult(new ResponseResult<TData>(stateCode, message, code));
        }
    }
}

using System;

namespace Jurassic.So.SpiderTool.IService.Processers
{
    public abstract class Processor<S, T>
    {
        private Processor<S, T> _next;

        public Processor<S, T> GetNext()
        {
            return this._next;
        }

        public void SetNext(Processor<S, T> next)
        {
            this._next = next;
        }

        protected abstract bool CanProcess(S s);

        protected abstract T DoProcess(S s);

        public T CreateProcessor(S s)
        {
            if (CanProcess(s))
            {
                return this.DoProcess(s);
            }
            else
            {
                if (GetNext() != null)
                {
                    return GetNext().CreateProcessor(s);
                }
                else
                {
                    throw new Exception("不存在相关处理器！");
                }
            }
        }
    }
}

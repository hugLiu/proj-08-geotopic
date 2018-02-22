using System;

namespace Jurassic.Sooil.IServiceBase
{
    public abstract class Store : IDisposable
    {
      
        protected bool Disposed { get; set; }

      
        /// <summary>
        /// 摧毁必要的资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 将自动摧毁使用过的事务以及执行子类中需要清理的对象
        /// </summary>
        /// <param name="disposing"></param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// 检测如果需要使用的对象是否摧毁，如果是使用的对象已经摧毁将抛出异常
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        protected void ThrowIfDisposed()
        {
            if (this.Disposed)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }

    }
}

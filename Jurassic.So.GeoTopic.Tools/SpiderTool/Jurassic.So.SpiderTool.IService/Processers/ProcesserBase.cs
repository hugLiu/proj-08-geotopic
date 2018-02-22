using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Jurassic.So.SpiderTool.IService.Processers
{

    /// <summary>
    /// 定义可以排队处理的一些业务操作的基类
    /// </summary>
    public abstract class ProcesserBase
    {
        readonly ConcurrentQueue<object> _itemsQueue = new ConcurrentQueue<object>();
        private int _processed = 0;

        public bool Enabled { get; set; }

        /// <summary>
        /// 任务实例标示符
        /// </summary>
        public string TaskInstanceId { get; set; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 已经处理的项数目
        /// </summary>
        public int Processed => _processed;

        /// <summary>
        /// 所有项数目
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 剩余的项数目
        /// </summary>
        public int Remain => _itemsQueue.Count;

        /// <summary>
        /// 其他信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 完成百分比
        /// </summary>
        public int ProcessedPercent => Processed * 100 / Total;

        /// <summary>
        /// 任务开始执行的委托
        /// </summary>
        public event EventHandler TaskStart;

        /// <summary>
        /// 报告进度的委托
        /// </summary>
        public event EventHandler ProgressChanged;

        public void FireProgressChanged()
        {
            ProgressChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 任务结束执行的委托
        /// </summary>
        public event EventHandler TaskFinished;

        /// <summary>
        /// 在处理器中添加一个元素并开始处理
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            _itemsQueue.Enqueue(item);
            if (!_inProcess && Enabled)
            {
                _inProcess = true;
                Start();
            }
        }

        ~ProcesserBase()
        {
            if (!_inProcess)
            {
                ProcessCall(null);
            }
        }

        /// <summary>
        /// 待处理资讯出队
        /// </summary>
        /// <returns></returns>
        object DeQueue()
        {
            object item;
            _itemsQueue.TryDequeue(out item);
            return item;
        }

        bool _inProcess;
        void ProcessCall(object stateInfo)
        {
            try
            {
                object item;
                while (!EqualityComparer<object>.Default.Equals(item = DeQueue(), default(object)) && Enabled)
                {
                    if (Processed == 0 && TaskStart != null)
                    {
                        ProgressChanged?.Invoke(this, EventArgs.Empty);
                    }

                    Process(item);

                    _processed++;

                    ProgressChanged?.Invoke(this, EventArgs.Empty);

                    if (Remain == 0)
                    {
                        TaskFinished?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            finally
            {
                _inProcess = false;
            }
        }

        /// <summary>
        /// 实际用于处理item的方法
        /// </summary>
        /// <param name="item"></param>
        public abstract void Process(object item);


        internal void Start()
        {
            Total = _itemsQueue.Count;
            Enabled = true;
            ThreadPool.QueueUserWorkItem(ProcessCall);
        }

        internal void Stop()
        {
            Enabled = false;
        }
    }
}
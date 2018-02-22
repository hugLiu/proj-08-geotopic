using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.So.SpiderTool.IService.Processers
{
    /// <summary>
    /// 定义用于延时处理一些逻辑的处理器工厂
    /// </summary>
    public class ProcesserFactory
    {
        private readonly ConcurrentDictionary<string, ProcesserBase> procTable = new ConcurrentDictionary<string, ProcesserBase>();

        public event EventHandler TaskFinished;

        /// <summary>
        /// 将待处理对象添加进工厂中的指定处理器中
        /// </summary>
        /// <param name="procName">处理器名称</param>
        /// <param name="obj">待处理对象</param>
        public void Add(string procName, object obj)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Add(obj);
        }

        public void Start(string procName)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Start();
        }

        public void Stop(string procName)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Stop();
        }

        public IList<ProcesserBase> GetAllProcessers()
        {
            return procTable.Values.ToList();
        }

        public void ClearAll()
        {
            procTable.Clear();
        }

        /// <summary>
        /// 在工厂中立即处理待处理对象
        /// </summary>
        /// <typeparam name="T">待处理对象的类型</typeparam>
        /// <param name="procName">处理器名称</param>
        /// <param name="obj">待处理对象</param>
        public void Process(string procName, object obj)
        {
            var proc = procTable[procName];
            if (proc == null) return;
            procTable[procName].Process(obj);
        }

        void ProcesserFactory_TaskFinished(object sender, EventArgs e)
        {
            ProcesserBase processer = (ProcesserBase)sender;
            ProcesserBase value = null;

            this.procTable.TryRemove(processer.Name, out value);

            if (TaskFinished != null)
            {
                this.TaskFinished(processer.Name, e);
            }
        }

        /// <summary>
        /// 注册新的处理器
        /// </summary>
        /// <typeparam name="T">待处理对象的类型</typeparam>
        /// <param name="procName">处理器名称</param>
        /// <param name="processer">处理器实例</param>
        public virtual void Register(string procName, ProcesserBase processer)
        {
            processer.Name = procName;
            processer.TaskInstanceId = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid();
            procTable[processer.Name] = processer;
            procTable[processer.Name].TaskFinished += ProcesserFactory_TaskFinished;
            AfterRegister(processer);
        }

        protected virtual void AfterRegister(ProcesserBase processer)
        {

        }
    }
}
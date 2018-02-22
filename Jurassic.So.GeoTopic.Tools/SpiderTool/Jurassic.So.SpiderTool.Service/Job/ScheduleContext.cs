using Jurassic.CommonModels;
using Jurassic.So.SpiderTool.Service.Triggers;
using Ninject;
using System;
using System.Reflection;
using Jurassic.So.SpiderTool.IService.Processers;
using Jurassic.So.SpiderTool.IService.ViewModel;

namespace Jurassic.So.SpiderTool.Service
{
    public sealed class ScheduleContext
    {
        private static ScheduleContext _context = null;

        private Processor<SchedulePlan, ITriggerCreator> _triggerCreator = null;

        public SpiderSchedule SpiderSchedule { get; set; } = null;

        private ScheduleContext()
        {
            this.SpiderSchedule = SiteManager.Kernel.Get<SpiderSchedule>();
            InitailChain();
        }

        /// <summary>
        ///  获取适配器上下文
        /// </summary>
        /// <returns></returns>
        public static ScheduleContext GetContext()
        {
            if (_context != null) return _context;
            _context = new ScheduleContext();
            return _context;
        }

        /// <summary>
        /// 处理始化各种处理器链
        /// </summary>
        private void InitailChain()
        {
            Assembly ass = typeof(ITriggerCreator).Assembly;
            foreach (var t in ass.DefinedTypes)
            {
                Attribute a = Attribute.GetCustomAttribute(t, typeof(ProcessorAttribute));
                if (a == null) continue;
                if (t.BaseType == null) continue;
                Type baseType = t.BaseType.GenericTypeArguments[1];
                if (baseType == typeof(ITriggerCreator))
                {
                    SetChain(ref _triggerCreator, t);
                }
            }
        }

        private void SetChain<S, T>(ref Processor<S, T> chain, Type target)
        {
            Processor<S, T> newProcessor = (Processor<S, T>)Activator.CreateInstance(target);
            if (chain == null)
            {
                chain = newProcessor;
            }
            else
            {
                newProcessor.SetNext(chain);
                chain = newProcessor;
            }
        }
        public ITriggerCreator GetTriggerProcessor(SchedulePlan p)
        {
            return _triggerCreator.CreateProcessor(p);
        }
    }
}

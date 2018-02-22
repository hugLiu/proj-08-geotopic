using System;

namespace Jurassic.So.SpiderTool.IService.Processers
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]//设置了定位参数和命名参数
    public class ProcessorAttribute : Attribute
    {
        public string Type { get; set; }
    }
}

using Quartz;

namespace Jurassic.So.SpiderTool.Service.Triggers
{
    public interface ITriggerCreator
    {
        ITrigger CreateCronTrigger();
    }
}

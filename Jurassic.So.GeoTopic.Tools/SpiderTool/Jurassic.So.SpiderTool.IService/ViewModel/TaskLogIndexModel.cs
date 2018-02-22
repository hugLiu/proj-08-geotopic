using System;

namespace Jurassic.So.SpiderTool.IService.ViewModel
{
    public class TaskLogIndexModel
    {
        public int AdapterId { get; set; }
        public string AdapterTaskLogIdentity { get; set; }
        public string AdapterName { get; set; }
        public string DataSourceName { get; set; }
        public int TaskCount { get; set; }
        public int TaskSuccessCount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ElapsedTime
        {
            get
            {
                if (EndDate == null)
                {
                    return string.Empty;
                }
                var span = (TimeSpan)(EndDate - StartDate);
                return span.ToString().Substring(0, 8);
            }
        }
        public string Performer { get; set; }
    }
}

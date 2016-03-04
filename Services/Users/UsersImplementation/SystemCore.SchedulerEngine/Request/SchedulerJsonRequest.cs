using Scheduler.DataRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCore.SchedulerEngine.Request
{
    public class SchedulerJsonRequest
    {
        public SchedulerIntervals interval { get; set; }
        public FrequenceType frequencyType { get; set; }
        public int frequencyInterval { get; set; }
        public FrequencyRelativeInterval frequencyRelativeInterval { get; set; }
        public int frequencyRecurrenceFactor { get; set; }
    }
}

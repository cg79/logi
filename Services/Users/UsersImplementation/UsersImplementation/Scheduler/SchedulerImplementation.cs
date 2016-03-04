using Scheduler.DataRequest;
using SchedulerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.Scheduler
{
    public class SchedulerImplementation
    {
        public object GetRecurrenceValues(string request)
        {
            SchedulerResponse resp = SchedulerEngineManager.GetRecurrenceValuesFromJson(request);
            return resp;
        }
    }
}

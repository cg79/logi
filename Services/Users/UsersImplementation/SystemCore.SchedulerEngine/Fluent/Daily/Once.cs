using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Base;
using SchedulerEngine.Renderer;

namespace SchedulerEngine.Request.Daily
{
    /// <summary>
    /// Once class used to get the developer information for week days
    /// </summary>
    public class Once : BaseRequest 
    {
        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Once SchedulerSettings(Action<SchedulerRequestSettings> settings)
        {
            this.intervalsWrapper = new IntervalsWrapper();
            settings(intervalsWrapper);

            return this;
        }


        /// <summary>
        /// Set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public Once SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }

        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public SchedulerResponse GetRecurrenceValues()
        {
            SchedulerRequest request = CreateSchedulerRequest();

            DailyRequest dailyRequest = new DailyRequest();
            dailyRequest.DailyReccurenceType = DailyType.OnEveryWeekday;
            
            request.DailyRequest = dailyRequest;


            SchedulerResponse values = GetRecurringInfo(request);
          
            
            return values;
        }
        
    }
}

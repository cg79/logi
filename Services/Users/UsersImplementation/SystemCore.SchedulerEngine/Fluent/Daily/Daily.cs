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
    /// Daily class used to read the developer input request
    /// </summary>
    public class Daily : BaseRequest 
    {
        /// <summary>
        /// DailyType value; 
        /// DailyType { NotSet = -1, OnEveryXDays = 1, OnEveryWeekday = 4 ,}
        /// </summary>
        protected DailyType dailyType = DailyType.OnEveryXDays;

        /// <summary>
        /// Set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public Daily SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }
        
        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Daily SchedulerSettings(Action<SchedulerRequestSettings> settings)
        {
            base.intervalsWrapper = new IntervalsWrapper();
            settings(intervalsWrapper);

            return this;
        }


        protected int everyXDays = 1;
        /// <summary>
        /// Set the EveryXDays value 
        /// </summary>
        /// <param name="everyXDays"></param>
        /// <returns></returns>
        public Daily EveryXDays(int everyXDays)
        {
            if (everyXDays < 0)
                return this;

            this.everyXDays = everyXDays;
            dailyType = DailyType.OnEveryXDays;
            return this;
        }


        /// <summary>
        /// Returns the Recurrence Values
        /// </summary>
        /// <returns></returns>
        public SchedulerResponse GetRecurrenceValues()
        {
            SchedulerRequest request = CreateSchedulerRequest();

            DailyRequest dailyRequest = new DailyRequest();
            dailyRequest.DailyReccurenceType = DailyType.OnEveryXDays;
            dailyRequest.EveryXDays = everyXDays;

            request.DailyRequest = dailyRequest;


            SchedulerResponse values = GetRecurringInfo(request);
          
            return values;
        }
        
    }
}

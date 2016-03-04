using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Renderer;
using SchedulerEngine.Base;


namespace SchedulerEngine.Request.Monthly
{
    public class MonthlyRelative:BaseRequest
    {
        protected MonthlyType MonthlyType { get; set; }

        #region Settings
       
        public MonthlyRelative SchedulerSettings(Action<SchedulerRequestSettings> settings)
        {
            this.intervalsWrapper = new IntervalsWrapper();
            settings(intervalsWrapper);

            return this;
        }

        public MonthlyRelative SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }
        #endregion
        /// <summary>
        /// Second part of a custom date. This is day of week, weekend day, etc.
        /// </summary>
        protected FrequencyDaysValue frequencyInterval { get; set; }

        public MonthlyRelative FrequencyInterval(FrequencyDaysValue frequencyInterval)
        {
            this.frequencyInterval = frequencyInterval;
            return this;
        }

        

        /// <summary>
        /// First part of a custom date. This would be First, Second, etc. item of the month.
        /// </summary>
        protected FrequencyRelativeInterval relativeInterval { get; set; }

        public MonthlyRelative FrequencyRelative(FrequencyRelativeInterval relativeInterval)
        {
            this.relativeInterval = relativeInterval;
            return this;
        }


        protected int everyXMonths = 1;
        public MonthlyRelative EveryXMonths(int everyXMonths)
        {
            this.everyXMonths = everyXMonths;
            if (everyXMonths == 0)
            {
                everyXMonths = 1;
            }
            return this;
        }


        public virtual SchedulerResponse GetRecurrenceValues()
        {

            SchedulerRequest request = CreateSchedulerRequest();


            MonthlyRequest monthlyRequest = new MonthlyRequest();


            monthlyRequest.MonthlyType = MonthlyType.MonthlyRelative;
            monthlyRequest.MonthlyRelativePattern = new MonthlyRelativePattern();
            monthlyRequest.MonthlyRelativePattern.FrequencyRelative = (FrequencyRelativeInterval)relativeInterval;
            monthlyRequest.MonthlyRelativePattern.FrequencyDaysValue = (FrequencyDaysValue)frequencyInterval;
            monthlyRequest.MonthlyRelativePattern.EveryXMonths = everyXMonths;


            request.MonthlyRequest = monthlyRequest;

            SchedulerResponse values = GetRecurringInfo(request);

            return values;
        }
        

    }
}

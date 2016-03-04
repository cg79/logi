using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Renderer;
using SchedulerEngine.Base;


namespace SchedulerEngine.Request.Yearly
{
    public class Yearly:BaseRequest
    {
        protected YearlyType yearlyType = YearlyType.Yearly;

        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Yearly SchedulerSettings(Action<SchedulerRequestSettings> settings)
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
        public Yearly SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }

        protected int dayNo = 1;
        /// <summary>
        /// Day NO
        /// </summary>
        /// <param name="dayNo"></param>
        /// <returns></returns>
        public Yearly DayNo(int dayNo)
        {
            this.dayNo = dayNo;
            return this;
        }

        protected FrequencyMonthValue month { get; set; }
        /// <summary>
        /// the Month used for a Yearly request
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public Yearly Month(FrequencyMonthValue month)
        {
            this.month = month;
            return this;
        }

        /// <summary>
        /// Every X years
        /// Default value is 1
        /// </summary>
        protected int everyXYears = 1;
        public Yearly EveryXYears(int everyXYears)
        {
            this.everyXYears = everyXYears;
            return this;
        }

        /// <summary>
        /// Get the recurrence Values
        /// </summary>
        /// <returns></returns>
        public virtual SchedulerResponse GetRecurrenceValues()
        {
            SchedulerRequest request = CreateSchedulerRequest();

            YearlyRequest yearlyRequest = new YearlyRequest();
            yearlyRequest.YearlyType = this.yearlyType;

            yearlyRequest.YearlyPattern = new YearlyPattern();
            yearlyRequest.YearlyPattern.Month = (FrequencyMonthValue)month;
            yearlyRequest.YearlyPattern.DayNo = dayNo;
            yearlyRequest.YearlyPattern.EveryXYears = everyXYears;

            request.YearlyRequest = yearlyRequest;

            SchedulerResponse values = GetRecurringInfo(request);

            return values;
        }
        
    }
}

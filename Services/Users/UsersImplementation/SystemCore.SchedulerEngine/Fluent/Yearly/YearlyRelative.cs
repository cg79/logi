using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Renderer;
using SchedulerEngine.Base;


namespace SchedulerEngine.Request.Yearly
{
    public class YearlyRelative:BaseRequest
    {

        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public YearlyRelative SchedulerSettings(Action<SchedulerRequestSettings> settings)
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
        public YearlyRelative SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }

      
        protected FrequencyRelativeInterval frequencyRelative { get; set; }
        /// <summary>
        /// FrequencyRelativeInterval
        /// {  First=1, Second=2, Third=4, Fourth=8, Fifth = 16,Last=32 }
        /// </summary>
        public YearlyRelative FrequencyRelative(FrequencyRelativeInterval frequencyRelative)
        {
            this.frequencyRelative = frequencyRelative;
            return this;
        }

        protected FrequencyDaysValue frequencyInterval { get; set; }
        
        /// <summary>
        /// FrequencyDaysValue
        /// FrequencyDaysValue {  Sunday = 1, Monday =2, Tuesday=3, Wednesday=4, Thursday=5, Friday=6, Saturday=7, Day=8, Weekday=9, WeekendDay=10, }
        /// </summary>
        /// <param name="frequencyInterval"></param>
        /// <returns></returns>
        public YearlyRelative FrequencyInterval(FrequencyDaysValue frequencyInterval)
        {
            this.frequencyInterval = frequencyInterval;
            return this;
        }

        protected FrequencyMonthValue frequenceRecurrenceFactor { get; set; }
        /// <summary>
        /// FrequencyMonthValue
        /// FrequencyMonthValue {  January = 1, February, March, April, May, June, July, August, September, October, November, December }
        /// </summary>
        /// <param name="frequenceRecurrenceFactor"></param>
        /// <returns></returns>
        public YearlyRelative FrequenceRecurrenceFactor(FrequencyMonthValue frequenceRecurrenceFactor)
        {
            this.frequenceRecurrenceFactor = frequenceRecurrenceFactor;
            return this;
        }

        protected int everyXYears = 1;
        public YearlyRelative EveryXYears(int everyXYears)
        {
            this.everyXYears = everyXYears;
            return this;
        }

        /// <summary>
        /// Return the Reccurence values
        /// </summary>
        /// <returns></returns>
        public virtual SchedulerResponse GetRecurrenceValues()
        {

            SchedulerRequest request = CreateSchedulerRequest();

            YearlyRequest yearlyRequest = new YearlyRequest();
            yearlyRequest.YearlyType = YearlyType.YearlyRelative;

            yearlyRequest.YearlyRelativePattern = new YearlyRelativePattern();
            yearlyRequest.YearlyRelativePattern.FrequencyRelative = (FrequencyRelativeInterval)frequencyRelative;
            yearlyRequest.YearlyRelativePattern.FrequencyDayValue = (FrequencyDaysValue)frequencyInterval;
            yearlyRequest.YearlyRelativePattern.FrequencyMonthValue = (FrequencyMonthValue)frequenceRecurrenceFactor;
            yearlyRequest.YearlyRelativePattern.EveryXYears = everyXYears;

            request.YearlyRequest = yearlyRequest;

            SchedulerResponse values = GetRecurringInfo(request);

            return values;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Renderer;
using SchedulerEngine.Base;

namespace SchedulerEngine.Request.Monthly
{
    /// <summary>
    /// Represent the Monthly request used for a fluent interface approach
    /// </summary>
    public class Monthly : BaseRequest
    {
        #region Settings

        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Monthly SchedulerSettings(Action<SchedulerRequestSettings> settings)
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
        public Monthly SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }
        #endregion

        protected int dayNo = 1;

        /// <summary>
        /// Day No
        /// </summary>
        /// <param name="dayNo"></param>
        /// <returns></returns>
        public Monthly DayNo(int dayNo)
        {
            this.dayNo = dayNo;
            if (dayNo == 0)
            {
                dayNo = 1;
            }
            return this;
        }

        protected int everyXMonths = 1;
        /// <summary>
        /// Every X Months
        /// </summary>
        /// <param name="everyXMonths"></param>
        /// <returns></returns>
        public Monthly EveryXMonths(int everyXMonths)
        {
            this.everyXMonths = everyXMonths;
            if (everyXMonths == 0)
            {
                everyXMonths = 1;
            }
            return this;
        }

        /// <summary>
        /// Returns the Recurrence Values
        /// </summary>
        /// <returns></returns>
        public virtual SchedulerResponse GetRecurrenceValues()
        {
            SchedulerRequest request = CreateSchedulerRequest();

            MonthlyRequest monthlyRequest = new MonthlyRequest();


            monthlyRequest.MonthlyType = MonthlyType.Monthly;
            monthlyRequest.MonthlyPattern = new MonthlyPattern();
            monthlyRequest.MonthlyPattern.EveryXMonths = everyXMonths;
            monthlyRequest.MonthlyPattern.DayNo = dayNo;


            request.MonthlyRequest = monthlyRequest;

            SchedulerResponse values = GetRecurringInfo(request);

            return values;
        }

    }
}

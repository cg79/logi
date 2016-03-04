using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;


using SchedulerEngine.Base;
using SchedulerEngine.Renderer;

namespace SchedulerEngine.Request.Weekly
{
    public class Weekly : BaseRequest
    {
        /// <summary>
        /// Delegate used to set the scheduler interval; Start Date; End Date, Recurrence No
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Weekly SchedulerSettings(Action<SchedulerRequestSettings> settings)
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
        public Weekly SchedulerSettings(SchedulerIntervals range)
        {
            base.intervals = range;

            return this;
        }

        protected SelectedDayOfWeekValues SelectedDayOfWeekValues;

        protected int everyXWeeks = 1;
        /// <summary>
        /// Every X Weeks
        /// </summary>
        /// <param name="everyXWeeks"></param>
        /// <returns></returns>
        public Weekly EveryXWeeks(int everyXWeeks)
        {
            this.everyXWeeks = everyXWeeks;
            return this;
        }

        #region Selected Days
        
        /// <summary>
        /// The developer has the option to set the selected days
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public Weekly SelectedDays(params DayEnum[] day)
        {
            //SelectedDayOfWeekValues = new SelectedDayOfWeekValues();

            foreach (DayEnum dayEnum in day)
            {

                switch (dayEnum)
                {
                    case DayEnum.Monday:
                        {
                            SelectedDayOfWeekValues.Monday = true;
                            break;
                        }
                    case DayEnum.Tuesday:
                        {
                            SelectedDayOfWeekValues.Tuesday = true;
                            break;
                        }
                    case DayEnum.Wednesday:
                        {
                            SelectedDayOfWeekValues.Wednesday = true;
                            break;
                        }
                    case DayEnum.Thursday:
                        {
                            SelectedDayOfWeekValues.Thursday = true;
                            break;
                        }
                    case DayEnum.Friday:
                        {
                            SelectedDayOfWeekValues.Friday = true;
                            break;
                        }
                    case DayEnum.Saturday:
                        {
                            SelectedDayOfWeekValues.Saturday = true;
                            break;
                        }
                    case DayEnum.Sunday:
                        {
                            SelectedDayOfWeekValues.Sunday = true;
                            break;
                        }
                }
            }

            return this;
        }

        /// <summary>
        /// Selected days as a bit wise approach
        /// </summary>
        /// <param name="selectedDaysBitWiseValue"></param>
        /// <returns></returns>
        public Weekly SelectedDays(int selectedDaysBitWiseValue)
        {
            #region Weekly
            if (selectedDaysBitWiseValue >= 64)
            {
                SelectedDayOfWeekValues.Saturday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 64;
            }

            if (selectedDaysBitWiseValue >= 32)
            {
                SelectedDayOfWeekValues.Friday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 32;
            }

            if (selectedDaysBitWiseValue >= 16)
            {
                SelectedDayOfWeekValues.Thursday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 16;
            }
            if (selectedDaysBitWiseValue >= 8)
            {
                SelectedDayOfWeekValues.Wednesday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 8;
            }

            if (selectedDaysBitWiseValue >= 4)
            {
                SelectedDayOfWeekValues.Tuesday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 4;
            }
            if (selectedDaysBitWiseValue >= 2)
            {
                SelectedDayOfWeekValues.Monday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 2;
            }

            if (selectedDaysBitWiseValue >= 1)
            {
                SelectedDayOfWeekValues.Sunday = true;
                selectedDaysBitWiseValue = selectedDaysBitWiseValue - 1;
            }
            #endregion
            return this;
        }
       
        #endregion

        /// <summary>
        /// Get the Recurrence Values
        /// </summary>
        /// <returns></returns>
        public SchedulerResponse GetRecurrenceValues()
        {

            SchedulerRequest request = CreateSchedulerRequest();


            request.RecurrencePattern = RecurrenceType.Weekly;

            WeeklyRequest weeklyRequest = new WeeklyRequest();
            weeklyRequest.SelectedDayOfWeekValues = SelectedDayOfWeekValues;
            weeklyRequest.EveryXWeeks = everyXWeeks;

            request.WeeklyRequest = weeklyRequest;

            SchedulerResponse values = GetRecurringInfo(request);
            return values;
        }
    }
}

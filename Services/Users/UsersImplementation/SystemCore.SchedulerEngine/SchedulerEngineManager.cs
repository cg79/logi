using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using SchedulerEngine.Request.Daily;
using SchedulerEngine.Request.Weekly;
using SchedulerEngine.Request.Monthly;
using SchedulerEngine.Request.Yearly;
using SchedulerEngine.Renderer;
using ESB.Utils.Serializers;
using SystemCore.SchedulerEngine.Request;

namespace SchedulerEngine
{
    /// <summary>
    /// Represent the class used by developers in order to retreive the recurrence information
    /// </summary>
    public static class SchedulerEngineManager
    {
        /// <summary>
        /// Represent a Once request
        /// returns only the workdays values
        /// </summary>
        /// <returns></returns>
        public static Once Once()
        {
            return new Once();
        }

        /// <summary>
        /// Represent a daily request
        /// </summary>
        /// <returns></returns>
        public static Daily Daily()
        {
            return new Daily();
        }

        /// <summary>
        /// Represent a weekly request
        /// </summary>
        /// <returns></returns>
        public static Weekly Weekly()
        {
            return new Weekly();
        }

        /// <summary>
        /// Represent a Monthly request
        /// </summary>
        /// <returns></returns>
        public static Monthly Monthly()
        {
            return new Monthly();
        }

        /// <summary>
        /// Represent a Monthly relative request
        /// </summary>
        /// <returns></returns>
        public static MonthlyRelative MonthlyRelative()
        {
            return new MonthlyRelative();
        }


        /// <summary>
        /// Represent a Yearly request
        /// </summary>
        /// <returns></returns>
        public static Yearly Yearly()
        {
            
            return new Yearly();
        }

        /// <summary>
        /// Represent a Yearly relative request
        /// </summary>
        /// <returns></returns>
        public static YearlyRelative YearlyRelative()
        {

            return new YearlyRelative();
        }

        /// <summary>
        /// Returns the recurrence values based on a previous request which was saved as a XML structure
        /// </summary>
        /// <param name="previousRequest"></param>
        /// <returns></returns>
        public static SchedulerResponse GetRecurrenceValues(string previousRequest)
        {
            SchedulerResponse resp = SchedulerRequestRenderer.GetRecurringInfo(previousRequest);
            return resp;
        }


        public static SchedulerResponse GetRecurrenceValuesFromJson(string request)
        {
            SchedulerJsonRequest schedRequest = request.JsonDeserialize<SchedulerJsonRequest>();
            SchedulerResponse response = GetRecurrenceValues(schedRequest.interval, schedRequest.frequencyType, schedRequest.frequencyInterval, schedRequest.frequencyRelativeInterval, schedRequest.frequencyRecurrenceFactor, 1);
            return response;
        }

        /// <summary>
        /// Get the recurrence values for a call as Microsoft 
        /// See http://msdn.microsoft.com/en-us/library/ms366342.aspx as a reference
        /// The Scheduler Intervals are sent by developer
        /// </summary>
        /// <param name="intervals"></param>
        /// <param name="frequencyType"></param>
        /// <param name="frequencyInterval"></param>
        /// <param name="frequencyRelativeInterval"></param>
        /// <param name="frequencyRecurrenceFactor"></param>
        /// <param name="yearRelativeXYears"></param>
        /// <returns></returns>
        public static SchedulerResponse GetRecurrenceValues(
          SchedulerIntervals intervals,
          FrequenceType frequencyType,
           int frequencyInterval,
          FrequencyRelativeInterval frequencyRelativeInterval,
          int frequencyRecurrenceFactor,
            int yearRelativeXYears
          )
        {
            SchedulerRequest request = new SchedulerRequest();
            request.SchedulerIntervals = intervals;

            switch (frequencyType)
            {
               
                case FrequenceType.Once:
                    {
                        DailyRequest dayRequest = new DailyRequest();
                        dayRequest.DailyReccurenceType = DailyType.OnEveryWeekday;
                        request.DailyRequest = dayRequest;

                        break;
                    }
                case FrequenceType.Daily:
                    {
                        #region OnEveryXDays
                        DailyRequest dayRequest = new DailyRequest();
                        dayRequest.DailyReccurenceType = DailyType.OnEveryXDays;
                        dayRequest.EveryXDays = frequencyInterval;
                        request.DailyRequest = dayRequest;
                        #endregion
                        break;
                    }
                case FrequenceType.Weekly:
                    {
                        WeeklyRequest weeklyRequest = new WeeklyRequest();
                        weeklyRequest.EveryXWeeks = frequencyRecurrenceFactor;

                        #region Weekly
                        if (frequencyInterval >= 64)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Saturday = true;
                            frequencyInterval = frequencyInterval - 64;
                        }

                        if (frequencyInterval >= 32)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Friday = true;
                            frequencyInterval = frequencyInterval - 32;
                        }

                        if (frequencyInterval >= 16)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Thursday = true;
                            frequencyInterval = frequencyInterval - 16;
                        }
                        if (frequencyInterval >= 8)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Wednesday = true;
                            frequencyInterval = frequencyInterval - 8;
                        }

                        if (frequencyInterval >= 4)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Tuesday = true;
                            frequencyInterval = frequencyInterval - 4;
                        }
                        if (frequencyInterval >= 2)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Monday = true;
                            frequencyInterval = frequencyInterval - 2;
                        }

                        if (frequencyInterval >= 1)
                        {
                            weeklyRequest.SelectedDayOfWeekValues.Sunday = true;
                            frequencyInterval = frequencyInterval - 1;
                        }
                        #endregion

                        request.WeeklyRequest = weeklyRequest;
                        break;
                    }
                case FrequenceType.Monthly:
                    {
                        MonthlyRequest monthlyRequest = new MonthlyRequest();
                        monthlyRequest.MonthlyType = MonthlyType.Monthly;
                        monthlyRequest.MonthlyPattern = new MonthlyPattern();
                        monthlyRequest.MonthlyPattern.DayNo = frequencyInterval;
                        monthlyRequest.MonthlyPattern.EveryXMonths = frequencyRecurrenceFactor;

                        request.MonthlyRequest = monthlyRequest;
                        break;
                    }
                case FrequenceType.MonthlyRelative:
                    {
                        MonthlyRequest monthlyRequest = new MonthlyRequest();
                        monthlyRequest.MonthlyType = MonthlyType.MonthlyRelative;

                        monthlyRequest.MonthlyRelativePattern = new MonthlyRelativePattern();
                        monthlyRequest.MonthlyRelativePattern.EveryXMonths = frequencyRecurrenceFactor;
                        monthlyRequest.MonthlyRelativePattern.FrequencyDaysValue = (FrequencyDaysValue)frequencyInterval;
                        monthlyRequest.MonthlyRelativePattern.FrequencyRelative = (FrequencyRelativeInterval)frequencyRelativeInterval;

                        request.MonthlyRequest = monthlyRequest;
                        break;
                    }
                case FrequenceType.Yearly:
                    {
                        YearlyRequest yearlyRequest = new YearlyRequest();
                        yearlyRequest.YearlyType = YearlyType.Yearly;
                        yearlyRequest.YearlyPattern = new YearlyPattern();
                        yearlyRequest.YearlyPattern.DayNo = frequencyRecurrenceFactor;
                        yearlyRequest.YearlyPattern.Month = (FrequencyMonthValue)frequencyInterval;

                        yearlyRequest.YearlyPattern.EveryXYears = yearRelativeXYears;

                        request.YearlyRequest = yearlyRequest;
                        break;
                    }
                case FrequenceType.YearlyRelative:
                    {
                        YearlyRequest yearlyRequest = new YearlyRequest();

                        yearlyRequest.YearlyType = YearlyType.YearlyRelative;
                        yearlyRequest.YearlyRelativePattern = new YearlyRelativePattern();
                        yearlyRequest.YearlyRelativePattern.FrequencyRelative = (FrequencyRelativeInterval)frequencyRelativeInterval;
                        yearlyRequest.YearlyRelativePattern.FrequencyDayValue = (FrequencyDaysValue)frequencyInterval;
                        yearlyRequest.YearlyRelativePattern.FrequencyMonthValue = (FrequencyMonthValue)frequencyRecurrenceFactor;
                        yearlyRequest.YearlyRelativePattern.EveryXYears = yearRelativeXYears;

                        request.YearlyRequest = yearlyRequest;
                        break;
                    }
            }

            SchedulerResponse response = SchedulerRequestRenderer.GetRecurringInfo(request);
            return response;
        }


        /// <summary>
        /// Get the recurence values in the same way as Microsoft
        /// See http://msdn.microsoft.com/en-us/library/ms366342.aspx as a reference
        /// </summary>
        /// <param name="frequencyType"></param>
        /// <param name="frequencyInterval"></param>
        /// <param name="frequencyRelative"></param>
        /// <param name="frequencyRecurrenceFactor"></param>
        /// <param name="yearRelativeXYears"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="occurenceNo"></param>
        /// <returns></returns>
        public static SchedulerResponse GetRecurrenceValues(
           FrequenceType frequencyType,
           int frequencyInterval,
           FrequencyRelativeInterval frequencyRelative,
           int frequencyRecurrenceFactor,
            int yearRelativeXYears,
           DateTime startDate,
           DateTime? endDate,
           int occurenceNo
           )
        {
            //return null;
            SchedulerIntervals intervals = new SchedulerIntervals();
            if (endDate.HasValue)
            {
                intervals.IntervalPattern = IntervalPattern.EndDate;
            }
            else
            {
                if (occurenceNo == 0)
                {
                    intervals.IntervalPattern = IntervalPattern.NotSet;
                }
                else
                {
                    intervals.IntervalPattern = IntervalPattern.OccurenceNumber;
                }
            }
            intervals.StartDate = startDate;
            intervals.EndDate = endDate;
            intervals.MaxOccurencesNo = occurenceNo;



            return GetRecurrenceValues(intervals, frequencyType, frequencyInterval, frequencyRelative, frequencyRecurrenceFactor, yearRelativeXYears);
        }
    }
}

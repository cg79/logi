using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;
using Scheduler.Logic;
using SchedulerEngine;
using System.Configuration;

namespace SchedulerEngine.Renderer
{
    /// <summary>
    /// Class used to invoke the logic of the scheduler
    /// 
    /// </summary>
    internal  class SchedulerRequestRenderer
    {
        /// <summary>
        /// Return the Recurring information based on a XML request passed as a string
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static SchedulerResponse GetRecurringInfo(string request)
        {

            SchedulerRequest test = request.FromXml<SchedulerRequest>();
            return GetRecurringInfo(test);
        }

        /// <summary>
        /// Max Recurrence No
        /// the default value is 1100 and can be ovevriden by setting the MaxRecurrenceNo key into the AppSettings
        /// </summary>
        private static int MaxRecurrenceNo
        {
            get
            {
                int confValue = 0;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("MaxRecurrenceNo"))
                {
                    if (Int32.TryParse(ConfigurationManager.AppSettings["MaxRecurrenceNo"], out confValue))
                    {
                        return confValue;
                    }
                    return 1100;
                }
                return 1100;
            }
        }

        private static bool UseService
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Get the Recurrence values by calling the Scheduler logic classes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static SchedulerResponse GetRecurringInfo(SchedulerRequest request)
        {
            if (request == null || request.RecurrencePattern == RecurrenceType.NotSet)
                return null;

            SchedulerResponse resp = null;
            //if (UseService)
            //{
            //    SchedulerServiceClient client = new SchedulerServiceClient();

            //    resp = client.GetRecurringInfo(request);
            //    client.Close();

            //    return resp;

            //}
            RecurrenceValues values = null;

            request.UpperRecurrenceLimit = MaxRecurrenceNo;
            switch (request.RecurrencePattern)
            {
                case RecurrenceType.NotSet:
                    break;
                case RecurrenceType.Daily:
                    {
                        #region Daily Processing

                        DailyRecurrenceSettings da = null;

                        switch (request.SchedulerIntervals.IntervalPattern)
                        {
                            case IntervalPattern.NotSet:
                                {
                                    request.SchedulerIntervals.MaxOccurencesNo = MaxRecurrenceNo;
                                    da = new DailyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo,request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.EndDate:
                                {
                                    da = new DailyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.EndDate.Value, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.OccurenceNumber:
                                {
                                    da = new DailyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                        }


                        if (request.DailyRequest.DailyReccurenceType == DailyType.OnEveryXDays)
                        {
                            values = da.GetValues(request.DailyRequest.EveryXDays);
                        }
                        else
                        {
                            values = da.GetValues(1, DailyType.OnEveryWeekday);
                        }
                        #endregion

                        break;
                    }
                case RecurrenceType.Weekly:
                    {
                        bool isRequestValid =
                            request.WeeklyRequest.SelectedDayOfWeekValues.Sunday ||
                            request.WeeklyRequest.SelectedDayOfWeekValues.Monday ||
                            request.WeeklyRequest.SelectedDayOfWeekValues.Tuesday ||
                            request.WeeklyRequest.SelectedDayOfWeekValues.Wednesday||
                            request.WeeklyRequest.SelectedDayOfWeekValues.Thursday ||
                            request.WeeklyRequest.SelectedDayOfWeekValues.Friday ||
                            request.WeeklyRequest.SelectedDayOfWeekValues.Saturday;

                        if (!isRequestValid)
                        {
                            throw new Exception("Please select at least one day for a weekly request");
                        }

                        #region Weekly Processing
                        WeeklyRecurrenceSettings we = null;


                        switch (request.SchedulerIntervals.IntervalPattern)
                        {
                            case IntervalPattern.NotSet:
                                {
                                    request.SchedulerIntervals.MaxOccurencesNo = MaxRecurrenceNo;
                                    we = new WeeklyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.EndDate:
                                {
                                    we = new WeeklyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.EndDate.Value, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.OccurenceNumber:
                                {
                                    we = new WeeklyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                        }


                        values = we.GetValues(request.WeeklyRequest.EveryXWeeks, request.WeeklyRequest.SelectedDayOfWeekValues);
                        #endregion
                        break;
                    }
                case RecurrenceType.Monthly:
                    {
                        #region Monthly Processing
                        MonthlyRecurrenceSettings mo = null;
                        switch (request.SchedulerIntervals.IntervalPattern)
                        {
                            case IntervalPattern.NotSet:
                                {
                                    request.SchedulerIntervals.MaxOccurencesNo = MaxRecurrenceNo;
                                    mo = new MonthlyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.EndDate:
                                {
                                    mo = new MonthlyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.EndDate.Value, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.OccurenceNumber:
                                {
                                    mo = new MonthlyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                        }


                        switch (request.MonthlyRequest.MonthlyType)
                        {
                            case MonthlyType.NotSet:
                                {
                                    break;
                                }
                            case MonthlyType.Monthly:
                                {
                                    values = mo.GetValues(request.MonthlyRequest.MonthlyPattern.DayNo, request.MonthlyRequest.MonthlyPattern.EveryXMonths);
                                    break;
                                }
                            case MonthlyType.MonthlyRelative:
                                {
                                    values = mo.GetValues(request.MonthlyRequest.MonthlyRelativePattern.FrequencyRelative, request.MonthlyRequest.MonthlyRelativePattern.FrequencyDaysValue, request.MonthlyRequest.MonthlyRelativePattern.EveryXMonths);
                                    break;
                                }
                          

                        }
                        #endregion
                        break;
                    }
                case RecurrenceType.Yearly:
                    {
                        #region Yearly Processing
                        YearlyRecurrenceSettings yr = null;
                        switch (request.SchedulerIntervals.IntervalPattern)
                        {
                            case IntervalPattern.NotSet:
                                {
                                    request.SchedulerIntervals.MaxOccurencesNo = MaxRecurrenceNo;
                                    yr = new YearlyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.EndDate:
                                {
                                    yr = new YearlyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.EndDate.Value, request.UpperRecurrenceLimit);
                                    break;
                                }
                            case IntervalPattern.OccurenceNumber:
                                {
                                    yr = new YearlyRecurrenceSettings(request.SchedulerIntervals.StartDate, request.SchedulerIntervals.MaxOccurencesNo, request.UpperRecurrenceLimit);
                                    break;
                                }
                        }

                        switch (request.YearlyRequest.YearlyType)
                        {
                            case YearlyType.NotSet:
                                {
                                    break;
                                }

                            case YearlyType.Yearly:
                                {
                                    values = yr.GetValues(request.YearlyRequest.YearlyPattern.DayNo, (int)request.YearlyRequest.YearlyPattern.Month, request.YearlyRequest.YearlyPattern.EveryXYears);
                                    break;
                                }
                            case YearlyType.YearlyRelative:
                                {
                                    values = yr.GetValues(request.YearlyRequest.YearlyRelativePattern.FrequencyRelative, request.YearlyRequest.YearlyRelativePattern.FrequencyDayValue, request.YearlyRequest.YearlyRelativePattern.FrequencyMonthValue, request.YearlyRequest.YearlyRelativePattern.EveryXYears);
                                    break;
                                }
                          
                        }

                        #endregion
                        break;
                    }
            }

            resp = new SchedulerResponse();
            resp.Values = values.Values;

            if (request.SchedulerIntervals.EndDate.HasValue)
            {
                TimeSpan ts = request.SchedulerIntervals.EndDate.Value - request.SchedulerIntervals.StartDate;
                request.SchedulerIntervals.EndDate = values.EndDate.AddDays(ts.Days + 1);
            }

            
            //switch (request.RecurrencePattern)
            //{
            //    case RecurrenceType.NotSet:
            //        break;
            //    case RecurrenceType.Daily:
            //        {
            //            request.SchedulerIntervals.StartDate = values.EndDate.AddDays(request.DailyRequest.EveryXDays);
            //            break;
            //        }
            //    case RecurrenceType.Weekly:
            //        {
            //            request.SchedulerIntervals.StartDate = values.EndDate.AddDays(request.WeeklyRequest.EveryXWeeks *7);
            //            break;
            //        }
            //    case RecurrenceType.Monthly:
            //        {
            //            switch (request.MonthlyRequest.MonthlyType)
            //            {
            //                case MonthlyType.NotSet:
            //                    break;
            //                case MonthlyType.Monthly:
            //                    {
            //                        request.SchedulerIntervals.StartDate = values.EndDate.AddMonths(request.MonthlyRequest.MonthlyPattern.EveryXMonths);
            //                        break;
            //                    }
            //                case MonthlyType.MonthlyRelative:
            //                    {
            //                        request.SchedulerIntervals.StartDate = values.EndDate.AddMonths(request.MonthlyRequest.MonthlyRelativePattern.EveryXMonths);
            //                        break;
            //                    }
            //                default:
            //                    break;
            //            }
                        
            //            break;
            //        }
            //    case RecurrenceType.Yearly:
            //        {
            //            switch (request.YearlyRequest.YearlyType)
            //            {
            //                case YearlyType.NotSet:
            //                    break;
            //                case YearlyType.Yearly:
            //                    {
            //                        DateTime newStartDate = new DateTime(values.EndDate.Year, 1, 1).AddYears(request.YearlyRequest.YearlyPattern.EveryXYears);
            //                        request.SchedulerIntervals.StartDate = newStartDate;
            //                        break;
            //                    }
            //                case YearlyType.YearlyRelative:
            //                    {
            //                        DateTime newStartDate = new DateTime(values.EndDate.Year, 1, 1).AddYears(request.YearlyRequest.YearlyRelativePattern.EveryXYears);
            //                        request.SchedulerIntervals.StartDate = newStartDate;
            //                            //values.EndDate.AddYears(request.YearlyRequest.CombinedYearlyPattern.EveryXYears);
            //                        break;
            //                    }
            //            }
            //            //
            //            break;
            //        }
            //}
            //resp.NextStartDateRequest = request.SchedulerIntervals.StartDate;
            //resp.NextEndDateRequest = request.SchedulerIntervals.EndDate;
            //resp.RecurrencePattern = null;//request.ToXml();

            return resp;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Represent the request which contains all the possible properties used by th eScheduler logic classes
    /// </summary>
    public class SchedulerRequest
    {
        public RecurrenceType rec { get; set; }

        /// <summary>
        /// RecurrenceType { NotSet=0, Daily=1,Weekly=4,Monthly =8, Yearly = 16}
        /// </summary>
        public RecurrenceType RecurrencePattern 
        {
            get { return rec; }
            set { rec = value; } 
        }

        /// <summary>
        /// Upper Recurrence limit is 1100
        /// </summary>
        public int UpperRecurrenceLimit { get; set; }

        /// <summary>
        /// The date time interval and the recurrence pattern
        /// </summary>
        public SchedulerIntervals SchedulerIntervals = new SchedulerIntervals();

        private DailyRequest dailyRequest;
        /// <summary>
        /// Contains the Daily request properties
        /// </summary>
        public DailyRequest DailyRequest 
        {
            get { return dailyRequest; }
            set { dailyRequest = value; RecurrencePattern = RecurrenceType.Daily; } 
        }


        private WeeklyRequest weeklyRequest;
        /// <summary>
        /// Contains the Weekly request properties
        /// </summary>
        public WeeklyRequest WeeklyRequest
        {
            get { return weeklyRequest; }
            set { weeklyRequest = value; RecurrencePattern = RecurrenceType.Weekly; }
        }

        private MonthlyRequest monthlyRequest;
        /// <summary>
        /// Contains the Monthly request properties
        /// </summary>
        public MonthlyRequest MonthlyRequest
        {
            get { return monthlyRequest; }
            set { monthlyRequest = value; RecurrencePattern = RecurrenceType.Monthly; }
        }

        private YearlyRequest yearlyRequest;
        /// <summary>
        /// Contains the Yearly request properties
        /// </summary>
        public YearlyRequest YearlyRequest
        {
            get { return yearlyRequest; }
            set { yearlyRequest = value; RecurrencePattern = RecurrenceType.Yearly; }
        }


        
    }
}

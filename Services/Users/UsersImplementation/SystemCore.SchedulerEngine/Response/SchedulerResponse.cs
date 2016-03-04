using System;
using System.Collections.Generic;

namespace Scheduler.DataRequest
{
    /// <summary>
    /// Class used to pass the recurrence values back to developer
    /// </summary>
    public class SchedulerResponse
    {
        List<DateTime> values = new List<DateTime>() ;
        
        /// <summary>
        /// List of Date Time values
        /// </summary>
        public List<DateTime> Values
        {
            get
            {
                return values;
            }
            set { values = value; }
        }

        /// <summary>
        /// Last date from the values
        /// </summary>
        public DateTime LastDate
        {
          	get
            {
                if (values.Count > 0)
                    return values[values.Count - 1];
                else
                    return DateTime.MaxValue;
            }
        }
        public DateTime StartDate
        {
            get
            {
                if (values.Count > 0)
                    return values[0];
                else
                    return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Next Generate Date represent the date when the developer must call again the Scheduler  functionality
        /// </summary>
        //public DateTime NextStartDateRequest { get; set; }
        //public DateTime? NextEndDateRequest { get; set; }
        

        //public string RecurrencePattern { get; set; }
    }
}

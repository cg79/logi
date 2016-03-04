using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Scheduler Intervals used to get the developer values and pass to the Scheduler logic 
    /// </summary>
    public class SchedulerIntervals
    {
        /// <summary>
        ///  POssible values are NotSet = 0, OccurenceNumber = 1, EndDate = 2
        /// </summary>
        public IntervalPattern IntervalPattern { get; set; }


        /// <summary>
        /// start date
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// maximum number of results
        /// </summary>
        public int MaxOccurencesNo { get; set; }

        /// <summary>
        /// the end date 
        /// </summary>
        public DateTime? EndDate { get; set; }

        private string dateFormat = "mm/dd/yyyy";
        public string DateFormat 
        { 
            get{return dateFormat;} 
            set{dateFormat=value;} 
        }
    }
}

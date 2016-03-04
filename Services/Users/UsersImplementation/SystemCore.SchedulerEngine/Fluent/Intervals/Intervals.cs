using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;


namespace SchedulerEngine.Request
{
    /// <summary>
    /// Represent the request settings intervals; Start Date; End Date and the IntervalPattern
    /// </summary>
    public class SchedulerRequestSettings
    {
        protected IntervalPattern intervalPattern = IntervalPattern.OccurenceNumber;
        
        /// <summary>
        /// represent the request type of the interval
        /// IntervalPattern { NotSet = 0, OccurenceNumber = 1, EndDate = 2 }
        /// </summary>
        /// <param name="intervalPattern"></param>
        /// <returns></returns>
        public SchedulerRequestSettings IntervalType(IntervalPattern intervalPattern)
        {
            this.intervalPattern = intervalPattern;
            return this;
        }

        /// <summary>
        /// start date
        /// </summary>
        protected DateTime startDate { get; set; }
        /// <summary>
        /// Represent the Start Date value
        /// This value is mandatory
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public SchedulerRequestSettings StartDate(DateTime startDate)
        {
            this.startDate = startDate;
            return this;
        }

        /// <summary>
        /// Maximum number of results
        /// the default value is 1100
        /// </summary>
        protected int maxOccurencesNo { get; set; }
        public SchedulerRequestSettings MaxOccurencesNo(int maxOccurencesNo)
        {
            this.maxOccurencesNo = maxOccurencesNo;
            return this;
        }

        /// <summary>
        /// The end date of the request;
        /// It is used only if the request type is By End Date
        /// </summary>
        protected DateTime? endDate { get; set; }
        public SchedulerRequestSettings EndDate(DateTime? endDate)
        {
            this.endDate = endDate;
            return this;
        }

    }
}

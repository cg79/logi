using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler.DataRequest;


namespace SchedulerEngine.Request
{
    /// <summary>
    /// Class used only to read the properties set by developer
    /// This class is internal
    /// </summary>
    public class IntervalsWrapper:SchedulerRequestSettings
    {
        /// <summary>
        /// StartDateValue
        /// </summary>
        public DateTime StartDateValue { get { return startDate; } }
        
        /// <summary>
        /// End Date value
        /// </summary>
        public DateTime? EndDateValue { get { return endDate; } }
        
        /// <summary>
        /// MaxOccurencesNoValue
        /// </summary>
        public int MaxOccurencesNoValue { get { return maxOccurencesNo; } }

        /// <summary>
        /// IntervalPattern
        /// </summary>
        public IntervalPattern IntervalPatternValue { get { return intervalPattern; } }
    }
}

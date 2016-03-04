using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Represent a Monthly request
    /// </summary>
    public class MonthlyRequest
    {
        /// <summary>
        /// Monthly type
        /// public enum MonthlyType { NotSet = -1, Monthly = 16, MonthlyRelative = 32 };
        /// </summary>
        public MonthlyType MonthlyType { get; set; }
        /// <summary>
        /// Contains the Monthly relative values
        /// </summary>
        public MonthlyRelativePattern MonthlyRelativePattern { get; set; }

        /// <summary>
        /// contains the monthly request values
        /// </summary>
        public MonthlyPattern MonthlyPattern { get; set; }
    }
}

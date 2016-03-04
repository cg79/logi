using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Contains the proeprties for a MOnthly relative request
    /// </summary>
    public class MonthlyRelativePattern
    {
        private int everyXMonths = 1;
        /// <summary>
        /// Every X Months
        /// </summary>
        public int EveryXMonths
        {
            get { return everyXMonths; }
            set 
            { 
                everyXMonths = value;
                if (everyXMonths == 0)
                {
                    everyXMonths = 1;
                }
            }
        }

        /// <summary>
        /// First part of a custom date. This would be First, Second, etc. item of the month.
        /// </summary>
        public FrequencyRelativeInterval FrequencyRelative { get; set; }


        /// <summary>
        /// Second part of a custom date. This is day of week, weekend day, etc.
        /// </summary>
        public FrequencyDaysValue FrequencyDaysValue { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Class used internally to pass the Daily information to the Scheduler.Logic
    /// </summary>
    public class DailyRequest 
    {
        private int everyXDays = 1;

        /// <summary>
        /// Represent the days interval value 
        /// </summary>
        public int EveryXDays 
        {
            get { return everyXDays; }
            set 
            { 
                everyXDays = value;
                if (everyXDays == 0)
                {
                    everyXDays = 1;
                }
            }
        }

        /// <summary>
        /// Represent the daily Recurrence type
        /// DailyType { NotSet = -1, OnEveryXDays = 1, OnEveryWeekday = 4 ,}
        /// </summary>
        public DailyType DailyReccurenceType { get; set; }

    }
}
 
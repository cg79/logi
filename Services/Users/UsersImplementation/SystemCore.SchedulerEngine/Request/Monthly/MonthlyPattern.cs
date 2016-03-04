using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.DataRequest
{
    /// <summary>
    /// Contains the properties for a Monthly Request
    /// </summary>
    public class MonthlyPattern
    {
        private int dayNo = 1;
        /// <summary>
        /// Day No
        /// </summary>
        public int DayNo
        {
            get { return dayNo; }
            set { dayNo = value; }
        }

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
    }
}

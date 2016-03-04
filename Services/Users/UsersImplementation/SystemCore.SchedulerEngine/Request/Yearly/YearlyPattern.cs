using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Contains the input request values for a yearly request
    /// </summary>
    public class YearlyPattern
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
        /// <summary>
        /// Month value
        /// </summary>
        public FrequencyMonthValue Month { get; set; }

        /// <summary>
        /// Every X Years
        /// </summary>
        public int EveryXYears { get; set; }
    }
}

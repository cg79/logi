using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Contains the properties for a yearly relative request
    /// </summary>
    public class YearlyRelativePattern
    {
        /// <summary>
        /// Frequency relative
        /// FrequencyRelativeInterval {  First=1, Second=2, Third=4, Fourth=8, Fifth = 16,Last=32 };
        /// </summary>
        public FrequencyRelativeInterval FrequencyRelative { get; set; }

        /// <summary>
        /// FrequencyDaysValue
        /// FrequencyDaysValue {  Sunday = 1, Monday =2, Tuesday=3, Wednesday=4, Thursday=5, Friday=6, Saturday=7, Day=8, Weekday=9, WeekendDay=10, };
        /// </summary>
        public FrequencyDaysValue FrequencyDayValue { get; set; }

        /// <summary>
        /// FrequencyMonthValue {  January = 1, February, March, April, May, June, July, August, September, October, November, December };
        /// </summary>
        public FrequencyMonthValue FrequencyMonthValue { get; set; }

        /// <summary>
        /// Every X Years
        /// </summary>
        public int EveryXYears { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    /// <summary>
    /// Contains the properties for a weekly request
    /// </summary>
    public class WeeklyRequest 
    {
        /// <summary>
        /// Selected days of week values
        /// </summary>
        public SelectedDayOfWeekValues SelectedDayOfWeekValues;

        private int everyXWeeks = 1;
        /// <summary>
        /// Every X Weeks
        /// </summary>
        public int EveryXWeeks
        {
            get { return everyXWeeks; }
            set { everyXWeeks = value; }
        }
    }
}

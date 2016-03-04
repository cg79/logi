using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler.DataRequest
{
    /// <summary>
    /// Enumeration used by the scheduler logic classes in order to return the recurring dates
    /// </summary>
    public enum RecurrenceType { NotSet=0, Daily=1,Weekly=4,Monthly =8, Yearly = 16}

    /// <summary>
    /// Enumeration used to pass the recurring interval to the Sceduler.Logic
    /// </summary>
    public enum IntervalPattern { NotSet = 0, OccurenceNumber = 1, EndDate = 2 }
    

    /// <summary>
    /// Enumeration which contains the frequency values
    /// Contains all the possible scheduler request types
    /// </summary>
    public enum FrequenceType { Once = 1,Daily = 4 ,Weekly=8,Monthly=16, MonthlyRelative=32,
                            Yearly = 64, YearlyRelative = 128   }


    #region Daily Settings
    /// <summary>
    /// Enumeration containing the possible daily request
    /// </summary>
    public enum DailyType { NotSet = -1, OnEveryXDays = 1, OnEveryWeekday = 4 ,};

    /// <summary>
    /// Enumeration of the days and it is used only for a weekly request
    /// </summary>
    public enum DayEnum
    {
        Monday =0,
        Tuesday =1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday =5,
        Sunday =6,
    }
    #endregion

    #region Weekly Settings
    /// <summary>
    /// The Regeneration type. 
    /// </summary>
    public enum WeeklyType { NotSet = -1, OnEveryXWeeks };

    /// <summary>
    /// Structure used to store the selected days for a weekly request
    /// </summary>
    public struct SelectedDayOfWeekValues
    {

        public bool Sunday;
        public bool Monday;
        public bool Tuesday;
        public bool Wednesday;
        public bool Thursday;
        public bool Friday;
        public bool Saturday;
    }
    #endregion

    #region Monthly Settings

    /// <summary>
    /// Contains the possible values for a monthly request
    /// MonthlyType { NotSet = -1, Monthly = 16, MonthlyRelative = 32 }
    /// </summary>
    public enum MonthlyType { NotSet = -1, Monthly = 16, MonthlyRelative = 32 };
    
    /// <summary>
    /// Contains the relative criteria for a MOnthly or Yearly request. This would be First, Second, etc. item of the month.
    /// </summary>
    public enum FrequencyRelativeInterval {  First=1, Second=2, Third=4, Fourth=8, Fifth = 16,Last=32 };
    /// <summary>
    /// Second part of a custom date. This is day of week, weekend day, etc.
    /// </summary>
    public enum FrequencyDaysValue {  Sunday = 1, Monday =2, Tuesday=3, Wednesday=4, Thursday=5, Friday=6, Saturday=7, Day=8, Weekday=9, WeekendDay=10, };
    #endregion

    #region Yearly Settings

    /// <summary>
    /// Represent the Yearly type sent to the Scheduler.Logic
    /// </summary>
    public enum YearlyType { NotSet = -1, Yearly = 64, YearlyRelative = 128 };
   
    
    /// <summary>
    /// Used for a yearly request. This is the Month of the year for which the custom date confines to..
    /// The value of this enum matches the ordinal position of the given month. So Jan = 1, Feb = 2, etc.
    /// </summary>
    public enum FrequencyMonthValue {  January = 1, February, March, April, May, June, July, August, September, October, November, December };

    #endregion

}

using System;

using Scheduler.DataRequest;

namespace Scheduler.Logic
{
 
    /// <summary>
    /// Represent the logic behind the Yearly Recurrence request
    /// </summary>
    internal class YearlyRecurrenceSettings : RecurrenceSettings
    {
        #region Constructors
        /// <summary>
        /// Get dates by Start date only. This is for no ending date values.
        /// </summary>
        /// <param name="startDate"></param>
        public YearlyRecurrenceSettings(DateTime startDate, int upperRecurrenceLimit) : base(startDate,upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start and End date boundries.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public YearlyRecurrenceSettings(DateTime startDate, DateTime endDate, int upperRecurrenceLimit) : base(startDate, endDate,upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start date and number of occurrences.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="numberOfOccurrences"></param>
        public YearlyRecurrenceSettings(DateTime startDate, int numberOfOccurrences, int upperRecurrenceLimit) : base(startDate, numberOfOccurrences,upperRecurrenceLimit) { }
        #endregion
        
        #region Private Fields
        int regenerateOnSpecificDateDayValue;
        int regenerateOnSpecificDateMonthValue;
        int adjustmentValue;
        DateTime nextDateValue;
        YearlyType regenType = YearlyType.NotSet;
        FrequencyRelativeInterval specificDatePartOne = FrequencyRelativeInterval.First;

        FrequencyDaysValue specificDatePartTwo = FrequencyDaysValue.Monday;
        FrequencyMonthValue specificDatePartThree = FrequencyMonthValue.January;
        int everyXYears = 1;
        #endregion

        #region Public GetValues
        /// <summary>
        /// Get dates for a specific day and month of the year.
        /// </summary>
        /// <param name="specificDateDayValue">Day of the month.</param>
        /// <param name="specificDateMonthValue">Month of the year.</param>
        /// <returns></returns>
        public RecurrenceValues GetValues(int specificDateDayValue, int specificDateMonthValue, int xYears)
        {
            regenerateOnSpecificDateDayValue = specificDateDayValue;
            regenerateOnSpecificDateMonthValue = specificDateMonthValue;
            everyXYears = xYears;
            if (everyXYears < 1)
            {
                everyXYears = 1;
            }
            regenType = YearlyType.Yearly;
            return GetValues();
        }

        /// <summary>
        /// Get dates for a custom formatted date such as First weekend day of July.
        /// </summary>
        /// <param name="customDateFirstPart"></param>
        /// <param name="customDateSecondPart"></param>
        /// <param name="customDateThirdPart"></param>
        /// <returns></returns>
        public RecurrenceValues GetValues(FrequencyRelativeInterval customDateFirstPart, FrequencyDaysValue customDateSecondPart, FrequencyMonthValue customDateThirdPart, int xYears)
        {
            specificDatePartOne = customDateFirstPart;
            specificDatePartTwo = customDateSecondPart;
            specificDatePartThree = customDateThirdPart;
            regenType = YearlyType.YearlyRelative;
            everyXYears = xYears;
            if (everyXYears < 1)
            {
                everyXYears = 1;
            }
            return GetValues();
        }
#endregion //Public GetValues

        #region Internal GetValues
        /// <summary>
        ///     Final overloaded function that gets the Recurrence Values. 
        ///     This is called from the RecurrenceHelper staic methods only.
        /// </summary>
        /// <returns>
        ///     A RecurrenceGenerator.RecurrenceValues value...
        /// </returns>
        internal override RecurrenceValues GetValues()
        {
            return GetRecurrenceValues();
        }
#endregion //Internal GetValues

       

        internal override RecurrenceValues GetValues(DateTime startDate, DateTime endDate)
        {
            base.StartDate = startDate;
            base.EndDate = endDate;
            // Change the end type to End Date as this original series info
            // may have been set to number of occurrences.
            base.endDateType = EndDateType.SpecificDate;
            return GetRecurrenceValues();
        }
        internal override RecurrenceValues GetValues(DateTime startDate, int numberOfOccurrences)
        {
            base.NumberOfOccurrences = numberOfOccurrences;
            base.StartDate = startDate;
            // Change the end type to number of occurrences. 
            // This must be set because the original starting Series Info may
            // be set to have an End Date type.
            base.endDateType = EndDateType.NumberOfOccurrences;
            return GetRecurrenceValues();
        }

        RecurrenceValues GetRecurrenceValues()
        {
            RecurrenceValues values = null;
            switch (RegenType)
            {
                case YearlyType.Yearly:
                    values = GetSpecificDayOfYearDates();
                    break;

                case YearlyType.YearlyRelative:
                    values = GetCustomDayOfYearDates();
                    break;

                

            }
            if (values.Values.Count > 0)
            {
                values.SetStartDate(values.Values[0]);

                // Get the end date if not open-ended
                if (base.TypeOfEndDate != EndDateType.NotDefined)
                    values.SetEndDate(values.Values[values.Values.Count - 1]);
            }
            
            return values;
        }
        #region Internal Procedures
       

        void SetValues(int specificDateDayValue, int specificDateMonthValue)
        {
            regenerateOnSpecificDateDayValue = specificDateDayValue;
            regenerateOnSpecificDateMonthValue = specificDateMonthValue;
            regenType = YearlyType.Yearly;
        }

        void SetValues(FrequencyRelativeInterval customDateFirstPart, FrequencyDaysValue customDateSecondPart, FrequencyMonthValue customDateThirdPart)
        {
            specificDatePartOne = customDateFirstPart;
            specificDatePartTwo = customDateSecondPart;
            specificDatePartThree = customDateThirdPart;
            regenType = YearlyType.YearlyRelative;
        }

      

       


#endregion //Internal Procedures

        #region Private Procedures
       

        /// <summary>
        /// Get recurring dates from a specific constant date such as 27 July.
        /// </summary>
        /// <returns></returns>
        RecurrenceValues GetSpecificDayOfYearDates()
        {
            RecurrenceValues values = new RecurrenceValues();
            DateTime dt = base.StartDate;
            int dayValue = regenerateOnSpecificDateDayValue;
            int daysOfMonth = DateTime.DaysInMonth(dt.Year, regenerateOnSpecificDateMonthValue);
            // Get the max days of the month and make sure it's not 
            // less than the specified day value trying to be set.
            if (daysOfMonth < regenerateOnSpecificDateDayValue)
                dayValue = daysOfMonth;

            // Determine if start date is greater than the Day and Month values
            // for a specific date.
            DateTime newDate = new DateTime(dt.Year, regenerateOnSpecificDateMonthValue, dayValue);
            // Is the specific date before the start date, if so 
            // then make the specific date next year.
            if (newDate < dt)
                dt = newDate.AddYears(1);
            else
                dt = newDate;


            switch (base.TypeOfEndDate)
            {


                case EndDateType.NumberOfOccurrences:
                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        values.AddDateValue(GetCorrectedDate(dt.AddYears(i * everyXYears)), adjustmentValue);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            values.AddDateValue(dt, adjustmentValue);
                            dt = dt.AddYears(everyXYears);
                            dt = GetCorrectedDate(dt);
                            counter++;
                        } while (dt <= base.EndDate && counter<base.NumberOfOccurrences);
                        break;
                    }
                default:
                    throw new ArgumentNullException("TypeOfEndDate", "The TypeOfEndDate property has not been set.");
            }
            

            return values;
        }

        /// <summary>
        /// Correct an input date to be equal to or less than the specified day value.
        /// </summary>
        /// <param name="input">Date to check to ensure it matches the specified day value or the max number of days for that month, whichever comes first.</param>
        /// <returns>DateTime</returns>
        DateTime GetCorrectedDate(DateTime input)
        {
            DateTime dt = input;
            // Ensure the day value hasn't changed.
            // This will occurr if the month is Feb. All
            // dates after that will have the same day.
            if (dt.Day < this.regenerateOnSpecificDateDayValue && DateTime.DaysInMonth(dt.Year, dt.Month) > dt.Day)
            {
                // The Specified day is greater than the number of days in the month.
                if (this.regenerateOnSpecificDateDayValue > DateTime.DaysInMonth(dt.Year, dt.Month))
                    dt = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
                else
                    // The specified date is less than number of days in month.
                    dt = new DateTime(dt.Year, dt.Month, this.regenerateOnSpecificDateDayValue);
            }
            return dt;
        }

        /// <summary>
        /// Get recurring dates from custom date such as First Sunday of July.
        /// </summary>
        /// <returns></returns>
        RecurrenceValues GetCustomDayOfYearDates()
        {

            RecurrenceValues values = new RecurrenceValues();
            DateTime dt = base.StartDate;
            int year = dt.Year;


            switch (base.TypeOfEndDate)
            {

                case EndDateType.NumberOfOccurrences:
                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        dt = GetCustomDate(year);
                        // If the date returned is less than the start date
                        // then do it again to increment past the start date
                        if (dt < base.StartDate)
                        {
                            year++;
                            dt = GetCustomDate(year);
                        }
                        year = dt.Year + everyXYears;
                        values.AddDateValue(dt, adjustmentValue);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            dt = GetCustomDate(year);
                            // If the date returned is less than the start date
                            // then do it again to increment past the start date
                            if (dt < base.StartDate)
                            {
                                year++;
                                dt = GetCustomDate(year);
                            }
                            year = year + everyXYears;
                            values.AddDateValue(dt, adjustmentValue);
                            counter++;
                        } while (dt <= base.EndDate && counter < base.NumberOfOccurrences);
                        break;
                    }
                default:
                    throw new ArgumentNullException("TypeOfEndDate", "The TypeOfEndDate property has not been set.");

            }
            return values;

        }

        /// <summary>
        /// Get the custom value from the 1st, 2nd, and 3rd custom date parts
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime GetCustomDate(int year)
        {
            DateTime dt = new DateTime(year, (int)FrequencyMonthValue, 1);
            DateTime initialDate = dt;
            int day = 1;
            int firstPart = 0;
            switch (FrequencyRelative)
            {

                case FrequencyRelativeInterval.First:
                    {
                        firstPart = 1;
                        break;
                    }
                case FrequencyRelativeInterval.Second:
                    {
                        firstPart = 2;
                        break;
                    }
                case FrequencyRelativeInterval.Third:
                    {
                        firstPart = 3;
                        break;
                    }
                case FrequencyRelativeInterval.Fourth:
                    {
                        firstPart = 4;
                        break;
                    }
                case FrequencyRelativeInterval.Fifth:
                    {
                        firstPart = 5;
                        break;
                    }
                case FrequencyRelativeInterval.Last:
                    {
                        firstPart = 6;
                        break;
                    }
            }
            //(int)FrequencyRelative + 1;
            bool thereIsTheFifth = false;
            int daysOfMonth = DateTime.DaysInMonth(year, dt.Month);

            switch (FrequencyDaysValue)
            {
                case FrequencyDaysValue.Day:
                    // If only getting the Last day of the month
                    if (FrequencyRelative == FrequencyRelativeInterval.Last)
                        dt = new DateTime(year, dt.Month, DateTime.DaysInMonth(year, dt.Month));
                    else
                        // Get a specific day of the month such as First, Second, Third, Fourth
                        dt = new DateTime(year, dt.Month, firstPart);
                    
                    break;

                case FrequencyDaysValue.Weekday:
                    int weekDayCount = 0;
                    DateTime lastWeekday = dt;
                    do
                    {
                        // Check for anything other than Saturday and Sunday
                        if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                        {
                            // Get a specific Weekday of the Month
                            if (FrequencyRelative != FrequencyRelativeInterval.Last)
                            {
                                    // Add up the weekday count
                                    weekDayCount++;
                                // If the current weekday count matches then exit
                                if (weekDayCount == firstPart)
                                    break;
                            }
                            else
                            {
                                // Get the last weekday of the month
                                lastWeekday = dt;
                            }
                        }
                        dt = dt.AddDays(1);
                        day++;
                    } while (day <= daysOfMonth);

                    // If getting the last weekday of the month then 
                    // set the returning value to the last weekday found.
                    if (FrequencyRelative == FrequencyRelativeInterval.Last)
                        dt = lastWeekday;

                    break;

                case FrequencyDaysValue.WeekendDay:
                    int weekendDayCount = 0;
                    DateTime lastWeekendday = dt;
                    do
                    {
                        // Check for anything other than Saturday and Sunday
                        if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                        {
                            // Get a specific Weekday of the Month
                            if (FrequencyRelative != FrequencyRelativeInterval.Last)
                            {
                                // Add up the weekday count
                                weekendDayCount++;
                                // If the current weekday count matches then exit
                                if (weekendDayCount == firstPart)
                                    break;
                            }
                            else
                            {
                                // Get the last weekday of the month
                                lastWeekendday = dt;
                            }
                        }
                        dt = dt.AddDays(1);
                        day++;
                    } while (day <= daysOfMonth);

                    // If getting the last weekday of the month then 
                    // set the returning value to the last weekday found.
                    if (FrequencyRelative == FrequencyRelativeInterval.Last)
                        dt = lastWeekendday;

                    break;

                case FrequencyDaysValue.Monday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Monday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Monday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Tuesday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Tuesday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Tuesday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Wednesday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Wednesday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Wednesday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Thursday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Thursday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Thursday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Friday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Friday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Friday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Saturday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Saturday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Saturday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Sunday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Sunday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddYears(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Sunday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;
            }
            return dt;
        }

        DateTime GetCustomWeekday(DateTime startDate, DayOfWeek weekDay, int daysOfMonth, int firstDatePart, out bool thereIsTheFifth)
        {
            thereIsTheFifth = (FrequencyRelative != FrequencyRelativeInterval.Fifth);
            int day = 1;
            int dayCount = 0;
            DateTime lastDOW = startDate;
            DateTime returnDate = startDate;
            do
            {
                // Check for day of the week
                if (returnDate.DayOfWeek == weekDay)
                {
                    // Get a specific Weekday of the Month
                    if (FrequencyRelative != FrequencyRelativeInterval.Last)
                    {
                        // Add up the days found count
                        dayCount++;
                        // If the current weekday count matches then exit
                        if (dayCount == firstDatePart)
                        {
                            thereIsTheFifth = true;
                            break;
                        }
                    }
                    else
                    {
                        // Get the current date value
                        lastDOW = returnDate;
                    }
                }
                returnDate = returnDate.AddDays(1);
                day++;
            } while (day <= daysOfMonth);

            // If getting the last weekday of the month then 
            // set the returning value to the last weekday found.
            if (FrequencyRelative == FrequencyRelativeInterval.Last)
                returnDate = lastDOW;

            return returnDate;
        }
        int GetDatePartOneValue()
        {
            int val = 0;
            switch (FrequencyRelative)
            {
                case FrequencyRelativeInterval.First:
                    val = 1;
                    break;
                case FrequencyRelativeInterval.Second:
                    val = 2;
                    break;
                case FrequencyRelativeInterval.Third:
                    val = 3;
                    break;
                case FrequencyRelativeInterval.Fourth:
                    val = 4;
                    break;
                case FrequencyRelativeInterval.Fifth:
                    val = 5;
                    break;
            }
            return val;
        }
#endregion //Private Procedures

        #region Public Fields
        /// <summary>
        /// Part of the Custom date that equates to the month of year.
        /// </summary>
        public FrequencyMonthValue FrequencyMonthValue
        {
            get
            {
                return specificDatePartThree;
            }
        }

        /// <summary>
        /// Part of the Custom date that is the day part of the month such as weekend day, Tuesday, Wednesday, weekday, etc.
        /// </summary>
        public FrequencyDaysValue FrequencyDaysValue
        {
            get
            {
                return specificDatePartTwo;
            }
        }

        /// <summary>
        /// Part of the Custom date that is the part to get such as First, Second, Last, etc.
        /// </summary>
        public FrequencyRelativeInterval FrequencyRelative
        {
            get
            {
                return specificDatePartOne;
            }
        }

        /// <summary>
        /// Regeneration type such as by Specific day of the year, Custom date, etc.
        /// </summary>
        public YearlyType RegenType
        {
            get
            {
                return regenType;
            }
        }

        /// <summary>
        /// What day of the month do you want to regenerate dates when by a specific day of the year.
        /// </summary>
        public int RegenerateOnSpecificDateDayValue
        {
            get
            {
                return regenerateOnSpecificDateDayValue;
            }
        }

        /// <summary>
        /// What month of the year do you want to regenerate dates when by a specific day of the year.
        /// </summary>
        public int RegenerateOnSpecificDateMonthValue
        {
            get
            {
                return regenerateOnSpecificDateMonthValue;
            }
        }

        /// <summary>
        /// Used to adjust the date plus/minus x-days
        /// </summary>
        public int AdjustmentValue
        {
            get
            {
                return adjustmentValue;
            }
            set
            {
                adjustmentValue = value;
            }
        }
#endregion //Public Fields
        
    }
}

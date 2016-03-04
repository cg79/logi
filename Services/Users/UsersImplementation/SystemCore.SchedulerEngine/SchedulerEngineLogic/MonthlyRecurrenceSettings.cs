using System;

using Scheduler.DataRequest;

namespace Scheduler.Logic
{
   /// <summary>
    /// Represent the logic behind the  Monthly Recurrence request
   /// </summary>
    internal class MonthlyRecurrenceSettings : RecurrenceSettings
    {
        #region Constructors
        /// <summary>
        /// Get dates by Start date only. This is for no ending date values.
        /// </summary>
        /// <param name="startDate"></param>
        public MonthlyRecurrenceSettings(DateTime startDate, int upperRecurrenceLimit) : base(startDate, upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start and End date boundries.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public MonthlyRecurrenceSettings(DateTime startDate, DateTime endDate, int upperRecurrenceLimit) : base(startDate, endDate, upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start date and number of occurrences.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="numberOfOccurrences"></param>
        public MonthlyRecurrenceSettings(DateTime startDate, int numberOfOccurrences, int upperRecurrenceLimit) : base(startDate, numberOfOccurrences, upperRecurrenceLimit) { }
        #endregion

        #region Private Fields
        int regenerateOnSpecificDateDayValue;
        int regenEveryXMonths = 1;
        int adjustmentValue;
        DateTime nextDateValue;
        MonthlyType regenType = MonthlyType.NotSet;
        FrequencyRelativeInterval specificDatePartOne = FrequencyRelativeInterval.First;
        FrequencyDaysValue specificDatePartTwo = FrequencyDaysValue.Monday;
        #endregion

        #region Private Procedures
        


        /// <summary>
        /// Get recurring dates from a specific constant date such as 27 July.
        /// </summary>
        /// <returns></returns>
        RecurrenceValues GetSpecificDayOfMonthDates()
        {
            RecurrenceValues values = new RecurrenceValues();
            DateTime dt = base.StartDate;
            int dayValue = regenerateOnSpecificDateDayValue;
            int daysOfMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
            // Get the max days of the month and make sure it's not 
            // less than the specified day value trying to be set.
            if (daysOfMonth < regenerateOnSpecificDateDayValue)
                dayValue = daysOfMonth;

            // Determine if start date is greater than the Day and Month values
            // for a specific date.
            DateTime newDate = new DateTime(dt.Year, dt.Month, dayValue);
            // Is the specific date before the start date, if so 
            // then make the specific date next month.
            if (newDate < dt)
                dt = newDate.AddMonths(1);
            else
                dt = newDate;



            switch (base.TypeOfEndDate)
            {


                case EndDateType.NumberOfOccurrences:

                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        values.AddDateValue(dt, adjustmentValue);
                        dt = dt.AddMonths(RegenEveryXMonths);
                        dt = GetCorrectedDate(dt);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            values.AddDateValue(dt, adjustmentValue);
                            dt = dt.AddMonths(RegenEveryXMonths);
                            dt = GetCorrectedDate(dt);
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
        RecurrenceValues GetCustomDayOfMonthDates()
        {
            

            RecurrenceValues values = new RecurrenceValues();
            DateTime dt = base.StartDate;


            switch (base.TypeOfEndDate)
            {


                case EndDateType.NumberOfOccurrences:
                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        dt = GetCustomDate(dt);
                        // If the date returned is less than the start date
                        // then do it again to increment past the start date
                        if (dt < base.StartDate)
                        {
                            dt = dt.AddMonths(1);
                            dt = GetCustomDate(dt);
                        }
                        values.AddDateValue(dt, adjustmentValue);
                        dt = dt.AddMonths(RegenEveryXMonths);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            dt = GetCustomDate(dt);
                            // If the date returned is less than the start date
                            // then do it again to increment past the start date
                            if (dt < base.StartDate)
                            {
                                dt = dt.AddMonths(1);
                                dt = GetCustomDate(dt);
                            }
                            values.AddDateValue(dt, adjustmentValue);
                            dt = dt.AddMonths(RegenEveryXMonths);
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
        DateTime GetCustomDate(DateTime currentDate)
        {
            
            int year = currentDate.Year;
            DateTime dt = new DateTime(year, currentDate.Month, 1);
            DateTime initialDate = dt;
            int day = 1;

            int firstPart = 0;

            switch (SpecificDatePartOne)
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
                default:
                    break;
            }
            //(int)SpecificDatePartOne + 1;
            bool thereIsTheFifth = false;


            int daysOfMonth = DateTime.DaysInMonth(year, dt.Month);

            switch (SpecificDatePartTwo)
            {
                case FrequencyDaysValue.Day:
                    // If only getting the Last day of the month
                    if (SpecificDatePartOne == FrequencyRelativeInterval.Last)
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
                            if (SpecificDatePartOne != FrequencyRelativeInterval.Last)
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
                    if (SpecificDatePartOne == FrequencyRelativeInterval.Last)
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
                            if (SpecificDatePartOne != FrequencyRelativeInterval.Last)
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
                    if (SpecificDatePartOne == FrequencyRelativeInterval.Last)
                        dt = lastWeekendday;

                    break;

                case FrequencyDaysValue.Monday:

                    dt = GetCustomWeekday(dt, DayOfWeek.Monday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Monday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Tuesday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Tuesday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Tuesday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Wednesday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Wednesday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Wednesday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Thursday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Thursday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Thursday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Friday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Friday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Friday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Saturday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Saturday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Saturday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;

                case FrequencyDaysValue.Sunday:
                    dt = GetCustomWeekday(dt, DayOfWeek.Sunday, daysOfMonth, firstPart, out thereIsTheFifth);
                    while (!thereIsTheFifth)
                    {
                        initialDate = initialDate.AddMonths(1);
                        dt = GetCustomWeekday(initialDate, DayOfWeek.Sunday, DateTime.DaysInMonth(initialDate.Year, initialDate.Month), firstPart, out thereIsTheFifth);
                    }
                    break;
            }
            return dt;
        }

        DateTime GetCustomWeekday(DateTime startDate, DayOfWeek weekDay, int daysOfMonth, int firstDatePart, out bool thereIsTheFifth)
        {
            thereIsTheFifth = (SpecificDatePartOne != FrequencyRelativeInterval.Fifth);
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
                    if (SpecificDatePartOne != FrequencyRelativeInterval.Last)
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
            if (SpecificDatePartOne == FrequencyRelativeInterval.Last)
                returnDate = lastDOW;

            return returnDate;
        }
        #endregion //Private Procedures

      
        #region Public GetValues
        /// <summary>
        /// Get Custom dates such as Last Saturday of the month with option as to the increment of every x-months.
        /// </summary>
        /// <param name="customDatePartOne">Corresponds to Part of month such as First, Last.</param>
        /// <param name="customDatePartTwo">Corresponds to day of the week to get such as Tuesday or Weekend Day.</param>
        /// <param name="regenEveryXMonths">How many months to skip, such as 2 for every other month.</param>
        /// <returns></returns>
        public RecurrenceValues GetValues(FrequencyRelativeInterval customDatePartOne, FrequencyDaysValue customDatePartTwo, int regenEveryXMonths)
        {
            this.regenEveryXMonths = regenEveryXMonths;
            specificDatePartOne = customDatePartOne;
            specificDatePartTwo = customDatePartTwo;
            regenType = MonthlyType.MonthlyRelative;
            return GetValues();
        }

        /// <summary>
        /// Get values for a specific day of the month. Eg. Every 23rd day. With option to get every x-month.
        /// </summary>
        /// <param name="dayOfMonthToRegen">Day of month you want to set as the value to get from month to month.</param>
        /// <param name="regenEveryXMonths">How many months to skip, such as 2 for every other month.</param>
        /// <returns></returns>
        public RecurrenceValues GetValues(int dayOfMonthToRegen, int regenEveryXMonths)
        {
            this.regenEveryXMonths = regenEveryXMonths;
            regenerateOnSpecificDateDayValue = dayOfMonthToRegen;
            regenType = MonthlyType.Monthly;
            return GetValues();
        }
#endregion //Public GetValues

        #region Public Fields
        /// <summary>
        /// What is the first part to the Custom date such as First, Last.
        /// </summary>
        public FrequencyRelativeInterval SpecificDatePartOne
        {
            get
            {
                return specificDatePartOne;
            }
        }

        /// <summary>
        /// What is the second part to the Custom date such as which weekday, weekend day, etc.
        /// </summary>
        public FrequencyDaysValue SpecificDatePartTwo
        {
            get
            {
                return specificDatePartTwo;
            }
        }

        /// <summary>
        /// What is the regeneration type such as Specific day of month, custom date, etc.
        /// </summary>
        public MonthlyType RegenType
        {
            get
            {
                return regenType;
            }
        }

        /// <summary>
        ///  Day of month to regenerate when RegenType = specific day of month.
        /// </summary>
        public int RegenerateOnSpecificDateDayValue
        {
            get
            {
                return regenerateOnSpecificDateDayValue;
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
        /// <summary>
        /// What is the interval to generate dates. This is used to skip months in the cycle.
        /// </summary>
        public int RegenEveryXMonths
        {
            get
            {
                return regenEveryXMonths;
            }
        }
        #endregion //Public Fields

        #region Internal GetValues
        /// <summary>
        ///      Final overloaded function that gets the Recurrence Values. 
        ///      This is called from the RecurrenceHelper staic methods only.
        /// </summary>
        /// <returns>
        ///     A RecurrenceGenerator.RecurrenceValues value...
        /// </returns>
        internal override RecurrenceValues GetValues()
        {
            return GetRecurrenceValues();
        }
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
                case MonthlyType.Monthly:
                    values = GetSpecificDayOfMonthDates();
                    break;

                case MonthlyType.MonthlyRelative:
                    values = GetCustomDayOfMonthDates();
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
#endregion //Internal GetValues
        
        #region Internal Procedures
       
        void SetValues(FrequencyRelativeInterval customDatePartOne, FrequencyDaysValue customDatePartTwo, int regenEveryXMonths)
        {
            this.regenEveryXMonths = regenEveryXMonths;
            specificDatePartOne = customDatePartOne;
            specificDatePartTwo = customDatePartTwo;
            regenType = MonthlyType.MonthlyRelative;
        }

        void SetValues(int dayOfMonthToRegen, int regenEveryXMonths)
        {
            this.regenEveryXMonths = regenEveryXMonths;
            regenerateOnSpecificDateDayValue = dayOfMonthToRegen;
            regenType = MonthlyType.Monthly;
        }


     
    


#endregion //Internal Procedures

        
        

    }
}

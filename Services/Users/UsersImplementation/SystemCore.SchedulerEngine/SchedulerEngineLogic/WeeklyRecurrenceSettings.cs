using System;

using Scheduler.DataRequest;

namespace Scheduler.Logic
{


    /// <summary>
    /// Represent the logic behind the Weekly Recurrence request
    /// </summary>
    internal class WeeklyRecurrenceSettings : RecurrenceSettings
    {
        #region Constructors
        /// <summary>
        /// Get dates by Start date only. This is for no ending date values.
        /// </summary>
        /// <param name="startDate"></param>
        public WeeklyRecurrenceSettings(DateTime startDate, int upperRecurrenceLimit) : base(startDate,upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start and End date boundries.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public WeeklyRecurrenceSettings(DateTime startDate, DateTime endDate, int upperRecurrenceLimit) : base(startDate, endDate, upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start date and number of occurrences.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="numberOfOccurrences"></param>
        public WeeklyRecurrenceSettings(DateTime startDate, int numberOfOccurrences, int upperRecurrenceLimit) : base(startDate, numberOfOccurrences, upperRecurrenceLimit) { }
        #endregion

        #region Private Fields
        
        WeeklyType regenType = WeeklyType.OnEveryXWeeks;
        SelectedDayOfWeekValues selectedDays;
        int regenEveryXWeeks;
        DateTime nextDateValue;
#endregion //Private Fields

        #region Public GetValues
        /// <summary>
        /// Get day values. This overload is for every x-weeks.
        /// </summary>
        /// <param name="regenEveryXDays">Interval of weeks. Every x-weeks.</param>
        /// <returns>RecurrenceValues</returns>
        public RecurrenceValues GetValues(int regenEveryXWeeks, SelectedDayOfWeekValues selectedDays)
        {
            this.regenEveryXWeeks = regenEveryXWeeks;
            regenType = WeeklyType.OnEveryXWeeks;
            this.selectedDays = selectedDays;
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
                case WeeklyType.OnEveryXWeeks:
                    values = GetEveryXWeeksValues();
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

        /// <summary>
        /// Set the values in preperation for getting the Next date in the series.
        /// </summary>
        /// <param name="regenEveryXWeeks">Value to regenerate the dates every x-weeks</param>
        /// <param name="selectedDays">Struct of days selected for the week.</param>
        void SetValues(int regenEveryXWeeks, SelectedDayOfWeekValues selectedDays)
        {
            this.regenEveryXWeeks = regenEveryXWeeks;
            regenType = WeeklyType.OnEveryXWeeks;
            this.selectedDays = selectedDays;
        }

       
       

     
        
#endregion //Internal Procedures
        
        #region Private Procedures
        

        RecurrenceValues GetEveryXWeeksValues()
        {
            RecurrenceValues values = new RecurrenceValues();
            DateTime dt = base.StartDate.AddDays(-1); // Backup a day so the first instance of GetNextDay will increment to the next day.


            switch (base.TypeOfEndDate)
            {

                case EndDateType.NumberOfOccurrences:

                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        dt = GetNextDay(dt);
                        values.AddDateValue(dt);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            dt = GetNextDay(dt);
                            // Handle for dates past the end date
                            if (dt > base.EndDate)
                                break;

                            values.AddDateValue(dt);
                            counter++;
                        } while (dt <= base.EndDate && counter < base.NumberOfOccurrences);
                        break;
                    }
                default:
                    throw new ArgumentNullException("TypeOfEndDate", "The TypeOfEndDate property has not been set.");
            }
            

            return values;
        }

        DateTime GetNextDay(DateTime input)
        {
            DateTime? returnDate = null;

            // Get the return date by incrementing the date
            // and checking the value against the selected days
            // of the week.
            do
            {
                input = input.AddDays(1);
                switch (input.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        if (selectedDays.Sunday)
                            returnDate = input;
                        break;
                    case DayOfWeek.Monday:
                        if (selectedDays.Monday)
                            returnDate = input;
                        break;
                    case DayOfWeek.Tuesday:
                        if (selectedDays.Tuesday)
                            returnDate = input;
                        break;
                    case DayOfWeek.Wednesday:
                        if (selectedDays.Wednesday)
                            returnDate = input;
                        break;
                    case DayOfWeek.Thursday:
                        if (selectedDays.Thursday)
                            returnDate = input;
                        break;
                    case DayOfWeek.Friday:
                        if (selectedDays.Friday)
                            returnDate = input;
                        break;
                    case DayOfWeek.Saturday:
                        if (selectedDays.Saturday)
                            returnDate = input;
                        else
                        {
                            // Increment by weeks if regenXWeeks has a value 
                            // greater than 1 which is default.
                            // But only increment if we've gone over
                            // at least 7 days already.
                            if (regenEveryXWeeks > 1 )
                                input = input.AddDays((regenEveryXWeeks -1) * 7);
                        }
                        break;
                }
            } while (!returnDate.HasValue);
            return returnDate.Value;
        }
#endregion //Private Procedures

        #region Public Fields
        /// <summary>
        /// What is the interval to generate dates. This is used to skip weeks in the cycle.
        /// </summary>
        public int RegenEveryXWeeks
        {
            get
            {
                return regenEveryXWeeks;
            }
        }

        public SelectedDayOfWeekValues SelectedDays
        {
            get
            {
                return selectedDays;
            }
        }

        /// <summary>
        /// What is the regeneration type such as Specific day of month, custom date, etc.
        /// </summary>
        public WeeklyType RegenType
        {
            get
            {
                return regenType;
            }
        }

#endregion //Public Fields

    }
}

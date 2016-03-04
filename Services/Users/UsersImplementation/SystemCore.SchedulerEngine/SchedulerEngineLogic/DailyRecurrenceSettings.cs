using System;

using Scheduler.DataRequest;

namespace Scheduler.Logic
{
 
    /// <summary>
    /// Represent the logic behind the Daily Recurrence request
    /// </summary>
    internal class DailyRecurrenceSettings : RecurrenceSettings
    {
        #region Constructors
        /// <summary>
        /// Get dates by Start date only. This is for no ending date values.
        /// </summary>
        /// <param name="startDate"></param>
        public DailyRecurrenceSettings(DateTime startDate, int upperRecurrenceLimit) : base(startDate, upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start and End date boundries.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public DailyRecurrenceSettings(DateTime startDate, DateTime endDate, int upperRecurrenceLimit) : base(startDate, endDate, upperRecurrenceLimit) { }
        /// <summary>
        /// Get dates by Start date and number of occurrences.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="numberOfOccurrences"></param>
        public DailyRecurrenceSettings(DateTime startDate, int numberOfOccurrences, int upperRecurrenceLimit) : base(startDate, numberOfOccurrences, upperRecurrenceLimit) { }
        #endregion

        #region Private Fields
        DailyType regenType = DailyType.OnEveryWeekday;
        int regenEveryXDays = 1;
        DateTime nextDateValue;
        DateTime finalNextDateValue;
        #endregion

        #region Public GetValues 
        /// <summary>
        /// Get day values. This overload is for every x-days.
        /// </summary>
        /// <param name="regenEveryXDays">Interval of days. Every x-days.</param>
        /// <returns>RecurrenceValues</returns>
        public RecurrenceValues GetValues(int regenEveryXDays)
        {
            this.regenEveryXDays = regenEveryXDays;
            regenType = DailyType.OnEveryXDays;
            return GetValues();
        }

        /// <summary>
        ///     An overload to use to get either every weekday or just every x-days
        /// </summary>
        /// <param name="regenEveryXDays" type="int">
        ///     <para>
        ///         Interval of days. Every x-days.
        ///     </para>
        /// </param>
        ///     <para>
        ///         Type of regeneration to perform. Every x-days or every weekday.
        ///     </para>
        /// </param>
        /// <returns>
        ///     A RecurrenceGenerator.RecurrenceValues value...
        /// </returns>
        public RecurrenceValues GetValues(int regenEveryXDays, DailyType regenType)
        {
            this.regenEveryXDays = regenEveryXDays;
            this.regenType = regenType;
            return GetValues();
        }

#endregion //Public GetValues 



        #region Internal GetValues
        /// <summary>
        /// Get day values. Default is to get every weekday. This is called from the RecurrenceHelper staic methods only.
        /// </summary>
        /// <returns>RecurrenceValues</returns>
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
                case DailyType.OnEveryXDays:
                    values = GetEveryXDaysValues();
                    break;

                case DailyType.OnEveryWeekday:
                    values = GetEveryWeekday();
                    break;

            }
            // Values will be null if just getting next date in series. No need
            // to fill the RecurrenceValues collection if all we need is the last date.
            if (values != null)
            {
                if (values.Values.Count > 0)
                {
                    values.SetStartDate(values.Values[0]);

                    // Get the end date if not open-ended
                    if (base.TypeOfEndDate != EndDateType.NotDefined)
                        values.SetEndDate(values.Values[values.Values.Count - 1]);
                }
               
            }

            return values;

        }
#endregion //Internal GetValues

        #region Internal Procedures
       
        void SetValues(int regenEveryXDays)
        {
            this.regenEveryXDays = regenEveryXDays;
            regenType = DailyType.OnEveryXDays;
        }

      
       




#endregion //Internal Procedures

        #region Private Procedures
       


        /// <summary>
        /// Get the values for just weekdays.
        /// </summary>
        /// <returns>RecurrenceValues</returns>
        RecurrenceValues GetEveryWeekday()
        {
            RecurrenceValues values;
            DateTime dt = base.StartDate;
            // Make sure the first date is a weekday
            if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                dt = GetNextWeekday(dt);


            values = new RecurrenceValues();
            switch (base.TypeOfEndDate)
            {

                case EndDateType.NumberOfOccurrences:

                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        values.AddDateValue(dt);
                        dt = GetNextWeekday(dt);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            values.AddDateValue(dt);
                            dt = GetNextWeekday(dt);
                            counter++;
                        } while (dt <= base.EndDate && counter <base.NumberOfOccurrences);
                        break;
                    }
                default:
                    throw new ArgumentNullException("TypeOfEndDate", "The TypeOfEndDate property has not been set.");
            }
            return values;

        }

        /// <summary>
        /// Get the next Weekday value. This will increment the input date until it finds the next non-Saturday and non-Sunday dates.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>DateTime</returns>
        DateTime GetNextWeekday(DateTime input)
        {
            do
            {
                input = input.AddDays(1);
            } while (input.DayOfWeek == DayOfWeek.Saturday || input.DayOfWeek == DayOfWeek.Sunday);

            return input;
        }

        /// <summary>
        /// Get dates for every x-days starting from the start date.
        /// </summary>
        /// <returns></returns>
        RecurrenceValues GetEveryXDaysValues()
        {
            RecurrenceValues values;
            DateTime dt = base.StartDate;


            values = new RecurrenceValues();
            switch (base.TypeOfEndDate)
            {

                case EndDateType.NumberOfOccurrences:

                    for (int i = 0; i < base.NumberOfOccurrences; i++)
                    {
                        values.AddDateValue(dt);
                        dt = dt.AddDays(RegenEveryXDays);
                    }
                    break;

                case EndDateType.SpecificDate:
                    {
                        int counter = 0;
                        do
                        {
                            values.AddDateValue(dt);
                            dt = dt.AddDays(RegenEveryXDays);
                            counter++;
                        } while (dt <= base.EndDate && counter < base.NumberOfOccurrences);
                        break;
                    }
                default:
                    throw new ArgumentNullException("TypeOfEndDate", "The TypeOfEndDate property has not been set.");
            }
            return values;

        }
#endregion //Private Procedures

        #region Public Fields
        /// <summary>
        /// What is the interval to generate dates. This is used to skip days in the cycle.
        /// </summary>
        public int RegenEveryXDays
        {
            get
            {
                return regenEveryXDays;
            }
        }

        /// <summary>
        /// What is the regeneration type such as Specific day of month, custom date, etc.
        /// </summary>
        public DailyType RegenType
        {
            get
            {
                return regenType;
            }
        }
#endregion //Public Fields


    }




}

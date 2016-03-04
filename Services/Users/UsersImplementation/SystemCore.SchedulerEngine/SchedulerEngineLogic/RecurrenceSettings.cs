using System;
using Scheduler.DataRequest;

namespace Scheduler.Logic
{
    public enum EndDateType { NotDefined = -1, SpecificDate, NumberOfOccurrences };
    internal abstract class RecurrenceSettings
    {

        public RecurrenceSettings(DateTime startDate, int numberOfOccurrences)
        {
            this.startDate = startDate;
            if (numberOfOccurrences == 0)
            {
                this.numberOfOccurrences = 30;
            }
            else {
                this.numberOfOccurrences = numberOfOccurrences;
            }
        }
        public RecurrenceSettings(DateTime startDate, DateTime endDate, int numberOfOccurrences)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            endDateType = EndDateType.SpecificDate;
            if (numberOfOccurrences == 0)
            {
                this.numberOfOccurrences = 30;
            }
            else
            {
                this.numberOfOccurrences = numberOfOccurrences;
            }
        }
        public RecurrenceSettings(DateTime startDate, int numberOfOccurrences, int maxOcurenceNo)
        {
            this.startDate = startDate;
            this.numberOfOccurrences = numberOfOccurrences;
            endDateType = EndDateType.NumberOfOccurrences;
            if (numberOfOccurrences > maxOcurenceNo)
            {
                this.numberOfOccurrences = maxOcurenceNo;
            }

           
        }

        DateTime? endDate; // Nullable date because there may or may not be an end date.
        DateTime startDate;
        int recurrenceInterval = 1;
        int numberOfOccurrences = 0;
        int regenerationInterval = 0;
        protected EndDateType  endDateType = EndDateType.NotDefined;

        //internal abstract DateTime GetNextDate(DateTime currentDate);
        internal abstract RecurrenceValues GetValues();
        internal abstract RecurrenceValues GetValues(DateTime startDate, int numberOfOccurrences);
        internal abstract RecurrenceValues GetValues(DateTime startDate, DateTime endDate);

        /// <summary>
        ///     Get/Set the type of End Date. Occurrences, End Date, or no end date at all.
        /// </summary>
        /// <value>
        ///     <para>
        ///         EndDateType enumeration.
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public EndDateType TypeOfEndDate
        {
            get
            {
                return endDateType;
            }
            set
            {
                endDateType = value;
            }
        }

        /// <summary>
        /// Regenerate the Occurrence x-amount of days, weeks, etc. 
        /// after the current item is completed.
        /// </summary>
        public int RegenerationAfterCompletedInterval
        {
            get
            {
                return regenerationInterval;
            }
            set
            {
                regenerationInterval = value;
            }
        }

        public int NumberOfOccurrences
        {
            get
            {
                return numberOfOccurrences;
            }
            set
            {
                numberOfOccurrences = value;
            }
        }

        /// <summary>
        /// Readonly bool if this recurrence instance
        /// has an End date or note. If it does not then
        /// it means this instance has no ending date and should
        /// only create one IRecurrenceItem object.
        /// </summary>
        public bool HasEndDate
        {
            get
            {
                return endDate.HasValue;
            }
        }


        /// <summary>
        ///     End Date for the recurrence values.
        /// </summary>
        /// <value>
        ///     <para>
        ///         DateTime value.
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public DateTime? EndDate
        {
            get
            {
                if (endDate.HasValue)
                    return endDate.Value;
                else
                    return null;
            }
            set
            {
                endDate = value;
            }
        }

        /// <summary>
        ///     Start Date of the rucurence Values.
        /// </summary>
        /// <value>
        ///     <para>
        ///         DateTime value.
        ///     </para>
        /// </value>
        /// <remarks>
        ///     
        /// </remarks>
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

        /// <summary>
        /// This is the "Recurr every x-amount of Days, Weeks.
        /// </summary>
        public int RecurrenceInterval
        {
            get
            {
                return recurrenceInterval;
            }
            set
            {
                recurrenceInterval = value;
            }
        }

    }
}

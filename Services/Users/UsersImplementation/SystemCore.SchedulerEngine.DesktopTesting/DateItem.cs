using System;
using System.Collections.Generic;
using System.Text;

namespace RecurrenceTester
{
    public class DateItem
    {
        DateTime value;
        public DateItem(DateTime dateValue)
        {
            value = dateValue;
        }

        public DateTime Value
        {
            get
            {
                return value;
            }
        }
        public override string ToString()
        {
            return value.ToString("M/dd/yyyy");
            //return value.ToString("d MMM, yyyy   ddd");
        }
    }
}

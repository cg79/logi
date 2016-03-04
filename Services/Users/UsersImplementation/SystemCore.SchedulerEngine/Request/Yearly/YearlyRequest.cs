using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Scheduler.DataRequest
{
    public class YearlyRequest
    {
        public YearlyType YearlyType;
        public YearlyRelativePattern YearlyRelativePattern { get; set; }
        public YearlyPattern YearlyPattern { get; set; }
    }
}

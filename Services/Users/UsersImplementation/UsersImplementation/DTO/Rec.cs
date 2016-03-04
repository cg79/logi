using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class Rec
    {
        public Interval interval { get; set; }
        public int frequencyType { get; set; }
        public int frequencyInterval { get; set; }
        public int frequencyRelativeInterval { get; set; }
        public int frequencyRecurrenceFactor { get; set; }
    }
}

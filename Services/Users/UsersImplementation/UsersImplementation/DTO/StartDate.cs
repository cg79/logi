using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersImplementation.DTO
{
    public class StartDate
    {
        public DateTime isoDate { get; set; }
        public DateTime localDate { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
        public int seconds { get; set; }
        public int offset { get; set; }
    }

}

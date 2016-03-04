using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Pagination
{
   
   

    public enum Operator
    {
        /// <summary>
        /// Equal to
        /// </summary>
        Eq = 0,

        /// <summary>
        /// Greater than
        /// </summary>
        Gt = 1,

        /// <summary>
        /// Greater than or equal to
        /// </summary>
        Ge = 2,

        /// <summary>
        /// Less than
        /// </summary>
        Lt = 3,

        /// <summary>
        /// Less than or equal to
        /// </summary>
        Le = 4,

        /// <summary>
        /// Like (You can use % in the value to do wilcard searching)
        /// </summary>
        Like = 5,
        NotEq = 6,
        Starts=7,
        Contains=8,
        NotContains=9,
        Ends=10
    }
}

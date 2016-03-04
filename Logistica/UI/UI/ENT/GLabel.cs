using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ENT
{
    public class GLabel : BaseObj
    {
        public string For { get; set; }
        public string text { get; set; }

        public override string ToString()
        {
            string rez = "<label";

            if (!string.IsNullOrEmpty(For))
            {
                rez = string.Concat(rez, " for = ", "\"", For, Comma);
            }

            rez = string.Concat(rez, ">", text, "</label>",Environment.NewLine);

            return rez;
        }
    }
}

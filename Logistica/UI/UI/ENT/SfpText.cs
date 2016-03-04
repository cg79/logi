using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ENT
{
    public class SfpText : BaseObj
    {
        public string ToolTip { get; set; }
        public string model { get; set; }

        public GValidation validationInfo { get; set; }

        public override string ToString()
        {
            string rez = "<input";
            rez = string.Concat(rez, " type=", Comma, "text", Comma);

            rez = string.Concat(rez, base.ToString());

            if (!string.IsNullOrEmpty(model))
            {
                rez = string.Concat(rez, " ng-model= ", Comma, model, Comma);
            }
            rez = string.Concat(rez, ">",Environment.NewLine);
            return rez;
        }
    }
}

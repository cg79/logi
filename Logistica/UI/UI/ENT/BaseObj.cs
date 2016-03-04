using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ENT
{
    public class BaseObj
    {
        public int LineNo { get; set; }
        public string Type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int RenderVersion { get; set; }

        protected string Comma = "\"";
        public override string ToString()
        {
            string rez = "";
            if (!string.IsNullOrEmpty(id))
            {
                rez = string.Concat(rez, " id= ",Comma,id,Comma);
            }
            if (!string.IsNullOrEmpty(name))
            {
                rez = string.Concat(rez, " name= ", Comma, name, Comma);
            }
            return rez;
        }
    }
}

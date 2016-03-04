using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ENT
{
    public class GValidation
    {
        public bool IsRequired { get; set; }
        public string RequiredMsg { get; set; }

        public int MaxCharacters { get; set; }
        public string MaxCharactersMsg { get; set; }

        public string Directive { get; set; }
        public string DirectiveMsg { get; set; }
    }
}

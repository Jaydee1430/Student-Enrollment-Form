using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosales_UIDesign
{
    public class Requirements{
        public bool BirthCertificate { get; set; }
        public int Id {  get; set; }
        public bool TOR { get; set; }
        public bool GoodMoral { get; set; }



        public bool AllSubmitted() { 
            return BirthCertificate && TOR && GoodMoral;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosales_UIDesign
{
    public class ProgramInfo {

        public string ProgramName { get; set; }

        public int Id { get; set; }
        public decimal TuitionFee { get; set; }


        public ProgramInfo() { }



        public ProgramInfo(int id, string name, decimal fee) { 
            Id = id;
            ProgramName = name;
            TuitionFee = fee;
        }

    }
}

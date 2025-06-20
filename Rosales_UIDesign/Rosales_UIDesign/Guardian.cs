using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rosales_UIDesign
{
    public class Guardian : Person
    { 
        public int Id {  get; set; }
    public string Relationship { get; set; }
    public Guardian() { }

    public Guardian(int id, string fname, string mnname, string lname, string relationship, string address, string contact)
    {
        Id = id;
        FirstName = fname;
        MiddleName = mnname;
        LastName = lname;
        Relationship = relationship;
        ContactNumber = contact;
        Address = address;
    }

        public override string DisplayInfo()
        {
            return base.DisplayInfo() +
                $"\nName: {LastName}, {FirstName} {MiddleName}" +
                $"\nAddress: {Address}" +
                $"\nRelationship: {Relationship}";
        }
    }
}

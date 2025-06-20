using System;


namespace Rosales_UIDesign
{
    public class Student : Person
    {
        public string StudentId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ProgramInfo Program { get; set; }
        public Guardian Guardian { get; set; }
        public Requirements Requirements { get; set; }
        public Payment Payment { get; set; }
        public string YearLevel { get; set; }

        public Student()
        {
        }
        public Student(string id, string fname, string lname, string mname, DateTime dob,
                        ProgramInfo prog, Guardian guardian, Requirements reqs, Payment pay, string contact, string address,
                        string yearlevel)
        {
            StudentId = id;
            FirstName = fname;
            MiddleName = mname;
            LastName = lname;
            DateOfBirth = dob;
            Program = prog;
            Guardian = guardian;
            Requirements = reqs;
            Payment = pay;
            YearLevel = yearlevel;
            Address = address;
            ContactNumber = contact;
        }
        public override string DisplayInfo()
        {
            return
                $"Student" +
                $"\nID: {StudentId}" +
                base.DisplayInfo() +
                $"\nDOE: {DateOfBirth}" +
                $"\nProgram: {Program.ProgramName}" +
                $"\n" +
                $"\nGuardian:" +
                $"\n{Guardian.DisplayInfo()}" +
                $"\n" + 
                $"\nRequirements Complete: {Requirements.AllSubmitted()}" + 
                $"\nPayment Due: {Payment.CalculateBalance(Program.TuitionFee)}";
        }
    }
}

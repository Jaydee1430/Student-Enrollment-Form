using System;

namespace Rosales_UIDesign
{
    public static class IDGenerator
    {

        private static int counter = 1;

        public static string GenerateStudentID() { 
            string year = DateTime.Now.Year.ToString();
            string numberPart = counter.ToString("D4");
            counter++;

            return $"084-{year}-{numberPart}";
        }
    }
}

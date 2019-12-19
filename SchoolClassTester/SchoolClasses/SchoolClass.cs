using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolClassTester.SchoolClasses
{
    public class SchoolClass
    {
        public string ClassPrefix { get; set; }
        public string ClassSuffix { get; set; }

        public List<StudentDetails> Students { get; set; } = new List<StudentDetails>();
    }
}

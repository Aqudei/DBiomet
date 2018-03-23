using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Reporting
{
    class DTR
    {
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string LogDate { get; internal set; }
        public string AMIN { get; internal set; }
        public string AMOUT { get; internal set; }
        public string PMIN { get; internal set; }
        public string PMOUT { get; internal set; }
    }
}


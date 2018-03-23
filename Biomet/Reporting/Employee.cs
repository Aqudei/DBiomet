using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Reporting
{
    class Employee
    {
        private string employeeNumber;
        private string fullName;
        private string sex;

        public Employee() { }

        public Employee(string employeeNumber, string fullName, string sex, string employeeType)
        {
            EmployeeNumber = employeeNumber;
            FullName = fullName;
            Sex = sex;
            EmployeeType = employeeType;
        }


        public string EmployeeNumber { get => employeeNumber; set => employeeNumber = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string Sex { get => sex; set => sex = value; }
         
        public string EmployeeType { get; set; }
        public string DateHired { get; internal set; }
        public string Designation { get; internal set; }
        public string Department { get; internal set; }
    }
}

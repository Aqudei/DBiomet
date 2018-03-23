using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Biomet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Reporting
{
    class ReportingRepository : RepositoryBase
    {
        public ReportingRepository(BiometContext context) : base(context)
        {
        }

        public IEnumerable<Employee> GetEmployeeList()
        {
            return _context.Employees.ToList().Select(e => new Employee
            {
                EmployeeNumber = e.EmployeeNumber,
                FullName = e.FullName,
                Sex = e.Sex,
                EmployeeType = GetEmployeeType(e),
                DateHired = e.DateHired.HasValue ? e.DateHired.Value.ToShortDateString() : "",
                Designation = e.Designation,
                Department = e.Department

            }).ToList();
        }

        public IEnumerable<DTR> GetDTR()
        {
            return _context.DayLogs.Include("Employee").ToList().Select(d => new DTR
            {
                EmployeeNumber = d.Employee.EmployeeNumber,
                FullName = d.Employee.FullName,
                LogDate = d.LogDate.ToShortDateString(),
                AMIN = d.AMIN.HasValue ? d.AMIN.Value.ToShortTimeString() : "",
                AMOUT = d.AMOUT.HasValue ? d.AMOUT.Value.ToShortTimeString() : "",
                PMIN = d.PMIN.HasValue ? d.PMIN.Value.ToShortTimeString() : "",
                PMOUT = d.PMOUT.HasValue ? d.PMOUT.Value.ToShortTimeString() : "",
            });
        }

        private string GetEmployeeType(Models.Entities.Employee e)
        {
            if (e is SalariedEmployee)
                return "Salaried";

            if (e is HourlyRatedEmployee)
                return "Hourly Rated";

            return string.Empty;
        }
    }
}

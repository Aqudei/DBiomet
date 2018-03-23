using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;

namespace Biomet.Repositories
{
    public class DTRRepository : RepositoryBase
    {
        public DTRRepository(BiometContext context) : base(context)
        { }

        public Employee Get(string employeeNumber, DateTime logDate)
        {
            var _logdate = logDate.Date;
            var employee = _context.Employees.Single(e => e.EmployeeNumber == employeeNumber);
            var daylog = _context.DayLogs.SingleOrDefault(dl => dl.EmployeeId == employee.Id && dl.LogDate == _logdate);

            if (daylog != null)
            {
                employee.DayLogs.Add(daylog);
            }
            else
            {
                employee.DayLogs.Add(new DayLog
                {
                    EmployeeId = employee.Id,
                    LogDate = _logdate,
                });
            }
            return employee;
        }

        public void Save(Employee employee)
        {
            foreach (var item in employee.DayLogs)
            {
                if (item.Id <= 0)
                {
                    _context.Set<DayLog>().Add(item);
                }
                else
                {
                    var dl = _context.Set<DayLog>().Attach(item);
                    _context.Entry(dl).State = System.Data.Entity.EntityState.Modified;
                }

                _context.SaveChanges();
            }
        }
    }
}

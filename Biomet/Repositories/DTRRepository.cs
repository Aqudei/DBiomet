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
            var employee = Context.Employees.Single(e => e.EmployeeNumber == employeeNumber);
            var daylog = Context.DayLogs.SingleOrDefault(dl => dl.EmployeeId == employee.Id && dl.LogDate == logDate);

            if (daylog != null)
            {
                employee.DayLogs.Add(daylog);
            }
            else
            {
                employee.DayLogs.Add(new DayLog
                {
                    EmployeeId = employee.Id,
                    LogDate = logDate,
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
                    Context.Set<DayLog>().Add(item);
                }
                else
                {
                    var dl = Context.Set<DayLog>().Attach(item);
                    Context.Entry(dl).State = System.Data.Entity.EntityState.Modified;
                }

                Context.SaveChanges();
            }
        }
    }
}

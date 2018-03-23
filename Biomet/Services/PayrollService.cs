using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models;
using Biomet.Models.Deductions;
using Biomet.Models.PayReceipt;
using Biomet.Models.Persistence;

namespace Biomet.Services
{
    public class PayrollService
    {
        private DeductorService _deductorService = new DeductorService();

        public PayCheck GeneratePayCheck(int employeeId, DateTime logDate)
        {
            using (var db = new BiometContext())
            {
                var employee = db.Employees.Find(employeeId);
                if (employee == null) return null;

                var payCheck = employee.Pay(DateTime.Now);
                _deductorService.ApplyDeduction(employee, payCheck);
                return payCheck;
            }
        }
    }
}

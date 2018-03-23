using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Deductions
{
    public class PagibigDeduction : IDeduction
    {
        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        {
            if (employee is SalariedEmployee salaried && salaried.HasPagibig)
            {
                HandleSalariedEmployee(salaried, payCheck);
            }
        }

        private void HandleSalariedEmployee(SalariedEmployee salaried, PayCheck payCheck)
        {
            if (salaried.MonthlySalary <= 1500)
            {
                payCheck.DeductPremium("Pag-Ibig", .01 * payCheck.BasePay);
            }

            if (salaried.MonthlySalary > 1500)
            {
                payCheck.DeductPremium("Pag-Ibig", 100);
            }
        }
    }
}

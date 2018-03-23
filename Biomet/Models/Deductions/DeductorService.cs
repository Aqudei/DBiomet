using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Deductions
{
    public class DeductorService : IDeduction
    {
        private List<IDeduction> _deductions = new List<IDeduction>();
        public DeductorService()
        {
            _deductions.Add(new PhilHealthDeduction());
            _deductions.Add(new SSSDeduction());
            _deductions.Add(new PagibigDeduction());
            _deductions.Add(new WitholdingTaxDeduction());
        }

        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        {
            foreach (var d in _deductions)
            {
                d.ApplyDeduction(employee, payCheck);
            }
        }
    }
}

using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Deductions
{
    //Deduction Visitor
    public interface IDeduction
    {
        void ApplyDeduction(Employee employee, PayCheck payCheck);
    }
}

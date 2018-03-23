using Biomet.Models.Deductions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Extentions;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Entities
{
    public class SalariedEmployee : Employee
    {
        public double MonthlySalary { get; set; }

        protected override void OnMakePayment(PayCheck payCheck)
        {
            payCheck.BasePay = MonthlySalary / 4;
        }

        public override bool IsPayDay(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Friday
                   && date.GetWeekOfMonth() < 5;
        }

        protected override void OnDeterminePaymentPeriod(PayCheck payCheck)
        {
            payCheck.StartOfPaymentPeriod = payCheck.PaymentDate.AddDays(-4);
        }

        public bool HasPhilHealth { get; set; }
        public bool HasSSS { get; set; }
        public bool HasPagibig { get; set; }

        public void ApplyDeductions(IDeduction deduction, PayCheck payCheck)
        {
            deduction.ApplyDeduction(this, payCheck);
        }
    }
}

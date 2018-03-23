using Biomet.Extentions;
using Biomet.Models.PayReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public class HourlyRatedEmployee : Employee
    {
        public double RatePerHour { get; set; }
        public bool HasPagibig { get; internal set; }

        protected override void OnDeterminePaymentPeriod(PayCheck payCheck)
        {
            payCheck.StartOfPaymentPeriod = new DateTime(payCheck.PaymentDate.Year, payCheck.PaymentDate.Month, 1);
        }

        public override bool IsPayDay(DateTime date)
        {
            return date.IsLastDayOfMonth();
        }
        

        protected override void OnMakePayment(PayCheck payCheck)
        {
            
        }
    }
}

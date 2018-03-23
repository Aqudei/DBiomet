using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.PayReceipt
{
    public class PayCheck
    {
        public Guid Id { get; private set; }
        public bool Approved { get; set; }

        public PayCheck()
        {
            Id = Guid.NewGuid();
        }

        public DateTime PaymentDate { get; private set; }
        public DateTime StartOfPaymentPeriod { get; internal set; }
        public double BasePay { get; set; }

        [NotMapped]
        public IEnumerable<PayEntry> Deductions
        {

            get
            {
                if (string.IsNullOrWhiteSpace(_Deductions))
                    return new List<PayEntry>();

                return JsonConvert.DeserializeObject<IEnumerable<PayEntry>>(_Deductions);
            }
            private set
            {
                if (value == null || value.Any() == false)
                {
                    _Deductions = "";
                }
                else
                {
                    var json = JsonConvert.SerializeObject(value);
                    _Deductions = json;
                }

            }
        }



        [NotMapped]
        public IEnumerable<PayEntry> Additions
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Additions))
                    return new List<PayEntry>();

                return JsonConvert.DeserializeObject<IEnumerable<PayEntry>>(_Additions);
            }
            private set
            {
                if (value == null || value.Any() == false)
                {
                    _Additions = "";
                }
                else
                {
                    var json = JsonConvert.SerializeObject(value);
                    _Additions = json;
                }
            }
        }

        [NotMapped]
        public IEnumerable<PayEntry> Premiums
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Premiums))
                    return new List<PayEntry>();

                return JsonConvert.DeserializeObject<IEnumerable<PayEntry>>(_Premiums);
            }
            private set
            {
                if (value == null || value.Any() == false)
                {
                    _Premiums = "";
                }
                else
                {
                    var json = JsonConvert.SerializeObject(value);
                    _Premiums = json;
                }
            }
        }

        public double WitholdingTax { get; set; }

        public void SetTax(double tax)
        {
            WitholdingTax = tax;
        }

        internal double NetLessPremiums()
        {
            var total = BasePay;

            foreach (var item in Additions)
            {
                total += item.Amount;
            }

            foreach (var item in Premiums)
            {
                total -= item.Amount;
            }

            return total;
        }

        internal string _Deductions { get; set; }
        internal string _Additions { get; set; }
        internal string _Premiums { get; set; }

        public double NetTotal
        {
            get
            {
                var total = BasePay;

                foreach (var item in Additions)
                {
                    total += item.Amount;
                }

                foreach (var item in Deductions)
                {
                    total -= item.Amount;
                }

                foreach (var item in Premiums)
                {
                    total -= item.Amount;
                }

                total -= WitholdingTax;

                return total;
            }
        }

        public void Deduct(string name, double balue)
        {
            var list = new List<PayEntry>(Deductions);
            if (list.Any(l => l.Label == name))
                throw new Exception($"Cannot deduct multiple {name} to paycheck.");

            list.Add(new PayEntry
            {
                Label = name,
                Amount = balue
            });
            Deductions = list;
        }

        internal void DeductPremium(string name, double balue)
        {
            var list = new List<PayEntry>(Premiums);
            if (list.Any(l => l.Label == name))
                throw new Exception($"Cannot deduct multiple {name} to paycheck.");

            list.Add(new PayEntry
            {
                Label = name,
                Amount = balue
            });
            Premiums = list;
        }

        public PayCheck(DateTime payDateTime) : this()
        {
            PaymentDate = payDateTime;
        }

        public static PayCheck PayCheckForDay(DateTime date)
        {
            return new PayCheck
            {
                PaymentDate = date.Date
            };
        }

        public string ToPrintFormat()
        {
            var sb = new StringBuilder();
            sb.AppendLine("-----------------------------");
            sb.AppendLine("-----Your Payroll System-----");
            sb.AppendLine("-----------------------------");
            sb.AppendLine();
            sb.AppendFormat("Date: {0}", PaymentDate.ToShortDateString());
            sb.AppendLine();
            sb.AppendFormat("Base Pay: {0}", BasePay);
            sb.AppendLine();
            sb.AppendLine("-----Additions-----");
            foreach (var item in Additions)
            {
                sb.AppendLine($"{item.Label}---{item.Amount}");
            }
            sb.AppendLine("-----Deductions-----");
            foreach (var item in Deductions)
            {
                sb.AppendLine($"{item.Label}---{item.Amount}");
            }
            sb.AppendLine("-----Premiums-----");
            foreach (var item in Premiums)
            {
                sb.AppendLine($"{item.Label}---{item.Amount}");
            }
            sb.AppendLine("---Tax------------");
            sb.AppendLine($"Witholding Tax---{WitholdingTax}");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine($"Total: {NetTotal}");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }
    }
}

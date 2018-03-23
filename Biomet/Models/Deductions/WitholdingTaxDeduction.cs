using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Deductions
{
    public class WitholdingTaxDeduction : IDeduction
    {
        private List<TaxInfo> taxInfos;

        class TaxInfo
        {
            public double LowerCL { get; set; }
            public double UpperCL { get; set; }
            public double PercentageOverLowerCL { get; set; }
            public double BaseTax { get; set; }
        }

        public WitholdingTaxDeduction()
        {
            taxInfos = new List<TaxInfo>();
            taxInfos.Add(new TaxInfo
            {
                LowerCL = 0,
                UpperCL = 20833,
            });

            taxInfos.Add(new TaxInfo
            {
                LowerCL = 20834,
                UpperCL = 33332.999,
                PercentageOverLowerCL = 0.20
            });

            taxInfos.Add(new TaxInfo
            {
                LowerCL = 33333,
                UpperCL = 66666.999,
                PercentageOverLowerCL = 0.25,
                BaseTax = 2500
            });

            taxInfos.Add(new TaxInfo
            {
                LowerCL = 66667,
                UpperCL = 166666.999,
                PercentageOverLowerCL = 0.30,
                BaseTax = 10833.33
            });

            taxInfos.Add(new TaxInfo
            {
                LowerCL = 166667,
                UpperCL = 666666.999,
                PercentageOverLowerCL = 0.32,
                BaseTax = 40833.33
            });

            taxInfos.Add(new TaxInfo
            {
                LowerCL = 666667,
                UpperCL = 9999999.999,
                PercentageOverLowerCL = 0.35,
                BaseTax = 200833.33
            });
        }

        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        {
            double netLessPremium = payCheck.NetLessPremiums();

            var taxInfo = (from t in taxInfos
                           where t.LowerCL <= netLessPremium && t.UpperCL >= netLessPremium
                           select t).FirstOrDefault();

            if (taxInfo != null)
            {
                var tax = taxInfo.BaseTax + (netLessPremium - taxInfo.LowerCL) * taxInfo.PercentageOverLowerCL;
                payCheck.SetTax(tax);
            }
        }
    }
}

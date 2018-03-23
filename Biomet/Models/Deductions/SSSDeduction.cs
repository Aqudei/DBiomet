using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Deductions
{
    public class SSSDeduction : IDeduction
    {
        private List<DeductionTable> _lookupTable;

        public SSSDeduction()
        {
            using (var csvReader = new CsvHelper.CsvReader(File.OpenText("Tables/sss.csv")))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                _lookupTable = csvReader.GetRecords<DeductionTable>().ToList();
            }
        }

        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        {
            if (employee is SalariedEmployee salariedEmployee && salariedEmployee.HasSSS)
            {
                var row = _lookupTable.FirstOrDefault(lookup =>
                    lookup.A <= salariedEmployee.MonthlySalary && lookup.B >= salariedEmployee.MonthlySalary);
                if (row == null)
                {
                    throw new ArgumentException("Unknown salary range");
                }

                payCheck.DeductPremium("SSS", row.Deduction / 4);
            }
        }
    }
}

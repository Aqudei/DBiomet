using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Deductions
{
    public class PhilHealthDeduction : IDeduction
    {
        private const string DEDUCTION_LABEL = "PhilHealth";

        private IEnumerable<DeductionTable> _lookupTable;

        public PhilHealthDeduction()
        {
            using (var csvReader = new CsvHelper.CsvReader(File.OpenText("Tables/philhealth.csv")))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                _lookupTable = csvReader.GetRecords<DeductionTable>().ToList();
            }
        }

        public void ApplyDeductionToSalariedEmployee(SalariedEmployee employee, PayCheck payCheck)
        {
            if (!employee.HasPhilHealth) return;

            var row = _lookupTable
                .FirstOrDefault(lookup => lookup.A <= employee.MonthlySalary && lookup.B >= employee.MonthlySalary);


            if (row == null)
                throw new ArgumentException("Salary not found on the PhilHealth Table");

            var deduction = employee.MonthlySalary - row.Deduction;
            payCheck.DeductPremium(DEDUCTION_LABEL, deduction / 4);
        }

        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        {
            if (employee is SalariedEmployee salariedEmployee)
            {
                ApplyDeductionToSalariedEmployee(salariedEmployee, payCheck);
            }
        }
    }
}

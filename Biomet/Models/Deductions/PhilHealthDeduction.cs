using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;

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

                _lookupTable = csvReader.GetRecords<DeductionTable>();
            }
        }

        public void ApplyDeduction(SalariedEmployee employee, PayCheck payCheck)
        {
            if (!employee.HasPhilHealth) return;
            if (payCheck.Deductions.ContainsKey(DEDUCTION_LABEL)) return;


            var row = _lookupTable
                .FirstOrDefault(l => employee.MonthlySalary >= l.A && employee.MonthlySalary <= l.B);
            if (row == null)
                throw new ArgumentException("Salary not found on the PhilHealth Table");

            var deduction = employee.MonthlySalary - row.Deduction;
            payCheck.Deductions.Add(DEDUCTION_LABEL, deduction / 4);
        }

        public void ApplyDeduction(HourlyRatedEmployee employee, PayCheck payCheck)
        { }
    }
}

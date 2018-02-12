using Biomet.Models.Deductions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public class SalariedEmployee : Employee
    {
        public int MonthlySalary { get; set; }

        protected override void OnMakePayment(PayCheck payCheck)
        {

        }

        protected override bool IsPayDay(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Friday;
        }

        protected override void OnDeterminePaymentPeriod(PayCheck payCheck)
        {
            payCheck.StartOfPaymentPeriod = payCheck.PaymentDate.AddDays(-4);
        }

        public bool HasPhilHealth { get; set; }
        public bool HasSSS { get; set; }

        public void ApplyDeductions(IDeduction deduction, PayCheck payCheck)
        {
            deduction.ApplyDeduction(this, payCheck);
        }
    }
}

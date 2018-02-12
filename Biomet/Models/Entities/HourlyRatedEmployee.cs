using Biomet.Extentions;
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

        protected override void DeterminePaymentPeriod(PayCheck payCheck)
        {
            payCheck.StartOfPaymentPeriod = new DateTime(payCheck.PaymentDate.Year, payCheck.PaymentDate.Month, 1);
        }

        protected override bool IsPayDay(DateTime date)
        {
            return date.IsLastDayOfMonth();
        }

        protected override void MakePayment(PayCheck payCheck)
        {
            
        }
    }
}

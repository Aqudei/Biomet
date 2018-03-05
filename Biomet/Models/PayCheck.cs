using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models
{
    public class PayCheck
    {
        public DateTime PaymentDate { get; private set; }
        public DateTime StartOfPaymentPeriod { get; internal set; }
        public Dictionary<string, double> Deductions { get; set; }
        public Dictionary<string, double> Additions { get; set; }

        public PayCheck(DateTime payDateTime)
        {
            PaymentDate = payDateTime;
            Deductions = new Dictionary<string, double>();
            Additions = new Dictionary<string, double>();
        }

        public PayCheck()
        { }

        public static PayCheck PayCheckForDay(DateTime date)
        {
            return new PayCheck
            {
                PaymentDate = date.Date
            };
        }
    }
}

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
        public double BasePay { get; set; }
        public Dictionary<string, double> Deductions { get; set; }
        public Dictionary<string, double> Additions { get; set; }

        public PayCheck(DateTime payDateTime) : this()
        {
            PaymentDate = payDateTime;
        }

        private PayCheck()
        {
            Deductions = new Dictionary<string, double>();
            Additions = new Dictionary<string, double>();
        }

        public static PayCheck PayCheckForDay(DateTime date)
        {
            return new PayCheck
            {
                PaymentDate = date.Date
            };
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("------------------------");
            sb.AppendLine("-----Payroll System-----");
            sb.AppendLine("------------------------");
            sb.AppendLine();
            sb.AppendFormat("Date: {0}\n", PaymentDate.ToShortDateString());
            sb.AppendFormat("Base Pay: {0}\n", BasePay);
            sb.AppendLine("-----Additions-----");
            sb.AppendLine("-----Deduction-----");
            return sb.ToString();
        }
    }
}

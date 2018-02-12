using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public abstract class Employee : EntityBase
    {
        public string FirstName { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual void Pay(PayCheck payCheck)
        {
            if (!IsPayDay(payCheck.PaymentDate))
                throw new Exceptions.NotPayDayException();

            DeterminePaymentPeriod(payCheck);
            MakePayment(payCheck);
        }

        protected abstract void DeterminePaymentPeriod(PayCheck payCheck);
        protected abstract void MakePayment(PayCheck payCheck);
        protected abstract bool IsPayDay(DateTime date);
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public abstract class Employee : EntityBase
    {

        public enum EMPLOYEE_TYPE
        {
            Salaried,
            HourlyRated
        }

        public string FirstName { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual void Pay(PayCheck payCheck)
        {
            if (!IsPayDay(payCheck.PaymentDate))
                throw new Exceptions.NotPayDayException();

            OnDeterminePaymentPeriod(payCheck);
            OnMakePayment(payCheck);
        }


        protected abstract void OnDeterminePaymentPeriod(PayCheck payCheck);
        protected abstract void OnMakePayment(PayCheck payCheck);
        protected abstract bool IsPayDay(DateTime date);

        public static Employee Create(string empType)
        {
            var employeeType = (EMPLOYEE_TYPE)Enum.Parse(typeof(EMPLOYEE_TYPE), empType);
            if (employeeType == EMPLOYEE_TYPE.HourlyRated)
            {
                return new HourlyRatedEmployee();
            }
            if (employeeType == EMPLOYEE_TYPE.Salaried)
            {
                return new SalariedEmployee();
            }

            throw new ArgumentOutOfRangeException("Unknown employee type");
        }
    }
}

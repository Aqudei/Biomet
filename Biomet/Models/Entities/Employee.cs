using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public string EmployeeNumber { get; set; }

        protected Employee()
        {
            TimeCards = new List<TimeCard>();
            DayLogs = new List<DayLog>();
        }

        public void PostTimeCard(DateTime logDate, double numHours)
        {
            TimeCards.Add(new TimeCard
            {
                LogDate = logDate.Date,
                NumberOfHours = numHours
            });
        }

        public string Photo { get; set; }

        public List<TimeCard> TimeCards { get; set; }
        public List<DayLog> DayLogs { get; set; }

        public void AddDayLog(DayLog dayLog)
        {

        }



        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{LastName}, {FirstName} {MiddleName[0]}";

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

        public void SetLog(int selectedLogType, DateTime? logdate = null)
        {
            if (!logdate.HasValue)
                logdate = DateTime.Now;

            switch (selectedLogType)
            {
                case 1:
                    {
                        DayLogs.First().AMIN = logdate.Value;
                        break;
                    }
                case 2:
                    {
                        DayLogs.First().AMOUT = logdate.Value;
                        break;
                    }
                case 3:
                    {
                        DayLogs.First().PMIN = logdate.Value;
                        break;
                    }
                case 4:
                    {
                        DayLogs.First().PMOUT = logdate.Value;
                        break;
                    }
            }

        }
    }
}

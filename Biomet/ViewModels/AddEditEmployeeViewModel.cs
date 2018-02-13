using AutoMapper;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class AddEditEmployeeViewModel : Screen
    {
        private string _photo;
        private double _monthlySalary;
        private bool _hasPhilHealth;
        private bool _hasSSS;
        private bool _hasPagibig;
        private double _ratePerHour;

        public string Photo
        {
            get { return _photo; }
            set { Set(ref _photo, value); }
        }

        public AddEditEmployeeViewModel(IEventAggregator eventAggregator)
        {
            PaymentTypes = new Dictionary<string, Employee.EMPLOYEE_TYPE>
            {
                { Enum.GetName(typeof(Employee.EMPLOYEE_TYPE),Employee.EMPLOYEE_TYPE.Salaried) , Employee.EMPLOYEE_TYPE.Salaried},
                { Enum.GetName(typeof(Employee.EMPLOYEE_TYPE),Employee.EMPLOYEE_TYPE.HourlyRated) , Employee.EMPLOYEE_TYPE.HourlyRated},
            };

            Sexes = new Dictionary<string, string>
            {
                {"Male","Male" },
                {"Female","Female" }
            };
            Sex = "Male";

            _eventAggregator = eventAggregator;
        }

        private Employee.EMPLOYEE_TYPE _paymentType;
        private string _sex;
        private int _id;
        private string _firstName;
        private string _middleName;
        private string _lastName;
        private DateTime? _birthday;
        private string _birthplace;
        private readonly IEventAggregator _eventAggregator;

        public Employee.EMPLOYEE_TYPE PaymentType
        {
            get { return _paymentType; }
            set { Set(ref _paymentType, value); }
        }

        public Dictionary<string, string> Sexes { get; }


        public void Done()
        {
            TryClose();
        }

        public Dictionary<string, Employee.EMPLOYEE_TYPE> PaymentTypes { get; }
        public string Sex { get => _sex; set => Set(ref _sex, value); }
        public double MonthlySalary { get => _monthlySalary; set => Set(ref _monthlySalary, value); }
        public bool HasPhilHealth { get => _hasPhilHealth; set => Set(ref _hasPhilHealth, value); }
        public bool HasSSS { get => _hasSSS; set => Set(ref _hasSSS, value); }
        public bool HasPagibig { get => _hasPagibig; set => Set(ref _hasPagibig, value); }
        public double RatePerHour { get => _ratePerHour; set => Set(ref _ratePerHour, value); }
        public int Id { get => _id; set => Set(ref _id, value); }
        public string FirstName { get => _firstName; set => Set(ref _firstName, value); }
        public string MiddleName { get => _middleName; set => Set(ref _middleName, value); }
        public string LastName { get => _lastName; set => Set(ref _lastName, value); }
        public DateTime? Birthday { get => _birthday; set => Set(ref _birthday, value); }
        public string Birthplace { get => _birthplace; set => Set(ref _birthplace, value); }

        public async void Save()
        {
            var emp = Employee.Create(Enum.GetName(typeof(Employee.EMPLOYEE_TYPE), PaymentType));
            Mapper.Map(this, emp);
            if (Id <= 0)
            {
                using (var db = new BiometContext())
                {
                    db.Employees.Add(emp);
                    await db.SaveChangesAsync();
                    await _eventAggregator.PublishOnCurrentThreadAsync(new Events.CrudEvent<Employee>(emp, Events.CrudEvent<Employee>.CrudActionEnum.Created));
                }
            }
        }
    }
}

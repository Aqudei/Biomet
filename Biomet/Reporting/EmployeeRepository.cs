using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Biomet.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Reporting
{
    class EmployeeRepository : RepositoryBase
    {
        public EmployeeRepository(BiometContext context) : base(context)
        {
        }

        public IEnumerable<Employee> GetList()
        {
            return _context.Employees.ToList().Select(e => new Employee
            {
                EmployeeNumber = e.EmployeeNumber,
                FullName = e.FullName,
                Sex = e.Sex,
                EmployeeType = GetEmployeeType(e)
            }).ToList();
        }

        private string GetEmployeeType(Models.Entities.Employee e)
        {
            if (e is SalariedEmployee)
                return "Salaried";

            if (e is HourlyRatedEmployee)
                return "Hourly Rated";

            return string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;

namespace Biomet.Repositories
{
    public class DTRRepository : RepositoryBase
    {
        public DTRRepository(BiometContext context) : base(context)
        {}

        public Employee Get(string employeeNumber, DateTime logDate)
        {
            var _logdate = logDate.Date;
            var result = (from e in _context.Employees
                          where e.EmployeeNumber == employeeNumber
                          select new
                          {
                              Employee = e,
                              Log = e.DayLogs.FirstOrDefault(l => l.LogDate == _logdate)
                          }).FirstOrDefault();

            result.Employee.DayLogs = new List<DayLog>();
            if (result.Log != null)
            {
                result.Employee.DayLogs.Add(result.Log);
            }
            else
            {
                result.Employee.DayLogs.Add(new DayLog
                {
                    EmployeeId = result.Employee.Id,
                    LogDate = _logdate,
                });
            }
            return result.Employee;
        }

        public void Save(Employee employee)
        {
            foreach (var item in employee.DayLogs)
            {
                if (item.Id <= 0)
                {
                    _context.Set<DayLog>().Add(item);
                }
                else
                {
                    var dl = _context.Set<DayLog>().Attach(item);
                    _context.Entry(dl).State = System.Data.Entity.EntityState.Modified;
                }

                _context.SaveChanges();
            }
        }
    }
}

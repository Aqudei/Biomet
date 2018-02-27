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
        {
        }

        public Employee Get(int id, DateTime logDate)
        {
            var _logdate = logDate.Date;
            var result = (from e in _context.Employees
                          where e.Id == id
                          select new
                          {
                              Employee = e,
                              Logs = e.DayLogs.Where(l => l.LogDate == _logdate)
                          }).FirstOrDefault();

            result.Employee.DayLogs = new List<DayLog>();
            result.Employee.DayLogs.AddRange(result.Logs.ToList());
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
                _context.SaveChanges();
            }
        }
    }
}

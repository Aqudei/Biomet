using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models;
using Biomet.Models.Deductions;
using Biomet.Models.Persistence;

namespace Biomet.Services
{
    public class PayrollService
    {
        private DeductionsComposite _deductionsComposite = new DeductionsComposite();

        public PayCheck GeneratePayCheck(int employeeId, DateTime logDate)
        {
            using (var db = new BiometContext())
            {
                var employee = db.Employees.Find(employeeId);
                if (employee == null) return null;

                var payCheck = employee.Pay(DateTime.Now);
                _deductionsComposite.ApplyDeduction(employee, payCheck);
                return payCheck;
            }
        }
    }
}

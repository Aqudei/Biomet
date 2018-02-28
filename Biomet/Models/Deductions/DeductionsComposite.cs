using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;

namespace Biomet.Models.Deductions
{
    public class DeductionsComposite : IDeduction
    {
        private List<IDeduction> _deductions = new List<IDeduction>();
        public DeductionsComposite()
        {
            _deductions.Add(new PhilHealthDeduction());
            _deductions.Add(new SSSDeduction());
            _deductions.Add(new WitholdingTaxDeduction());
        }

        public void ApplyDeduction(SalariedEmployee employee, PayCheck payCheck)
        {
            foreach (var d in _deductions)
            {
                d.ApplyDeduction(employee, payCheck);
            }
        }

        public void ApplyDeduction(HourlyRatedEmployee employee, PayCheck payCheck)
        {
            foreach (var d in _deductions)
            {
                d.ApplyDeduction(employee, payCheck);
            }
        }
    }
}

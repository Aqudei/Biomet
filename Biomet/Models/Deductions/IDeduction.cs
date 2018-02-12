using Biomet.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Deductions
{
    //Deduction Visitor
    public interface IDeduction
    {
        void ApplyDeduction(SalariedEmployee employee, PayCheck payCheck);
        void ApplyDeduction(HourlyRatedEmployee employee, PayCheck payCheck);
    }
}

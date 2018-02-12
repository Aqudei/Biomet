using Biomet.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Deductions
{
    public interface IDeductable<T> where T : IDeduction
    {
        void ApplyDeductions(T deduction, PayCheck payCheck);
    }
}

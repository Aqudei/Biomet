using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;

namespace Biomet.Models.Deductions
{
    public class WitholdingTaxDeduction : IDeduction
    {
        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        { }
    }
}

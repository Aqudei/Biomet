﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;

namespace Biomet.Models.Deductions
{
    public class SSSDeduction : IDeduction
    {
        public void ApplyDeduction(SalariedEmployee employee, PayCheck payCheck)
        {
            throw new NotImplementedException();
        }

        public void ApplyDeduction(HourlyRatedEmployee employee, PayCheck payCheck)
        { }

        public void ApplyDeduction(Employee employee, PayCheck payCheck)
        {}
    }
}

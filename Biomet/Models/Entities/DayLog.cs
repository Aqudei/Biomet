using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Entities
{
    public class DayLog : EntityBase
    {
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        

    }
}

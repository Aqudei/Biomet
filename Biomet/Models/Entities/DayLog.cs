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

        public virtual Employee Employee { get; set; }

        public DateTime LogDate { get; set; }
        public TimeSpan? AMIN { get; set; }
        public TimeSpan? AMOUT { get; set; }
        public TimeSpan? PMIN { get; set; }
        public TimeSpan? PMOUT { get; set; }
    }
}

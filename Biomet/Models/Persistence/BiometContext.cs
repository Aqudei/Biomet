using Biomet.Models.Entities;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Persistence
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BiometContext : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<DayLog> DayLogs { get; set; }
        
        public BiometContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BiometContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }
    }
}

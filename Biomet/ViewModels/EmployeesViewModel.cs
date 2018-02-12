using Biomet.Models.Entities;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class EmployeesViewModel : Screen
    {
        public BindableCollection<Employee> Employees { get; set; }

        public EmployeesViewModel()
        {
            Employees = new BindableCollection<Employee>();
        }

        protected override void OnViewLoaded(object view)
        {
            Task.Run(() =>
            {
                using (var db = new Models.Persistence.BiometContext())
                {
                    Employees.AddRange(db.Employees.ToList());
                }
            });
        }
    }
}

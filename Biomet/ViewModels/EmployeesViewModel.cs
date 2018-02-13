using Biomet.Events;
using Biomet.Models.Entities;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class EmployeesViewModel : Screen, IHandle<Events.CrudEvent<Employee>>
    {
        private readonly IWindowManager _windowManager;

        public BindableCollection<Employee> Employees { get; set; }

        public EmployeesViewModel(IWindowManager windowManager)
        {
            Employees = new BindableCollection<Employee>();
            _windowManager = windowManager;
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

        public void NewEmployee()
        {
            _windowManager.ShowDialog(IoC.Get<AddEditEmployeeViewModel>());
        }

        public Task HandleAsync(CrudEvent<Employee> message, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (message.CrudAction == CrudEvent<Employee>.CrudActionEnum.Created)
                {
                    Employees.Add(message.Entity);
                }

            }, cancellationToken);
        }
    }
}

using Biomet.Events;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class EmployeesViewModel : Screen, IHandle<CrudEvent<Employee>>
    {
        private readonly IWindowManager _windowManager;
        private readonly IEventAggregator _eventAggregator;
        private Employee _selectedEmployee;

        public BindableCollection<Employee> Employees { get; set; }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee; set => Set(ref _selectedEmployee, value);
        }

        public EmployeesViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {
            Employees = new BindableCollection<Employee>();
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedEmployee))
                {
                    NotifyOfPropertyChange(nameof(CanDelete));
                }
            };

            _windowManager = windowManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
        }

        protected override void OnViewLoaded(object view)
        {
            if (Employees.Any())
                Employees.Clear();

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

        public async void Delete()
        {
            using (var db = new BiometContext())
            {
                db.Employees.Attach(SelectedEmployee);
                db.Employees.Remove(SelectedEmployee);
                await db.SaveChangesAsync();
                await _eventAggregator.PublishOnCurrentThreadAsync(new Events.CrudEvent<Employee>(SelectedEmployee, CrudEvent<Employee>.CrudActionEnum.Deleted));
            }
        }

        public bool CanDelete { get => SelectedEmployee != null; }

        public Task HandleAsync(CrudEvent<Employee> message, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (message.CrudAction == CrudEvent<Employee>.CrudActionEnum.Created)
                {
                    Employees.Add(message.Entity);
                }

                if (message.CrudAction == CrudEvent<Employee>.CrudActionEnum.Deleted)
                {
                    Employees.Remove(message.Entity);
                }

            }, cancellationToken);
        }
    }
}

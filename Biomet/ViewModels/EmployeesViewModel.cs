using Biomet.Dialogs.ViewModels;
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
                    NotifyOfPropertyChange(nameof(CanEnrollFinger));
                    NotifyOfPropertyChange(nameof(CanModify));
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



        public bool CanModify => SelectedEmployee != null;


        public void Modify()
        {
            var vm = IoC.Get<AddEditEmployeeViewModel>();
            vm.Edit(SelectedEmployee);
            _windowManager.ShowDialog(vm);
        }

        public void NewEmployee()
        {
            _windowManager.ShowDialog(IoC.Get<AddEditEmployeeViewModel>());
        }

        public bool CanEnrollFinger
        {
            get
            {
                return SelectedEmployee != null;
            }
        }

        public void EnrollFinger()
        {
            var frVm = IoC.Get<FingerRegistrationViewModel>();
            frVm.Employee = SelectedEmployee;
            _windowManager.ShowDialog(frVm);
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

        public void GeneratePayChecks()
        {
            var dlg = new DateInputDialogViewModel();
            var rslt = _windowManager.ShowDialog(dlg);
            if (rslt.HasValue && rslt.Value)
            {
                var payDate = dlg.PayDate.Value;
            }
        }

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

                if (message.CrudAction == CrudEvent<Employee>.CrudActionEnum.Updated)
                {
                    var _updated = Employees.FirstOrDefault(e => e.Id == message.Entity.Id);
                    if (_updated != null)
                    {
                        Employees.Remove(_updated);
                        Employees.Add(message.Entity);
                    }
                }

            }, cancellationToken);
        }
    }
}

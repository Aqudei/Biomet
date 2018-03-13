﻿using Biomet.Dialogs.ViewModels;
using Biomet.Events;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Biomet.Extentions;
using Biomet.Models.Deductions;
using Xceed.Words.NET;

namespace Biomet.ViewModels
{
    internal sealed class EmployeesViewModel : Screen, IHandle<CrudEvent<Employee>>
    {
        private readonly IWindowManager _windowManager;
        private readonly DeductorService _deductorService = new DeductorService();
        private readonly IEventAggregator _eventAggregator;
        private Employee _selectedEmployee;

        public BindableCollection<Employee> Employees { get; set; }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => Set(ref _selectedEmployee, value);
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

                    PhotoSource = string.IsNullOrWhiteSpace(SelectedEmployee?.Photo)
                        ? null
                        : SelectedEmployee.Photo.BitmapFromStringPath();
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
                using (var db = new BiometContext())
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

        public bool CanEnrollFinger => SelectedEmployee != null;

        private ImageSource _photoSource;

        public ImageSource PhotoSource
        {
            get => _photoSource;
            set => Set(ref _photoSource, value);
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
                await _eventAggregator.PublishOnCurrentThreadAsync(new CrudEvent<Employee>(SelectedEmployee,
                    CrudEvent<Employee>.CrudActionEnum.Deleted));
            }
        }

        public bool CanDelete => SelectedEmployee != null;

        public void GeneratePayChecks()
        {
            var dateDlg = new Dialogs.ViewModels.DateInputDialogViewModel();
            var rslt = _windowManager.ShowDialog(dateDlg);
            if (rslt.HasValue && rslt.Value)
            {
                foreach (var emp in Employees)
                {
                    if (emp.IsPayDay(dateDlg.PayDate.Value))
                    {
                        var payCheck = emp.Pay(dateDlg.PayDate.Value);
                        File.WriteAllText("forprinting.txt", payCheck.ToPrintFormat());
                        Process.Start($"print /d:\"XP-58\" forprinting.txt");
                    }
                }
            }
        }

        public Task HandleAsync(CrudEvent<Employee> message, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                switch (message.CrudAction)
                {
                    case CrudEvent<Employee>.CrudActionEnum.Created:
                        Employees.Add(message.Entity);
                        break;
                    case CrudEvent<Employee>.CrudActionEnum.Deleted:
                        Employees.Remove(message.Entity);
                        break;
                    case CrudEvent<Employee>.CrudActionEnum.Updated:
                        var updated = Employees.FirstOrDefault(e => e.Id == message.Entity.Id);
                        if (updated != null)
                        {
                            Employees.Remove(updated);
                            Employees.Add(message.Entity);
                        }

                        break;
                }
            }, cancellationToken);
        }
    }
}
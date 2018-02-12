﻿using Biomet.ViewModels;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biomet
{
    class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _simpleContainer = new SimpleContainer();
        public Bootstrapper()
        {
            Initialize();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _simpleContainer.GetAllInstances(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _simpleContainer.GetInstance(service, key);
        }

        protected override void BuildUp(object instance)
        {
            _simpleContainer.BuildUp(instance);
        }

        protected override void Configure()
        {
            _simpleContainer.Instance(_simpleContainer);

            _simpleContainer.PerRequest<ShellViewModel>();
            _simpleContainer.PerRequest<AddEditEmployeeViewModel>();
            _simpleContainer.PerRequest<EmployeesViewModel>();
            _simpleContainer.PerRequest<DTRViewModel>();

            _simpleContainer.Singleton<IWindowManager, WindowManager>();
            _simpleContainer.Singleton<IEventAggregator, EventAggregator>();

            base.Configure();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}

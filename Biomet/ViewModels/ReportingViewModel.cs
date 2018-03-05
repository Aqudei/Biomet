using Biomet.Reporting;
using Caliburn.Micro;
using SAPBusinessObjects.WPF.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class ReportingViewModel : Screen
    {
        private CrystalReportsViewer _reportViewer;
        private readonly EmployeeRepository employeeRepository;

        public ReportingViewModel(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _reportViewer = (view as Views.ReportingView).ReportViewer;
        }

        public void ShowEmployeeList()
        {
            var empList = new EmployeeList();
            empList.SetDataSource(employeeRepository.GetList());
            _reportViewer.ViewerCore.ReportSource = empList;
            empList.Refresh();
        }
    }
}

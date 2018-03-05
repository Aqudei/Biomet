using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            OpenDTR();

            DisplayName = "Your Payroll System";
        }

        public void OpenManager()
        {
            ActivateItem(IoC.Get<EmployeesViewModel>());
        }

        public void OpenDTR()
        {
            ActivateItem(IoC.Get<DTRViewModel>());
        }

        public void OpenReporting()
        {
            ActivateItem(IoC.Get<ReportingViewModel>());
        }
    }
}

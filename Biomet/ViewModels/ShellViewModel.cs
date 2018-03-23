using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Dialogs.ViewModels;

namespace Biomet.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        private readonly IWindowManager _windowManager;

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            OpenDTR();

            DisplayName = "Your Payroll System";
        }

        public void OpenManager()
        {
            var authDialog = new AuthenticationViewModel();
            var rslt = _windowManager.ShowDialog(authDialog);
            if (rslt.HasValue && rslt.Value)
            {
                ActivateItem(IoC.Get<EmployeesViewModel>());
            }
        }

        public void OpenDTR()
        {
            ActivateItem(IoC.Get<DTRViewModel>());
        }

        public void OpenReporting()
        {
            ActivateItem(IoC.Get<ReportingViewModel>());
        }

        public void GoHome()
        {
            ActivateItem(IoC.Get<EmployeesViewModel>());
        }
    }
}

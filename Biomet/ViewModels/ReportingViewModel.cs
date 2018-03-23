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
        private readonly ReportingRepository employeeRepository;

        public ReportingViewModel(ReportingRepository employeeRepository)
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
            //var obj = new PrinterUtility.EscPosEpsonCommands.EscPosEpson();

            //var empList = employeeRepository.GetList();
            //byte[] buffer = new byte[1024];
            //foreach (var item in empList)
            //{
            //    var bytes = Encoding.ASCII.GetBytes(item.FullName);
            //    buffer = PrinterUtility.PrintExtensions.AddBytes(buffer, bytes);
            //    buffer = PrinterUtility.PrintExtensions.AddBytes(buffer, obj.Lf());
            //}

            //PrinterUtility.PrintExtensions.Print(buffer);

            var empList = new EmployeeList();
            empList.SetDataSource(employeeRepository.GetEmployeeList());
            _reportViewer.ViewerCore.ReportSource = empList;
            empList.Refresh();
        }

        public void ShowDTR()
        {
            var dtr = new DTRReportrpt();
            dtr.SetDataSource(employeeRepository.GetDTR());
            _reportViewer.ViewerCore.ReportSource = dtr;
            dtr.Refresh();
        }
    }
}

using AutoMapper;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Biomet.Reporting;
using Biomet.Repositories;
using Biomet.ViewModels;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
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
            Directory.CreateDirectory(Properties.Settings.Default.RECEIPT_DIR);

            _simpleContainer.Instance(_simpleContainer);

            _simpleContainer.PerRequest<ShellViewModel>();
            _simpleContainer.PerRequest<AddEditEmployeeViewModel>();
            _simpleContainer.PerRequest<EmployeesViewModel>();
            _simpleContainer.PerRequest<ReportingViewModel>();
            _simpleContainer.PerRequest<DTRViewModel>();
            _simpleContainer.PerRequest<FingerRegistrationViewModel>();
            _simpleContainer.PerRequest<DTRRepository>();

            _simpleContainer.Singleton<BiometContext>();

            _simpleContainer.PerRequest<ReportingRepository>();

            _simpleContainer.Singleton<IWindowManager, WindowManager>();
            _simpleContainer.Singleton<IEventAggregator, EventAggregator>();
            _simpleContainer.Instance(DialogCoordinator.Instance);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<AddEditEmployeeViewModel, SalariedEmployee>().ReverseMap();
                cfg.CreateMap<AddEditEmployeeViewModel, HourlyRatedEmployee>().ReverseMap();
            });

            base.Configure();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}

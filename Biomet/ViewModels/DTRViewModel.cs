﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Biomet.Repositories;
using Caliburn.Micro;
using DPFP;
using DPFP.Verification;
using MahApps.Metro.Controls.Dialogs;

namespace Biomet.ViewModels
{
    public class DTRViewModel : CaptureFingerViewModel
    {
        private Verification _verificator;

        public BindableCollection<DayLog> DayLogs { get; set; }

        public DTRViewModel(DTRRepository dtrRepository, IDialogCoordinator dialogCoordinator, IWindowManager windowManager)
        {
            SetupClock();
            DayLogs = new BindableCollection<DayLog>();
            _dtrRepository = dtrRepository;
            _dialogCoordinator = dialogCoordinator;
            _windowManager = windowManager;
            PropertyChanged += DTRViewModel_PropertyChanged;
            RefreshLogs();
        }

        private void RefreshLogs()
        {
            Task.Run(() =>
            {
                DayLogs.Clear();
                using (var db = new BiometContext())
                {
                    var _logdate = DateTime.Now.Date;
                    DayLogs.AddRange(db.DayLogs.Include("Employee").Where(d => d.LogDate == _logdate).ToList());
                }
            });
        }

        private void DTRViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedLogType):
                    {
                        Employee = null;
                        LogTime = "";
                        break;
                    }
            }
        }

        private void SetupClock()
        {
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (s, e) =>
            {
                DateNow = DateTime.Now.ToLongDateString();
                TimeNow = DateTime.Now.ToLongTimeString();
            };
            timer.Start();
        }

        private string _logTime;

        public string LogTime
        {
            get => _logTime;
            set => Set(ref _logTime, value);
        }


        protected override void Init()
        {
            base.Init();
            _verificator = new Verification();

            LoadTemplates();
        }

        private Dictionary<string, Template> _templates = new Dictionary<string, Template>();
        private DispatcherTimer timer;
        private string _dateTimeNow;
        private Employee _employee;
        private string _timeNow;
        private int _selectedLogType = 1;
        private readonly DTRRepository _dtrRepository;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IWindowManager _windowManager;

        public string DateNow { get => _dateTimeNow; private set => Set(ref _dateTimeNow, value); }
        public string TimeNow { get => _timeNow; private set => Set(ref _timeNow, value); }
        public Employee Employee { get => _employee; private set => Set(ref _employee, value); }

        private void LoadTemplates()
        {
            Directory.CreateDirectory(Properties.Settings.Default.FPTEMPLATE_DIR);
            _templates.Clear();
            var files = Directory.GetFiles(Properties.Settings.Default.FPTEMPLATE_DIR);
            foreach (var f in files)
            {
                var templateBytes = File.ReadAllBytes(f);
                using (var mem = new MemoryStream(templateBytes))
                {
                    var template = new Template();
                    template.DeSerialize(mem);
                    _templates.Add(Path.GetFileNameWithoutExtension(f), template);
                }
            }
        }

        private Dictionary<double, string> _logTypeLookup = new Dictionary<double, string>
        {
            {1,"Morning Time In" },
            {2,"Morning Time Out" },
            {3,"Afternoon Time In" },
            {4,"Afternoon Time Out" },
        };

        public string LogTypeText
        {
            get
            {
                if (SelectedLogType == 0)
                    return string.Empty;

                return _logTypeLookup[SelectedLogType];
            }
        }

        public int SelectedLogType
        {
            get => _selectedLogType; set
            {
                Set(ref _selectedLogType, value);
                NotifyOfPropertyChange(nameof(LogTypeText));
            }
        }

        protected override void Process(Sample Sample)
        {
            base.Process(Sample);
            var features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);
            if (features == null) return;

            var result = new Verification.Result();
            foreach (var storedTemplate in _templates)
            {
                _verificator.Verify(features, storedTemplate.Value, ref result);
                UpdateStatus(result.FARAchieved);

                if (!result.Verified) continue;
                OnFingerIdentified(storedTemplate.Key);
                return;
            }
        }

        private void OnFingerIdentified(string employeeNumber)
        {
            try
            {
                if (IsRequestingPaycheck)
                {
                    ProcessPaycheckRequest(employeeNumber);
                    IsRequestingPaycheck = false;
                }
                else
                {
                    Employee = _dtrRepository.Get(employeeNumber.Trim(), DateTime.Now.Date);
                    Employee.SetLogType(SelectedLogType);
                    LogTime = DateTime.Now.ToLongTimeString();
                    _dtrRepository.Save(Employee);
                    RefreshLogs();
                }
            }
            catch (Exception ex)
            {
                _dialogCoordinator.ShowMessageAsync(this, "Failure", ex.Message, MessageDialogStyle.Affirmative);
            }
        }



        private void UpdateStatus(int achievedFAR)
        {
            Debug.WriteLine("Far Achieved: " + achievedFAR);
        }

        #region PAYCHECK REQUEST


        private bool _isRequestingPaycheck;

        private DateTime? _requestedPaycheckDate;

        public DateTime? RequestedPaycheckDate
        {
            get => _requestedPaycheckDate;
            set => Set(ref _requestedPaycheckDate, value);
        }

        public bool IsRequestingPaycheck
        {
            get => _isRequestingPaycheck;
            set => Set(ref _isRequestingPaycheck, value);
        }

        public bool CanRequestPaycheck => IsRequestingPaycheck == false;

        public void RequestPaycheck()
        {
            IsRequestingPaycheck = true;
        }

        public void CancelRequestPaycheck()
        {
            IsRequestingPaycheck = false;
        }

        private void ProcessPaycheckRequest(string employeeNumber)
        {
            if (RequestedPaycheckDate == null)
            {
                _dialogCoordinator.ShowMessageAsync(this, "Error", "Please select a date.");
                return;
            }

            using (var db = new BiometContext())
            {
                var emp = db.Employees.SingleOrDefault(e => e.EmployeeNumber == employeeNumber);
                if (emp == null)
                    return;

                if (!emp.IsPayDay(RequestedPaycheckDate.Value))
                {
                    _dialogCoordinator.ShowMessageAsync(this, "Error", "Its not your pay day.");
                    return;
                }

                db.PaycheckRequests.Add(new PaycheckRequest
                {
                    EmployeeId = emp.Id,
                    PayDay = RequestedPaycheckDate.Value
                });
                db.SaveChanges();
            }
        }

        #endregion
    }
}

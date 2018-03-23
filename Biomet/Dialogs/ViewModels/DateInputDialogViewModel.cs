using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Dialogs.ViewModels
{
    class DateInputDialogViewModel : Screen
    {
        public DateInputDialogViewModel()
        {

        }

        private DateTime? _payDate;

        public DateTime? PayDate
        {
            get => _payDate; set
            {
                Set(ref _payDate, value);
                NotifyOfPropertyChange(nameof(CanAccept));
            }
        }

        public bool CanAccept => PayDate.HasValue;

        public void Accept()
        {
            TryClose(true);
        }

        public void Cancel()
        {
            PayDate = null;
            TryClose(false);
        }
    }
}

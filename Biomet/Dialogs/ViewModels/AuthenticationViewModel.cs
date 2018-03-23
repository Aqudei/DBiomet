using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Biomet.Dialogs.ViewModels
{
    class AuthenticationViewModel : Screen
    {
        private string _message;
        private string _password;
        private string _username;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                Message = "";
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                Message = "";
            }
        }

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public void Login()
        {
            if (Username == "admin" && Password == "admin")
            {
                TryClose(true);
            }
            else
            {
                Message = "Invalid username / Password";
            }
        }

        public void CancelLogin()
        {
            TryClose(false);
        }
    }
}

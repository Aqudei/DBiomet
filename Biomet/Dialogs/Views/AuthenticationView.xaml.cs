using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Biomet.Dialogs.ViewModels;

namespace Biomet.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView : Window
    {
        public AuthenticationView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AuthenticationViewModel vm)
            {
                vm.Password = Password.Password;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

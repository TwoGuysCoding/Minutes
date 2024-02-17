using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Minutes.Services;

namespace Minutes.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl, IPasswordContainer
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public SecureString Password => PwdReg.SecurePassword;

        public void Dispose()
        {
            Password.Dispose();
        }
    }
}

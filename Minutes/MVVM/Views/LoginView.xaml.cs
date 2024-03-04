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
using System.Xml.Serialization;
using Minutes.Services;
using Minutes.Utils;
﻿using System.Windows.Controls;

namespace Minutes.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl, IPasswordContainer
    {
        private SecureString _password = new SecureString();
        public LoginView()
        {
            InitializeComponent();
            PwdLogin.PasswordChanged += HandlePasswordChanged;
            PwdReg.PasswordChanged += HandlePasswordChanged;
            
        }
 
        
        public void HandlePasswordChanged(object sender, EventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _password = passwordBox.SecurePassword; 
            }
        }

        public SecureString Password => _password;

        public void Dispose()
        {
            Password.Dispose();
        }
    }
}

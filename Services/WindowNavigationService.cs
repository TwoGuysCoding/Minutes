using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Minutes.Core;

namespace Minutes.Services
{
    internal class WindowNavigationService(Func<Type, Window> windowFactory) : IWindowNavigationService
    {
        public void ShowWindow<TWindowType>() where TWindowType : Window
        {
            var window = windowFactory(typeof(TWindowType));
            var viewModel = window.DataContext as ViewModel;
            viewModel?.OnNavigatedTo();
            window.Show();
        }

        public void CloseWindow<TWindowType>() where TWindowType : Window
        {
            var window = windowFactory(typeof(TWindowType));
            window.Hide();
        }
    }
}

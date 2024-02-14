using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.MVVM.Models;
using Minutes.Services;
using Minutes.Utils;
using NAudio.Wave;

namespace Minutes.MVVM.ViewModels
{
    /// <summary>
    /// Main view model for the application. Contains all the bindings and commands for the main window.
    /// </summary>
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private INavigationService _mainNavigationService;
        [ObservableProperty] private bool _isAlwaysTopWindow;

        public MainViewModel(INavigationService navService)
        {
            MainNavigationService = navService;
            NavigateToHome();
            Mediator.Instance.Register("SetToAlwaysTopWindow", (value) =>
            {
                if (value is bool b)
                    IsAlwaysTopWindow = b;
                else
                    throw new InvalidCastException("Value is not a boolean");
            });
        }

        [RelayCommand]
        private void NavigateToHome()
        {
            MainNavigationService.NavigateTo<HomeViewModel>();
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            MainNavigationService.NavigateTo<LoginViewModel>();
        }

        [RelayCommand]
        private void NavigateToAlwaysTopWidget()
        {
            MainNavigationService.NavigateTo<AlwaysTopWidgetViewModel>();
        }
    }
}

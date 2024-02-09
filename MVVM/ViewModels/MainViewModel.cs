using System.Diagnostics;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.MVVM.Models;
using Minutes.Services;
using NAudio.Wave;

namespace Minutes.MVVM.ViewModels
{
    /// <summary>
    /// Main view model for the application. Contains all the bindings and commands for the main window.
    /// </summary>
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private INavigationService _mainNavigationService;

        public MainViewModel(INavigationService navService)
        {
            MainNavigationService = navService;
            NavigateToHome();
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
    }
}

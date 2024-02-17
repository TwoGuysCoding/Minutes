using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.Core;
using Minutes.MVVM.Models;
using Minutes.Services;
using Minutes.Utils;
using NAudio.Wave;

namespace Minutes.MVVM.ViewModels
{
    /// <summary>
    /// Main view model for the application. Contains all the bindings and commands for the main window.
    /// </summary>
    internal partial class MainViewModel : ViewModel
    {
        [ObservableProperty] private IMainNavigationService _mainNavigationService;
        [ObservableProperty] private bool _isAlwaysTopWindow;
        [ObservableProperty] private SizeToContent _sizeToContentState = SizeToContent.WidthAndHeight;
        [ObservableProperty] private ResizeMode _currentResizeMode = ResizeMode.CanResizeWithGrip;

        public MainViewModel(IMainNavigationService navService)
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

        public override void OnNavigatedTo()    // when the login screen will be implemented, this will be very likely changed
        {
            NavigateToHome();
        }
    }
}

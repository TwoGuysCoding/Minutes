using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.Core;
using Minutes.Services;
using System.Windows;

namespace Minutes.MVVM.ViewModels
{
    /// <summary>
    /// Main view model for the application. Contains all the bindings and commands for the main window.
    /// </summary>
    internal partial class MainViewModel : ViewModel
    {
        [ObservableProperty] private IMainNavigationService _mainNavigationService;

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

        public override void OnNavigatedTo()    // when the login screen will be implemented, this is likely to change
        {
            NavigateToHome();
        }
    }
}

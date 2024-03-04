using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;

namespace Minutes.Services
{
    public partial class NavigationService(Func<Type, ViewModel> viewModelFactory) : ObservableObject, ITextDisplayNavigationService, IMainNavigationService, IAlwaysTopWidgetNavigationService
    {

        [ObservableProperty]
        private ViewModel? _currentView;


        public void NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            var viewModel = viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
            viewModel.OnNavigatedTo();
        }
    }
}

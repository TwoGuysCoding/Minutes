using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Services;

namespace Minutes.MVVM.ViewModels
{
    internal partial class AlwaysTopWidgetMainViewModel : ViewModel
    {
        [ObservableProperty] private IAlwaysTopWidgetNavigationService _alwaysTopWidgetNavigationService;

        public AlwaysTopWidgetMainViewModel(IAlwaysTopWidgetNavigationService alwaysTopWidgetNavigationService)
        {
            AlwaysTopWidgetNavigationService = alwaysTopWidgetNavigationService;
        }

        private void NavigateToAlwaysTopWidgetHomeView()
        {
            AlwaysTopWidgetNavigationService.NavigateTo<AlwaysTopWidgetHomeViewModel>();
        }

        public override void OnNavigatedTo()
        {
            NavigateToAlwaysTopWidgetHomeView();
        }
    }
}

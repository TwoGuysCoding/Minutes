using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            NavigateToAlwaysTopWidgetHomeView();
        }

        [RelayCommand]
        private void NavigateToAlwaysTopWidgetHomeView()
        {
            AlwaysTopWidgetNavigationService.NavigateTo<AlwaysTopWidgetHomeViewModel>();
        }
    }
}

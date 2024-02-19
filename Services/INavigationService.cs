using Minutes.Core;

namespace Minutes.Services
{
    internal interface INavigationService
    {
        ViewModel CurrentView { get; }

        void NavigateTo<TViewModel>() where TViewModel : ViewModel;
    }
}

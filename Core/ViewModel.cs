using CommunityToolkit.Mvvm.ComponentModel;

namespace Minutes.Core
{
    public abstract class ViewModel : ObservableObject
    {
        public virtual void OnNavigatedTo() { }
    }
}

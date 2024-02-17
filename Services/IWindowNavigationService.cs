using System.Windows;

namespace Minutes.Services
{
    internal interface IWindowNavigationService
    {
        void ShowWindow<TWindowType>() where TWindowType : Window;
        void CloseWindow<TWindowType>() where TWindowType : Window;
    }
}

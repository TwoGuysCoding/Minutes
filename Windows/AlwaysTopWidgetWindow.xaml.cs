using System.Windows;
using System.Windows.Input;

namespace Minutes.Windows
{
    /// <summary>
    /// Interaction logic for AlwaysTopWidgetWindow.xaml
    /// </summary>
    public partial class AlwaysTopWidgetWindow : Window
    {
        public AlwaysTopWidgetWindow()
        {
            InitializeComponent();
        }

        private void Window_DragOver(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            DragMove();
        }
    }
}

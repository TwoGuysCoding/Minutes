using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.Core;
using Minutes.Services;
using Minutes.Utils;
using Minutes.Windows;

namespace Minutes.MVVM.ViewModels
{
    internal partial class AlwaysTopWidgetHomeViewModel : ViewModel
    {
        [ObservableProperty] private ImageSource _currentMicrophoneImage;
        private readonly IMainNavigationService _mainNavigationService;
        private readonly IWindowNavigationService _windowNavigationService;

        private readonly ImageSource _microphoneImage;
        private readonly ImageSource _microphoneImageCrossed;

        public AlwaysTopWidgetHomeViewModel(IMainNavigationService navigation, IWindowNavigationService windowNavigationService)
        {
            _mainNavigationService = navigation;

            Mediator.Instance.Register("SendRecordingStatus", UpdateRecordingStatus);

            var microphoneImage = new BitmapImage(new Uri(@"pack://application:,,,/Icons/Microphone.png", UriKind.Absolute));
            var microphoneImageCrossed = new BitmapImage(new Uri(@"pack://application:,,,/Icons/MicrophoneCrossed.png", UriKind.Absolute));
            _microphoneImage = microphoneImage;
            _microphoneImageCrossed = microphoneImageCrossed;
            CurrentMicrophoneImage = _microphoneImage;
            _windowNavigationService = windowNavigationService;
        }

        private void UpdateRecordingStatus(object? status)
        {
            UpdateMicrophoneImage((bool)status!);
        }

        private void UpdateMicrophoneImage(bool status)
        {
            CurrentMicrophoneImage = status ? _microphoneImage : _microphoneImageCrossed;
        }

        [RelayCommand]
        private void NavigateToHome()
        {
            _windowNavigationService.ShowWindow<MainWindow>();
            _windowNavigationService.CloseWindow<AlwaysTopWidgetWindow>();
        }
    }
}

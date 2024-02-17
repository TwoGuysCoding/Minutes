using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
        private readonly IRecordingService _recordingService;

        private readonly ImageSource _microphoneImage;
        private readonly ImageSource _microphoneImageCrossed;

        public AlwaysTopWidgetHomeViewModel(IMainNavigationService navigation, IWindowNavigationService windowNavigationService, IRecordingService recordingService)
        {
            _mainNavigationService = navigation;

            var microphoneImage = new BitmapImage(new Uri(@"pack://application:,,,/Icons/Microphone.png", UriKind.Absolute));
            var microphoneImageCrossed = new BitmapImage(new Uri(@"pack://application:,,,/Icons/MicrophoneCrossed.png", UriKind.Absolute));
            _microphoneImage = microphoneImage;
            _microphoneImageCrossed = microphoneImageCrossed;
            CurrentMicrophoneImage = _microphoneImage;
            _windowNavigationService = windowNavigationService;
            _recordingService = recordingService;
        }

        private void UpdateMicrophoneImage(bool isRecording)
        {
            CurrentMicrophoneImage = isRecording ? _microphoneImageCrossed : _microphoneImage;
        }

        [RelayCommand]
        private void NavigateToHome()
        {
            _windowNavigationService.ShowWindow<MainWindow>();
            _windowNavigationService.CloseWindow<AlwaysTopWidgetWindow>();
        }

        public override void OnNavigatedTo()
        {
            UpdateMicrophoneImage(_recordingService.IsRecording);
        }

        [RelayCommand]
        private void ToggleRecording()
        {
            _recordingService.ToggleRecording();
            Debug.WriteLine(_recordingService.IsRecording);
            UpdateMicrophoneImage(_recordingService.IsRecording);
        }
    }
}

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
using System.Windows.Threading;
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
        [ObservableProperty] private string _recordingTime = "00:00";
        private readonly IMainNavigationService _mainNavigationService;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly IRecordingService _recordingService;
        private readonly ITimerService _timerService;

        private readonly DispatcherTimer _dispatcher = new();

        private readonly ImageSource _microphoneImage;
        private readonly ImageSource _microphoneImageCrossed;

        public AlwaysTopWidgetHomeViewModel(IMainNavigationService navigation, IWindowNavigationService windowNavigationService,
            IRecordingService recordingService, ITimerService timerService)
        {
            _mainNavigationService = navigation;

            var microphoneImage = new BitmapImage(new Uri(@"pack://application:,,,/Icons/Microphone.png", UriKind.Absolute));
            var microphoneImageCrossed = new BitmapImage(new Uri(@"pack://application:,,,/Icons/MicrophoneCrossed.png", UriKind.Absolute));
            _microphoneImage = microphoneImage;
            _microphoneImageCrossed = microphoneImageCrossed;
            CurrentMicrophoneImage = _microphoneImage;
            _windowNavigationService = windowNavigationService;
            _recordingService = recordingService;
            _timerService = timerService;

            _dispatcher.Interval = TimeSpan.FromSeconds(.5f);
            _dispatcher.Tick += (sender, args) => UpdateRecordingTime();
            _dispatcher.Start();
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
            UpdateMicrophoneImage(_recordingService.IsRecording);
            if (_recordingService.IsRecording)
            {
                _timerService.StartTimer();
            }
            else
            {
                _timerService.StopTimer();
            }
        }

        private void UpdateRecordingTime()
        {
            RecordingTime = _timerService.ElapsedTime.ToString(@"mm\:ss");
        }
    }
}

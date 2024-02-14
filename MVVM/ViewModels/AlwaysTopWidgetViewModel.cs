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

namespace Minutes.MVVM.ViewModels
{
    internal partial class AlwaysTopWidgetViewModel : ViewModel
    {
        [ObservableProperty] private ImageSource _currentMicrophoneImage;
        private readonly INavigationService _mainNavigationService;

        private readonly ImageSource _microphoneImage;
        private readonly ImageSource _microphoneImageCrossed;

        public AlwaysTopWidgetViewModel(INavigationService navigation)
        {
            _mainNavigationService = navigation;

            Mediator.Instance.Register("SendRecordingStatus", UpdateRecordingStatus);

            var microphoneImage = new BitmapImage(new Uri(@"pack://application:,,,/Icons/Microphone.png", UriKind.Absolute));
            var microphoneImageCrossed = new BitmapImage(new Uri(@"pack://application:,,,/Icons/MicrophoneCrossed.png", UriKind.Absolute));
            _microphoneImage = microphoneImage;
            _microphoneImageCrossed = microphoneImageCrossed;
            CurrentMicrophoneImage = _microphoneImage;
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
            _mainNavigationService.NavigateTo<HomeViewModel>();
            Mediator.Instance.Send("SetToAlwaysTopWindow", false);
        }
    }
}

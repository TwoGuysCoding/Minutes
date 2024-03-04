using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.Core;
using Minutes.MVVM.Models;
using Minutes.Services;
using Minutes.Utils;
using Minutes.Windows;
using NAudio.Wave;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shell;
using System.Windows.Threading;
using Application = System.Windows.Application;

namespace Minutes.MVVM.ViewModels
{
    internal partial class HomeViewModel : ViewModel
    {
        private readonly IRecordingService _recordingService;
        [ObservableProperty] private List<string> _transcriptionFiles;
        private Dictionary<string, string> transDictionary;
        [ObservableProperty] private double[]? _audioLevels;
        [ObservableProperty] private ITextDisplayNavigationService _textDisplayNavigation;
        [ObservableProperty] private IMainNavigationService _mainNavigationService;
        private readonly IWindowNavigationService _windowNavigationService;
        private readonly ITimerService _timerService;
        private readonly ITranscriptionService _transcriptionService;

        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();

                switch (value)
                {
                    case 0:
                        NavigateToTranscriptionText();
                        break;
                    case 1:
                        NavigateToEnhancedTranscriptionText();
                        break;
                    case 2:
                        NavigateToSummaryText();
                        break;
                }
            }
        }


        [ObservableProperty] private bool _isRecording;

        [ObservableProperty] private bool _isDeviceCheckBoxChecked;
        [ObservableProperty] private bool _isDeviceCheckBoxEnabled = true;

        public HomeViewModel(ITextDisplayNavigationService navigation, IMainNavigationService mainNavigationService,
            IRecordingService recordingService, IWindowNavigationService windowNavigationService, ITimerService timerService,
            ITranscriptionService transcriptionService)
        {
            _windowNavigationService = windowNavigationService;
            TextDisplayNavigation = navigation;
            _mainNavigationService = mainNavigationService;
            _recordingService = recordingService;
            // Setting up both recording devices. Each device has its own audio format and recording handler, so
            // it has to be set up separately.
            _recordingService.ChangeRecordingDevice(RecordingDeviceType.WaveInEvent);
            _recordingService.SetAudioFormat(16000, 16, 1);
            _recordingService.InitializeRecordingHandler(RecordingHandler);
            _recordingService.ChangeRecordingDevice(RecordingDeviceType.WasapiLoopBackCapture); // after you change the device the setup does not persist
            _recordingService.SetAudioFormat(16000, 16, 1);
            _recordingService.InitializeRecordingHandler(RecordingHandler);
            NavigateToTranscriptionText();
            _recordingService = recordingService;
            _windowNavigationService = windowNavigationService;
            _timerService = timerService;
            _transcriptionService = transcriptionService;
            transDictionary = new Dictionary<string, string>()
            {
                { "Transcription 1", "This is the transcription for the first audio file" }, 
                { "Transcription 2", "This is the transcription for the second audio file" }
            };
            TranscriptionFiles = transDictionary.Keys.ToList();
            FullScreen();

            AudioLevels = FftAudioTransformer.CreateLowLevelArray(100);
        }

        [RelayCommand]
        private void FullScreen()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                WindowChrome.SetWindowChrome(Application.Current.MainWindow, new WindowChrome
                {
                    CaptionHeight = 0,
                    CornerRadius = new CornerRadius(10),
                    GlassFrameThickness = new Thickness(0),
                    ResizeBorderThickness = new Thickness(10)
                });
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                WindowChrome.SetWindowChrome(Application.Current.MainWindow, new WindowChrome
                {
                    CaptionHeight = 0,
                    CornerRadius = new CornerRadius(0),
                    GlassFrameThickness = new Thickness(0),
                    ResizeBorderThickness = new Thickness(0)
                });
            }
        }

        [RelayCommand]
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private async Task LoadMainView()
        {
            await _transcriptionService.OpenConnectionForTranscription();
        }

        [RelayCommand]
        private async Task UnloadMainView()
        {
            _recordingService.DisposeRecorder();
            await _transcriptionService.CloseConnectionForTranscription();
        }

        [RelayCommand]
        private void NavigateToSummaryText()
        {
            TextDisplayNavigation.NavigateTo<SummaryTextViewModel>();
        }

        [RelayCommand]
        private void NavigateToAlwaysTopWidget()
        {
            _windowNavigationService.ShowWindow<AlwaysTopWidgetWindow>();
            _windowNavigationService.CloseWindow<MainWindow>();
        }

        [RelayCommand]
        private void NavigateToTranscriptionText()
        {
            TextDisplayNavigation.NavigateTo<TranscriptionTextViewModel>();
        }

        [RelayCommand]
        private void NavigateToEnhancedTranscriptionText()
        {
            TextDisplayNavigation.NavigateTo<EnhancedTranscriptionTextViewModel>();
        }

        /// <summary>
        /// A command that handles recording audio. If it is not recording it opens the WebSocket connection and starts recording.
        /// If it is not recording, it closes the WebSocket connection and stops recording.
        /// </summary>
        [RelayCommand]
        private void Record()
        {
            // If not recording, start recording
            if (!IsRecording)
            {
                _recordingService.StartRecording();
                UpdateRecordingStatus();
                _timerService.StartTimer();
                AudioLevels = FftAudioTransformer.CreateLowLevelArray(100);
                IsDeviceCheckBoxEnabled = false;
            }
            else    // If recording, stop recording
            {
                _recordingService.StopRecording();
                UpdateRecordingStatus();
                _timerService.StopTimer();
                AudioLevels = FftAudioTransformer.CreateLowLevelArray(100);
                IsDeviceCheckBoxEnabled = true;
            }
        }

        /// <summary>
        /// Handles the RecordingStopped event of the audio recorder.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="WaveInEventArgs"/> instance containing the event data.</param>
        private async void RecordingHandler(object? sender, WaveInEventArgs e)
        {
            // Send the audio data to the server
            var buffer = new byte[e.BytesRecorded];
            Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
            var audioLevels = FftAudioTransformer.GetAudioLevels(buffer, .1d, 100, 0.16f);
            AudioLevels = audioLevels;
            await _transcriptionService.SendData(buffer);
        }

        [RelayCommand]
        private void ChangeRecordingDevice()
        {
            _recordingService.ChangeRecordingDevice(IsDeviceCheckBoxChecked ?
                RecordingDeviceType.WaveInEvent : RecordingDeviceType.WasapiLoopBackCapture);
        }

        public override void OnNavigatedTo()
        {
            UpdateRecordingStatus();
        }

        private void UpdateRecordingStatus()
        {
            IsRecording = _recordingService.IsRecording;
        }
    }
}

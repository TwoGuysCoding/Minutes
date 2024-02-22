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
        //private readonly WebsocketManager _transcriptionWebsocketManager;

        /// <summary>
        /// The audio recorder used for recording audio.
        /// </summary>
        private readonly IRecordingService _recordingService;


        /// <summary>
        /// The text displayed on the record button.
        /// </summary>
        [ObservableProperty] private List<string> _transcriptionFiles;
        private Dictionary<string, string> transDictionary;
        [ObservableProperty] private string _recordButtonText = "Start";
        [ObservableProperty] private string _stopWatchText = "00:00:00";
        [ObservableProperty] private string _summaryText = "The summary text will be displayed here";
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

        private readonly Stopwatch _stopwatch = new();
        private readonly DispatcherTimer _dispatcher = new();


        /// <summary>
        /// Indicates whether the application is currently recording audio.
        /// </summary>
        [ObservableProperty] private bool _isRecording;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HomeViewModel(ITextDisplayNavigationService navigation, IMainNavigationService mainNavigationService,
            IRecordingService recordingService, IWindowNavigationService windowNavigationService, ITimerService timerService,
            ITranscriptionService transcriptionService)
        {
            _windowNavigationService = windowNavigationService;
            TextDisplayNavigation = navigation;
            _mainNavigationService = mainNavigationService;
            _recordingService = recordingService;
            _recordingService.ChangeRecordingDevice(RecordingDeviceType.WasapiLoopBackCapture);
            _recordingService.SetAudioFormat(16000, 16, 1);
            _recordingService.InitializeRecordingHandler(RecordingHandler);
            NavigateToTranscriptionText();
            //_transcriptionWebsocketManager = new WebsocketManager("ws://localhost:8000/ws/transcribe_aai", DisplayTranscriptionText);
            _dispatcher.Tick += (s, a) => UpdateStopWatch();
            _dispatcher.Interval = new TimeSpan(0, 0, 0, 1, 0); // Update every second
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
            //await _transcriptionWebsocketManager.OpenConnectionAsync();
            await _transcriptionService.OpenConnectionForTranscription();
        }

        [RelayCommand]
        private async Task UnloadMainView()
        {
            _recordingService.DisposeRecorder();
            //await _transcriptionWebsocketManager.CloseConnectionAsync();
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
                RecordButtonText = "Stop";
                UpdateRecordingStatus();
                _timerService.StartTimer();
            }
            else    // If recording, stop recording
            {
                _recordingService.StopRecording();
                RecordButtonText = "Start";
                UpdateRecordingStatus();
                _timerService.StopTimer();
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

        private void UpdateStopWatch()
        {
            StopWatchText = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void DisplayTranscriptionText(string audioTranscript)
        {
            Mediator.Instance.Send("TranscriptionTextChanged", audioTranscript);
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

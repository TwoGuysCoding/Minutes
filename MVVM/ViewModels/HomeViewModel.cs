using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Threading;
using Minutes.Core;
using Minutes.MVVM.Models;
using Minutes.Services;
using NAudio.Wave;
using Minutes.Utils;

namespace Minutes.MVVM.ViewModels
{
    internal partial class HomeViewModel : ViewModel
    {
        /// <summary>
        /// The WebSocket manager used for managing WebSocket connections.
        /// </summary>
        private readonly WebsocketManager _transcriptionWebsocketManager;

        /// <summary>
        /// The audio recorder used for recording audio.
        /// </summary>
        private AudioRecorder _audioRecorder = new(16000, 16, 1);


        /// <summary>
        /// The text displayed on the record button.
        /// </summary>
        [ObservableProperty] private string _recordButtonText = "Start";
        [ObservableProperty] private string _stopWatchText = "00:00:00";
        [ObservableProperty] private string _summaryText = "The summary text will be displayed here";
        [ObservableProperty] private double[]? _audioLevels;
        [ObservableProperty] private ITextDisplayNavigationService _textDisplayNavigation;

        private readonly Stopwatch _stopwatch = new();
        private readonly DispatcherTimer _dispatcher = new();


        /// <summary>
        /// Indicates whether the application is currently recording audio.
        /// </summary>
        [ObservableProperty] private bool _isRecording;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HomeViewModel(ITextDisplayNavigationService navigation)
        {
            TextDisplayNavigation = navigation;
            NavigateToTranscriptionText();
            _transcriptionWebsocketManager = new WebsocketManager("ws://localhost:8000/ws/transcribe/vosk/en", DisplayTranscriptionText);
            _dispatcher.Tick += (s, a) => UpdateStopWatch();
            _dispatcher.Interval = new TimeSpan(0, 0, 0, 1, 0); // Update every second
        }

        [RelayCommand]
        private async Task LoadMainView()
        {
            await _transcriptionWebsocketManager.OpenConnectionAsync();
            _audioRecorder = new AudioRecorder(16000, 16, 1);
            _audioRecorder.InitializeRecorder(RecordingHandler);
        }

        [RelayCommand]
        private async Task UnloadMainView()
        {
            _audioRecorder.Dispose();
            await _transcriptionWebsocketManager.CloseConnectionAsync();
        }

        [RelayCommand]
        private void NavigateToSummaryText()
        {
            TextDisplayNavigation.NavigateTo<SummaryTextViewModel>();
        }

        [RelayCommand]
        private void NavigateToTranscriptionText()
        {
            TextDisplayNavigation.NavigateTo<TranscriptionTextViewModel>();
        }

        /// <summary>
        /// A command that handles recording audio. If it is not recording it opens the WebSocket connection and starts recording.
        /// If it is not recording, it closes the WebSocket connection and stops recording.
        /// </summary>
        [RelayCommand]
        private void Record()
        {
            // If not recording, start recording
            if (!_isRecording)
            {
                _audioRecorder.StartRecording();
                RecordButtonText = "Stop";
                IsRecording = true;
                _stopwatch.Start();
                _dispatcher.Start();
            }
            else    // If recording, stop recording
            {
                _audioRecorder.StopRecording();
                RecordButtonText = "Start";
                IsRecording = false;
                _stopwatch.Stop();
                _dispatcher.Stop();

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
            if (!_transcriptionWebsocketManager.IsOpen()) return;
            var buffer = new byte[e.BytesRecorded];
            Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
            var audioLevels = FftAudioTransformer.GetAudioLevels(buffer, .1d, 120, 0.16f);
            AudioLevels = audioLevels;
            await _transcriptionWebsocketManager.SendDataAsync(buffer);
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
    }
}

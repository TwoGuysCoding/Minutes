using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;

namespace Minutes
{
    /// <summary>
    /// Main view model for the application. Contains all the bindings and commands for the main window.
    /// </summary>
    internal partial class MainViewModel : ObservableObject
    {
        /// <summary>
        /// The WebSocket manager used for managing WebSocket connections.
        /// </summary>
        private readonly WebsocketManager _websocketManager;

        /// <summary>
        /// The audio recorder used for recording audio.
        /// </summary>
        private readonly AudioRecorder _audioRecorder = new(16000, 16, 1);

        /// <summary>
        /// The text displayed on the record button.
        /// </summary>
        [ObservableProperty]
        private string _recordButtonText = "Start";
        [ObservableProperty]
        private string _stopWatchText = "00:00:00";
        [ObservableProperty]
        private string _transcriptionText = "";

        private readonly Stopwatch _stopwatch = new();
        private readonly DispatcherTimer _dispatcher = new();

        /// <summary>
        /// Indicates whether the application is currently recording audio.
        /// </summary>
        private bool _isRecording;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _websocketManager = new WebsocketManager("ws://localhost:8000/ws/transcribe/vosk/en", DisplayTranscriptionText);
            _audioRecorder.InitializeRecorder(RecordingHandler);
            _dispatcher.Tick += (s, a) => UpdateStopWatch();
            _dispatcher.Interval = new TimeSpan(0, 0, 0, 1, 0); // Update every second
        }

        [RelayCommand]
        private async Task InitializeMainViewModel()
        {
            await _websocketManager.OpenConnectionAsync();
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
                _isRecording = true;
                _stopwatch.Start();
                _dispatcher.Start();
            }
            else    // If recording, stop recording
            {
                _audioRecorder.StopRecording();
                RecordButtonText = "Start";
                _isRecording = false;
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
            if (!_websocketManager.IsOpen()) return;
            var buffer = new byte[e.BytesRecorded];
            Buffer.BlockCopy(e.Buffer, 0, buffer, 0, e.BytesRecorded);
            await _websocketManager.SendDataAsync(buffer);
        }

        private void UpdateStopWatch()
        {
            StopWatchText = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        }

        private void DisplayTranscriptionText(string audioTranscript)
        {
            TranscriptionText += audioTranscript;
            TranscriptionText += " ";
        }
    }
}

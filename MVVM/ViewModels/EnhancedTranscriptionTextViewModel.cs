using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Services;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.MVVM.ViewModels
{
    internal partial class EnhancedTranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string _enhancedTranscriptionText = "The enhanced transcription text will be displayed here: \n\n";
        public ObservableCollection<TranscriptionBoxViewModel> TranscriptionBoxes { get; set; } = [];

        private readonly ITranscriptionService _transcriptionService;
        private readonly ITimerService _timerService;

        private string _previousTime = "00:00:00";

        public EnhancedTranscriptionTextViewModel(ITranscriptionService transcriptionService, ITimerService timerService)
        {
            _transcriptionService = transcriptionService;
            _transcriptionService.EnhancedTranscriptionTextChanged += (_, text) => DisplayEnhancedTranscriptionText(text);
            _timerService = timerService;
        }

        private void DisplayEnhancedTranscriptionText(string text)
        {
            EnhancedTranscriptionText = text;
            Application.Current?.Dispatcher.Invoke(() =>
            {
                TranscriptionBoxes.Add(new TranscriptionBoxViewModel
                {
                    Content = text,
                    Time = _previousTime + " - " + _timerService.ElapsedTime.ToString(@"hh\:mm\:ss")
                });
            });
            _previousTime = _timerService.ElapsedTime.ToString(@"hh\:mm\:ss");
        }

        private void UpdateLastTranscriptionBox(string content, string? time = null)
        {
            var lastBox = TranscriptionBoxes.Last();
            var updatedBox = new TranscriptionBoxViewModel
            {
                Content = content,
                Time = time ?? lastBox.Time
            };
            Application.Current?.Dispatcher.Invoke(() =>
            {
                var lastIndex = TranscriptionBoxes.Count - 1;
                TranscriptionBoxes[lastIndex] = updatedBox;
            });
        }
    }
}

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows;
using Minutes.Services;

namespace Minutes.MVVM.ViewModels
{
    internal partial class TranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string? _transcriptionText = string.Empty;

        private string? _transcriptionStorage;
        private string? _recentTranscription;
        private string? _partialTranscription;
        private string? _transcriptionBuffer;
        private readonly ITranscriptionService _transcriptionService;
        private readonly ITimerService _timerService;

        public ObservableCollection<TranscriptionBoxViewModel> TranscriptionBoxes { get; set; } = [];

        public TranscriptionTextViewModel(ITranscriptionService transcriptionService, ITimerService timerService)
        {
            _transcriptionService = transcriptionService;
            _transcriptionService.TranscriptionTextChanged += (_, text) => ReceiveMessages(text);
            _timerService = timerService;
            for (int i=0;i<99;i++)
            {
                TranscriptionBoxes.Add(new TranscriptionBoxViewModel
                {
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.\r\n\r\nWhy do we use it?\r\n",
                    Time = "00:00:00"
                });
            }
            
        }

        private void ReceiveMessages(string text)
        {
            if (TranscriptionBoxes.Count == 0)
            {
                Application.Current?.Dispatcher.Invoke(() =>
                    TranscriptionBoxes.Add(new TranscriptionBoxViewModel
                    {
                        Content = "",
                        Time = "00:00:00"
                    }));
            }
            var response = text ?? throw new NullReferenceException("Response is null");
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(response!);
            if (!jsonObject!.TryGetValue("type", out var type)) return;
            switch (type)
            {
                case "partial":
                    _partialTranscription = jsonObject["text"];
                    UpdateLastTranscriptionBox(_partialTranscription);
                    break;
                case "final":
                    _recentTranscription = jsonObject["text"];
                    _partialTranscription = null;
                    _transcriptionStorage += _timerService.ElapsedTime.ToString(@"hh\:mm\:ss") + '\n';
                    _transcriptionStorage += _recentTranscription + '\n' + '\n';
                    _transcriptionService.AppendEnhancedTranscriptionText(_recentTranscription ?? throw new InvalidOperationException());
                    UpdateLastTranscriptionBox(_recentTranscription, _timerService.ElapsedTime.ToString(@"hh\:mm\:ss"));
                    Application.Current?.Dispatcher.Invoke(() => 
                        TranscriptionBoxes.Add(new TranscriptionBoxViewModel
                    {
                        Content = "",
                        Time = _timerService.ElapsedTime.ToString(@"hh\:mm\:ss")
                    }));
                    break;
            }
            TranscriptionText = _transcriptionStorage + _partialTranscription;
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

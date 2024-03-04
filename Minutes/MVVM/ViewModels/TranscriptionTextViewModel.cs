using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
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

        public TranscriptionTextViewModel(ITranscriptionService transcriptionService, ITimerService timerService)
        {
            _transcriptionService = transcriptionService;
            _transcriptionService.TranscriptionTextChanged += (_, text) => ReceiveMessages(text);
            _timerService = timerService;
        }

        private void ReceiveMessages(string text)
        {
            var response = text ?? throw new NullReferenceException("Response is null");
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(response!);
            if (!jsonObject!.TryGetValue("type", out var type)) return;
            switch (type)
            {
                case "partial":
                    _partialTranscription = jsonObject["text"];
                    break;
                case "final":
                    _recentTranscription = jsonObject["text"];
                    _partialTranscription = null;
                    _transcriptionStorage += _timerService.ElapsedTime.ToString(@"hh\:mm\:ss") + '\n';
                    _transcriptionStorage += _recentTranscription + '\n' + '\n';
                    _transcriptionService.AppendEnhancedTranscriptionText(_recentTranscription ?? throw new InvalidOperationException());
                    break;
            }
            TranscriptionText = _transcriptionStorage + _partialTranscription;
        }

       
    }
}

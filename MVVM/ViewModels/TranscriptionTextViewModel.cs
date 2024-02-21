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
        private readonly ITranscriptionService _transcriptionService;

        public TranscriptionTextViewModel(ITranscriptionService transcriptionService)
        {
            _transcriptionService = transcriptionService;
            _transcriptionService.TranscriptionTextChanged += (_, text) => ReceiveMessages(text);
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
                    _transcriptionStorage += _recentTranscription + '\n';
                    _transcriptionService.AppendEnhancedTranscriptionText(TranscriptionText ?? throw new InvalidOperationException());
                    break;
            }
            TranscriptionText = _transcriptionStorage + _partialTranscription;
        }

       
    }
}

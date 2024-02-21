using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Services;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.MVVM.ViewModels
{
    internal partial class EnhancedTranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string _enhancedTranscriptionText = "The enhanced transcription text will be displayed here: \n";

        private readonly ITranscriptionService _transcriptionService;

        public EnhancedTranscriptionTextViewModel(ITranscriptionService transcriptionService)
        {
            _transcriptionService = transcriptionService;
            _transcriptionService.EnhancedTranscriptionTextChanged += (_, text) => DisplayEnhancedTranscriptionText(text);
        }

        private void DisplayEnhancedTranscriptionText(string text)
        {
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            if (jsonObject != null && jsonObject.TryGetValue("text", out var value))
            {
                EnhancedTranscriptionText += value;
            }
        }
    }
}

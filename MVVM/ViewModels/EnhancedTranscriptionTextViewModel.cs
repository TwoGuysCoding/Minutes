using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.MVVM.ViewModels
{
    internal partial class EnhancedTranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string _enhancedTranscriptionText = "The enhanced transcription text will be displayed here: \n";

        public EnhancedTranscriptionTextViewModel()
        {
            Mediator.Instance.Register("SendEnhancedTranscription", DisplayEnhancedTranscriptionText);
        }

        private void DisplayEnhancedTranscriptionText(object? text)
        {
            if (text is not string jsonString) return;
            var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            if (jsonObject != null && jsonObject.TryGetValue("text", out var value))
            {
                EnhancedTranscriptionText += value;
            }
        }
    }
}

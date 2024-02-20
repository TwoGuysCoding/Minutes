using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Minutes.MVVM.ViewModels
{
    internal partial class TranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string? _transcriptionText = string.Empty;

        private string? _transcriptionStorage;
        private string? _recentTranscription;
        private string? _partialTranscription;

        public TranscriptionTextViewModel()
        {
            Mediator.Instance.Register("TranscriptionTextChanged", ReceiveMessages);
        }

        private void ReceiveMessages(object? text)
        {
            if (text == null)
            {
                Debug.WriteLine("Tried to send null object to transcriptionViewModel");
                return;
            }

            var response = text as string ?? throw new NullReferenceException("Response is null");
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
                    _transcriptionStorage += _recentTranscription;
                    SendTranscriptionTextForEnhancement(_recentTranscription);
                    break;
            }
            TranscriptionText = _transcriptionStorage + _partialTranscription;
        }

        private async void SendTranscriptionTextForEnhancement(string? text)
        {
            try
            {
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(new { text = text ?? string.Empty }),
                    Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync("http://localhost:5000/demo/enhance_text", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Mediator.Instance.Send("SendEnhancedTranscription", responseString);
                    Debug.WriteLine($"Transcription sent successfully: {responseString}");
                }
                else
                {
                    Debug.WriteLine("Transcription failed to send");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occured while sending transcription: {e.Message}");
            }
        }
    }
}

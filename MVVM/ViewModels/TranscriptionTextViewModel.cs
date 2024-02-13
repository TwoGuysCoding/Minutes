using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.MVVM.ViewModels
{
    internal partial class TranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string? _transcriptionText = string.Empty;

        private string? _recentTranscription;

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
            _recentTranscription += text as string + " ";
            TranscriptionText += text as string;
            if (_recentTranscription.Length <= 500) return;

            SendTranscriptionTextForEnhancement(_recentTranscription);
            _recentTranscription = string.Empty;
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

using System.Diagnostics;
using System.Net.Http;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.MVVM.ViewModels
{
    internal partial class SummaryTextViewModel : ViewModel
    {
        [ObservableProperty] private string _summaryText = "The summary text will be displayed here: \n";

        public SummaryTextViewModel()
        {
            Mediator.Instance.Register("SendEnhancedTranscriptionForSummary", CreateSummaryText);
            Mediator.Instance.Send("GetSummary");
        }

        private async void CreateSummaryText(object? text)
        {
            try
            {
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(new { text = text as string ?? string.Empty }),
                    Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync("http://localhost:5000/demo/summary", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    SummaryText = responseString;
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

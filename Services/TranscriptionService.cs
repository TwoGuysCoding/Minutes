using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Minutes.MVVM.Models;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.Services
{
    internal class TranscriptionService : ITranscriptionService
    {
        private readonly WebsocketManager _websocket;

        private string? _enhancedTranscriptionBuffer;

        public TranscriptionService()
        {
            _websocket = new WebsocketManager("ws://localhost:8000/ws/transcribe_aai", ReceiveTranscriptionText);
        }

        private string? _transcriptionText;

        public string? TranscriptionText
        {
            get => _transcriptionText;
            set
            {
                _transcriptionText = value;
                TranscriptionTextChanged?.Invoke(this, value ?? throw new ArgumentNullException(nameof(value)));
            }
        }
        public event EventHandler<string>? TranscriptionTextChanged;

        private string? _enhancedTranscriptionText;

        public string? EnhancedTranscriptionText
        {
            get => _enhancedTranscriptionText;
            set
            {
                _enhancedTranscriptionText = value;
                EnhancedTranscriptionTextChanged?.Invoke(this, value ?? throw new ArgumentNullException(nameof(value)));
            }
        }
        public event EventHandler<string>? EnhancedTranscriptionTextChanged;

        private string? _summaryText;

        public string? SummaryText
        {
            get => _summaryText;
            set
            {
                _summaryText = value;
                SummaryTextChanged?.Invoke(this, value ?? throw new ArgumentNullException(nameof(value)));
            }
        }
        public event EventHandler<string>? SummaryTextChanged;

        public async Task OpenConnectionForTranscription()
        {
            await _websocket.OpenConnectionAsync();
        }

        public async Task CloseConnectionForTranscription()
        {
            await _websocket.CloseConnectionAsync();
        }

        public async Task SendData(byte[] data)
        {
            await _websocket.SendDataAsync(data);
        }

        private void ReceiveTranscriptionText(string text)
        {
            TranscriptionText = text;
        }

        public async Task AppendEnhancedTranscriptionText(string text)
        {
            _enhancedTranscriptionBuffer += text;
            if(_enhancedTranscriptionBuffer.Length < 400) return;
            try
            {
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(new { text = _enhancedTranscriptionBuffer }),
                    Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync("http://localhost:5000/demo/enhance_text", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                    if (jsonObject != null && jsonObject.TryGetValue("text", out var value))
                    {
                        EnhancedTranscriptionText += value;
                        _enhancedTranscriptionBuffer = string.Empty;
                    }
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

        public async Task AppendSummaryText(string text)
        {
            try
            {
                using var httpClient = new HttpClient();
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(new { text }),
                    Encoding.UTF8,
                    "application/json");
                var response = await httpClient.PostAsync("http://localhost:5000/demo/summary", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
                    if (jsonObject != null && jsonObject.TryGetValue("text", out var value))
                    {
                        SummaryText = value;
                    }
                    Debug.WriteLine($"Transcription sent successfully: {responseString}");
                }
                else
                {
                    Debug.WriteLine("Transcription failed to send");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occured while sending to summary: {e.Message}");
            }
        }
    }
}

using System.Diagnostics;
using System.Net.Http;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Minutes.Core;
using Minutes.Services;
using Minutes.Utils;
using Newtonsoft.Json;

namespace Minutes.MVVM.ViewModels
{
    internal partial class SummaryTextViewModel : ViewModel
    {
        [ObservableProperty] private string _summaryText = "The summary text will be displayed here: \n";
        private readonly ITranscriptionService _transcriptionService;

        public SummaryTextViewModel(ITranscriptionService transcriptionService)
        {
            _transcriptionService = transcriptionService;
            _transcriptionService.SummaryTextChanged += (_, text) => DisplaySummaryText(text);
        }

        private void DisplaySummaryText(string text)
        {
            SummaryText = text;
        }

        [RelayCommand]
        private void GenerateSummary()
        {
            if (_transcriptionService.EnhancedTranscriptionText != null)
                _transcriptionService.AppendSummaryText(_transcriptionService.EnhancedTranscriptionText);
        }
    }
}

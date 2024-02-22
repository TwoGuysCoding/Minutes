using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Minutes.Services
{
    internal interface ITranscriptionService
    {
        public string? TranscriptionText { get; protected set; }
        public event EventHandler<string> TranscriptionTextChanged;
        public string? EnhancedTranscriptionText { get; protected set; }
        public event EventHandler<string> EnhancedTranscriptionTextChanged;
        public string? SummaryText { get; protected set; }
        public event EventHandler<string> SummaryTextChanged;

        public Task OpenConnectionForTranscription();
        public Task CloseConnectionForTranscription();
        public Task SendData(byte[] data);
        public Task AppendEnhancedTranscriptionText(string text);
        public Task AppendSummaryText(string text);
    }
}

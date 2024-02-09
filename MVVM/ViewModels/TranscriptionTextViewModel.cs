using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;

namespace Minutes.MVVM.ViewModels
{
    internal partial class TranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string? _transcriptionText = string.Empty;

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
            TranscriptionText += (string)text;
            TranscriptionText += (string)" ";
        }
    }
}

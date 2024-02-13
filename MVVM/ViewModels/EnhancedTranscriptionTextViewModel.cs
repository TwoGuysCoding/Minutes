using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;
using Minutes.Utils;

namespace Minutes.MVVM.ViewModels
{
    partial class EnhancedTranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string _enhancedTranscriptionText = "The enhanced transcription text will be displayed here";

        public EnhancedTranscriptionTextViewModel()
        {
            Mediator.Instance.Register("SendEnhancedTranscription", DisplayEnhancedTranscriptionText);
        }

        private void DisplayEnhancedTranscriptionText(object? text)
        {
            EnhancedTranscriptionText += text as string;
        }
    }
}

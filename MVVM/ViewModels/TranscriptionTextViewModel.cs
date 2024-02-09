using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Minutes.Core;

namespace Minutes.MVVM.ViewModels
{
    internal partial class TranscriptionTextViewModel : ViewModel
    {
        [ObservableProperty] private string _transcriptionText = string.Empty;

        public TranscriptionTextViewModel()
        {
            HomeViewModel.TranscriptionTextChanged += HomeViewModel_TranscriptionTextChanged;
        }

        private void HomeViewModel_TranscriptionTextChanged(string text)
        {
            TranscriptionText += text;
            TranscriptionText += " ";
        }
    }
}

using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minutes.Services
{
    internal interface IRecordingDevice
    {
        void SetAudioFormat(int sampleRate, int bits, int channels);
        void StartRecording();
        void StopRecording();
        void Dispose();
        void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction);
    }
}

using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minutes.Core;

namespace Minutes.Services
{
    internal interface IRecordingService
    {
        void StartRecording();
        void StopRecording();
        void DisposeRecorder();
        void SetAudioFormat(int sampleRate, int bits, int channels);
        void ChangeRecordingDevice(RecordingDeviceType recordingDeviceType);
        void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction);
    }
}

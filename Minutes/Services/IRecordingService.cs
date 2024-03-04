using Minutes.Core;
using NAudio.Wave;

namespace Minutes.Services
{
    internal interface IRecordingService
    {
        void StartRecording();
        void StopRecording();
        void ToggleRecording();
        void DisposeRecorder();
        void SetAudioFormat(int sampleRate, int bits, int channels);
        void ChangeRecordingDevice(RecordingDeviceType recordingDeviceType);
        void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction);
        bool IsRecording { get; }
    }
}

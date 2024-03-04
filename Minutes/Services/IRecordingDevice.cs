using NAudio.Wave;

namespace Minutes.Services
{
    internal interface IRecordingDevice
    {
        void SetAudioFormat(int sampleRate, int bits, int channels);
        void StartRecording();
        void StopRecording();
        void Dispose();
        void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction);
        bool IsRecording { get; }
    }
}

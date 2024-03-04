using System.Diagnostics;
using Minutes.Core;
using NAudio.Wave;

namespace Minutes.Services
{
    internal class RecordingService(Func<RecordingDeviceType, IRecordingDevice> recordingDeviceFactory)
        : IRecordingService
    {
        private IRecordingDevice? _recordingDevice;
        private Action<object?, WaveInEventArgs>? _recordingFunction;
        private WaveFormat? _waveFormat;

        public void ToggleRecording()
        {
            if (IsRecording)
            {
                StopRecording();
            }
            else
            {
                StartRecording();
            }
        }

        public void DisposeRecorder()
        {
            _recordingDevice?.Dispose();
        }

        public void ChangeRecordingDevice(RecordingDeviceType recordingDeviceType)
        {
            _recordingDevice = recordingDeviceFactory(recordingDeviceType);
        }

        public void SetAudioFormat(int sampleRate, int bits, int channels)
        {
            _recordingDevice?.SetAudioFormat(sampleRate, bits, channels);
            _waveFormat = new WaveFormat(sampleRate, bits, channels);
        }

        public void StartRecording()
        {
            _recordingDevice?.StartRecording();
            Debug.WriteLine(_recordingDevice);
        }

        public void StopRecording()
        {
            _recordingDevice?.StopRecording();
        }

        public void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _recordingDevice?.InitializeRecordingHandler(recordingFunction);
            _recordingFunction = recordingFunction;
        }

        public bool IsRecording => _recordingDevice is { IsRecording: true };
    }
}

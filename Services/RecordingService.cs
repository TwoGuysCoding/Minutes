using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minutes.Core;
using NAudio.Wave;

namespace Minutes.Services
{
    internal class RecordingService(Func<RecordingDeviceType, IRecordingDevice> recordingDeviceFactory)
        : IRecordingService
    {
        private IRecordingDevice? _recordingDevice;

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
        }

        public void StartRecording()
        {
            _recordingDevice?.StartRecording();
        }

        public void StopRecording()
        {
            _recordingDevice?.StopRecording();   
        }

        public void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _recordingDevice?.InitializeRecordingHandler(recordingFunction);
        }
    }
}

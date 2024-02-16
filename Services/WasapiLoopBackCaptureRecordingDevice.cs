using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Minutes.Services
{
    internal class WasapiLoopBackCaptureRecordingDevice : IRecordingDevice
    {
        private readonly WasapiLoopbackCapture _wasapiLoopbackCapture = new();

        public void SetAudioFormat(int sampleRate, int bits, int channels)
        {
            _wasapiLoopbackCapture.WaveFormat = new WaveFormat(sampleRate, bits, channels);
        }

        public void StartRecording()
        {
            _wasapiLoopbackCapture.StartRecording();
        }

        public void StopRecording()
        {
            _wasapiLoopbackCapture.StopRecording();
        }

        public void Dispose()
        {
            _wasapiLoopbackCapture.Dispose();
        }

        public void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _wasapiLoopbackCapture.DataAvailable += new EventHandler<WaveInEventArgs>(recordingFunction);
        }

        public bool IsRecording => _wasapiLoopbackCapture.CaptureState == CaptureState.Capturing;
    }
}

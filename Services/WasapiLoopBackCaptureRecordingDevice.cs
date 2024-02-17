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
            IsRecording = false;
        }

        public void StartRecording()
        {
            _wasapiLoopbackCapture.StartRecording();
            IsRecording = true;
        }

        public void StopRecording()
        {
            _wasapiLoopbackCapture.StopRecording();
            IsRecording = false;
        }

        public void Dispose()
        {
            IsRecording = false;
            _wasapiLoopbackCapture.Dispose();
        }

        public void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _wasapiLoopbackCapture.DataAvailable += new EventHandler<WaveInEventArgs>(recordingFunction);
        }

        public bool IsRecording { private set; get; }
    }
}

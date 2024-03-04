using System.Diagnostics;
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
            Debug.WriteLine($"Started recoding from {this} using {_wasapiLoopbackCapture}. Recoding status: {_wasapiLoopbackCapture.CaptureState}");
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

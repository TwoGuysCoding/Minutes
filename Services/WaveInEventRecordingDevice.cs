using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Minutes.Services
{
    internal class WaveInEventRecordingDevice : IRecordingDevice
    {
        private readonly WaveInEvent _waveInEvent = new();

        public void SetAudioFormat(int sampleRate, int bits, int channels)
        {
            _waveInEvent.WaveFormat = new WaveFormat(sampleRate, bits, channels);
            IsRecording = false;
        }

        public void StartRecording()
        {
            _waveInEvent.StartRecording();
            IsRecording = true;
        }

        public void StopRecording()
        {
            _waveInEvent.StopRecording();
            IsRecording = false;
        }

        public void Dispose()
        {
            IsRecording = false;
            _waveInEvent.Dispose();
        }

        public void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _waveInEvent.DataAvailable += new EventHandler<WaveInEventArgs>(recordingFunction);
        }

        public bool IsRecording { get; private set; }
    }
}

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
        }

        public void StartRecording()
        {
            _waveInEvent.StartRecording();
        }

        public void StopRecording()
        {
            _waveInEvent.StopRecording();
        }

        public void Dispose()
        {
            _waveInEvent.Dispose();
        }

        public void InitializeRecordingHandler(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _waveInEvent.DataAvailable += new EventHandler<WaveInEventArgs>(recordingFunction);
        }
    }
}

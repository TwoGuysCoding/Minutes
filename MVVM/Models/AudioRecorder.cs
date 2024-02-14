using NAudio.Wave;

namespace Minutes.MVVM.Models
{
    /// <summary>
    /// A class that handles recording audio. To use it properly,
    /// you must first initialize it with the InitializeRecorder method,
    /// passing in a function that will handle the recording.
    /// Then, you can start recording with the StartRecording method,
    /// and stop recording with the StopRecording method.
    /// </summary>
    internal class AudioRecorder
    {
        private readonly WasapiLoopbackCapture _capture = new(); // the audio capture device

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioRecorder"/> class with specified sample rate, bits, and channels.
        /// </summary>
        /// <param name="sampleRate"></param>
        /// <param name="bits"></param>
        /// <param name="channels"></param>
        public AudioRecorder(int sampleRate, int bits, int channels)
        {
            var waveFormat = new WaveFormat(sampleRate, bits, channels);
            _capture.WaveFormat = waveFormat;
        }

        /// <summary>
        /// Initializes the recorder with a function that will handle the recording
        /// and the recording stopped event which will dispose of the recorder.
        /// </summary>
        /// <param name="recordingFunction">A function that will handle the recording</param>
        public void InitializeRecorder(Action<object?, WaveInEventArgs> recordingFunction)
        {
            _capture.DataAvailable += new EventHandler<WaveInEventArgs>(recordingFunction);
        }

        public void StartRecording()
        {
            _capture.StartRecording();
        }

        public void StopRecording()
        {
            _capture.StopRecording();
        }

        public void Dispose()
        {
            _capture.Dispose();
        }
    }
}

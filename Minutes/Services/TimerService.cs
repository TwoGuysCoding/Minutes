using System.Diagnostics;

namespace Minutes.Services
{
    internal class TimerService : ITimerService
    {
        private readonly Stopwatch _stopwatch = new();


        public void StartTimer()
        {
            _stopwatch.Start();
        }

        public void StopTimer()
        {
            _stopwatch.Stop();
        }

        public void ResetTimer()
        {
            _stopwatch.Reset();
        }

        public TimeSpan ElapsedTime => _stopwatch.Elapsed;
    }
}

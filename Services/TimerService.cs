using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

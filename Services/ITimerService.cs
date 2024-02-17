using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minutes.Services
{
    internal interface ITimerService
    {
        void StartTimer();
        void StopTimer();
        void ResetTimer();

        TimeSpan ElapsedTime { get; }
    }
}

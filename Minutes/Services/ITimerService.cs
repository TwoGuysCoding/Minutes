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

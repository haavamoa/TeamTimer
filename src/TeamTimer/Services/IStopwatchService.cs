using System;

namespace TeamTimer.Services
{
    public interface IStopwatchService : IDisposable
    {
        void RegisterInterval(double interval, Action callBack);

        void Start();

        void Pause();
    }
}
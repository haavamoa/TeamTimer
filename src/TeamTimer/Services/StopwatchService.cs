using System;
using System.Timers;

namespace TeamTimer.Services
{
    public class StopwatchService : IStopwatchService
    {
        private Timer m_timer;
        private Action m_callBack;

        public StopwatchService()
        {
            m_timer = new Timer();
        }
        
        public void RegisterInterval(double interval, Action callBack)
        {
            m_timer.Interval = interval;
            m_callBack = callBack;
            
            m_timer.Elapsed += TimerOnElapsed;
        }

        public void Start()
        {
            m_timer.Enabled = true;
        }

        public void Pause()
        {
            m_timer.Enabled = false;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            m_callBack?.Invoke();
        }

        public void Dispose()
        {
            m_timer.Elapsed -= TimerOnElapsed;
        }
    }
}
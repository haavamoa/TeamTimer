using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace TeamTimer.Services.Profiling
{
    public class ProfilerService : IProfilerService
    {
        public void RaiseEvent(string name)
        {
            Analytics.TrackEvent(name);
        }

        public void RaiseError(Exception exception)
        {
            Crashes.TrackError(exception);
        }

        public void RaiseError(Exception exception, Dictionary<string, string> properties)
        {
            Crashes.TrackError(exception, properties);
        }
    }
}
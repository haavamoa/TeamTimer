using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;

namespace TeamTimer.Services.Profiling
{
    public class ProfilerService : IProfilerService
    {
        public void RaiseEvent(string name)
        {
#if !DEBUG
            Analytics.TrackEvent(name);
#endif
        }

        public void RaiseError(Exception exception)
        {
#if !Debug
            Crashes.TrackError(exception);

#endif
        }

        public void RaiseError(Exception exception, Dictionary<string, string> properties)
        {
#if !DEBUG
            Crashes.TrackError(exception, properties);
#endif
        }
    }
}
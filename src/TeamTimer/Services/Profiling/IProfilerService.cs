using System;
using System.Collections.Generic;

namespace TeamTimer.Services.Profiling
{
    public interface IProfilerService
    {
        /// <summary>
        /// Raises an event to the profiling mechanism, the events should be available to profile what functionality the user is using 
        /// </summary>
        /// <param name="name">The name of the event</param>
        void RaiseEvent(string name);
        
        /// <summary>
        /// Raises an error to the profiling mechanism, the errors should be available for developers to monitor errors
        /// </summary>
        /// <param name="exception">The exception from the stacktrace</param>
        void RaiseError(Exception exception);
        
        /// <summary>
        /// Raises an error to the profiling mechanism, the errors should be available for developers to monitor errors
        /// </summary>
        /// <param name="exception">The exception from the stacktrace</param>
        /// <param name="properties">Additional properties to determine the context of the exception</param>
        void RaiseError(Exception exception, Dictionary<string, string> properties);
    }
}
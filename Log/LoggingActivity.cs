using System;

namespace Log
{
    public static class LogActivityExtensions
    {
        public static IDisposable Activity(this ILogger log, string format, params object[] args)
        {
            return new LoggingActivity(log, String.Format(format, args));
        }
    }

    class LoggingActivity : IDisposable
    {
        private readonly ILogger _log;
        private readonly string _activityName;
        private readonly IDisposable _scope;

        public LoggingActivity(ILogger log, string activityName)
        {
            _log = log;
            _activityName = activityName;

            _scope = _log.PushActivity(activityName); 
            log.DebugFormat(">> Entering activity [{0}]", activityName);            
        }

        public void Dispose()
        {
            _log.DebugFormat("<< Leaving activity [{0}]", _activityName);
            _scope.Dispose();
        }
    }
}

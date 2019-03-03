using System;
using System.Collections.Generic;

namespace Log
{
    class CachingLogManager : ILogManager
    {
        private readonly ILogManager _logManager;
        static readonly IDictionary<Type,ILogger> LoggerMap = new Dictionary<Type, ILogger>();

        public CachingLogManager( ILogManager logManager )
        {
            _logManager = logManager;
        }

        public ILogger GetLogger(Type type)
        {
            ILogger logger = null;
            if (! LoggerMap.TryGetValue(type, out logger))
            {
                logger = _logManager.GetLogger(type);
                LoggerMap[type] = logger;
            }

            return logger;
        }
    }
}

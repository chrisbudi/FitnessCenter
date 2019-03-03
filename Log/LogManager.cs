using System;
using System.IO;

namespace Log
{
    public class LogManager : ILogManager
    {
        static readonly ILogManager _logManager;
        
        static LogManager()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log.config"));
            _logManager = new LogManager(); 
        }
                
        public static ILogger GetLogger<T>()
        {
            return _logManager.GetLogger(typeof (T));
        }

        public ILogger GetLogger(Type type)
        {
            var logger = log4net.LogManager.GetLogger(type);
            return new LoggerAdapter( logger );
        }
    }
}

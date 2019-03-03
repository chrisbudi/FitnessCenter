using System;

namespace Log
{
    public interface ILogManager
    {
        ILogger GetLogger(Type type);
    }
}
#nullable enable
using System;
using Microsoft.Maui.Graphics;

class LoggingService : ILoggingService
{
    void ILoggingService.Log(LogType logType, string message)
    {
#if DEBUG
        Console.WriteLine(message);
#endif
    }

    void ILoggingService.Log(LogType logType, string message, Exception exception)
    {
#if DEBUG
        Console.WriteLine(message);
        Console.WriteLine(exception.Message);
        Console.WriteLine(exception.StackTrace);
#endif
    }
}

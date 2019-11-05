using Amazon.Lambda.Core;
using Microsoft.Extensions.Logging;
using System;

namespace ContactForm.AWSLambda
{
    class LoggerAdapter : ILogger
    {
        ILambdaLogger lambdaLogger;

        public LoggerAdapter(ILambdaLogger lambdaLogger)
        {
            this.lambdaLogger = lambdaLogger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = string.Empty;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }

            lambdaLogger.Log(message);
        }
    }
}

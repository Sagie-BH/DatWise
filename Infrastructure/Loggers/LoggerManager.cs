using Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Loggers
{
    public interface IAppLogger
    {
        void LogMessage(string message, LogLevel level);
        void SetNextLogger(IAppLogger nextLogger);
    }

    public abstract class AppLogger : IAppLogger
    {
        protected LogLevel logLevel;
        protected IAppLogger? nextLogger;
        private static readonly Dictionary<Type, SemaphoreSlim> _semaphoreGates = new();

        public AppLogger(LogLevel level)
        {
            logLevel = level;

            var loggerType = this.GetType();
            if (!_semaphoreGates.ContainsKey(loggerType))
            {
                _semaphoreGates[loggerType] = new SemaphoreSlim(1, 1);
            }
        }
        public void SetNextLogger(IAppLogger nextLogger)
        {
            this.nextLogger = nextLogger;
        }
        public void LogMessage(string message, LogLevel level)
        {
            if (CanLog(level))
            {
                var loggerType = this.GetType();
                _semaphoreGates[loggerType].Wait();
                try
                {
                    Write(message);
                }
                finally
                {
                    _semaphoreGates[loggerType].Release();
                }
            }
            nextLogger?.LogMessage(message, level);
        }

        protected bool CanLog(LogLevel level)
        {
            return this.logLevel == LogLevel.INFO || level > LogLevel.INFO;
        }

        abstract protected void Write(string message);
        abstract protected void LogException(Exception e);
    }


    public static class LoggerChainExtensions
    {
        public static IServiceCollection RegisterAppLoggers(this IServiceCollection services)
        {
            // Register individual loggers
            services.AddSingleton(sp => new DBLogger(LogLevel.ERROR));
            services.AddSingleton(sp => new FileLogger(LogLevel.INFO));
            services.AddSingleton(sp => new ConsoleLogger(LogLevel.INFO));
            services.AddSingleton(sp => new JsonLogger(LogLevel.INFO));


            // Register LoggerChain
            services.AddSingleton<IAppLogger>(serviceProvider =>
            {
                var dbLogger = serviceProvider.GetRequiredService<DBLogger>();
                var fileLogger = serviceProvider.GetRequiredService<FileLogger>();
                var consoleLogger = serviceProvider.GetRequiredService<ConsoleLogger>();
                var jsonLogger = serviceProvider.GetRequiredService<JsonLogger>();

                consoleLogger.SetNextLogger(fileLogger);
                fileLogger.SetNextLogger(jsonLogger);
                jsonLogger.SetNextLogger(dbLogger);

                return consoleLogger;
            });

            return services;
        }
    }


}

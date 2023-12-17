using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Loggers
{
    public class ConsoleLogger(LogLevel level) : AppLogger(level)
    {
        protected override void Write(string message)
        {
            Console.WriteLine("Standard Console::Logger: " + message);
        }
        protected override void LogException(Exception ex)
        {
            Console.WriteLine($"File::Logger:Exception: {ex.Message}; {(ex.InnerException != null ? $"\nInnerException: {ex.InnerException.Message}" : "")}");
        }
    }

    public class DBLogger(LogLevel level) : AppLogger(level)
    {
        protected override void Write(string message)
        {
            Console.WriteLine("Error Console::Logger: " + message);
        }
        protected override void LogException(Exception ex)
        {
            Console.WriteLine($"File::Logger:Exception: {ex.Message}; {(ex.InnerException != null ? $"\nInnerException: {ex.InnerException.Message}" : "")}");
        }
    }

}

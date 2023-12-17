using Core;
using Newtonsoft.Json;

namespace Infrastructure.Loggers
{
    public class JsonLogger(LogLevel level) : AppLogger(level)
    {
        protected override void Write(string message)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = message
            };
            WriteLogEntryToFile(logEntry);
        }

        protected override void LogException(Exception ex)
        {
            var logEntry = new LogEntry
            {
                Timestamp = DateTime.Now,
                Message = $"Exception: {ex.Message}",
                InnerException = ex.InnerException?.Message
            };
            WriteLogEntryToFile(logEntry);
        }

        private void WriteLogEntryToFile(LogEntry logEntry)
        {
            string fileName = GetFileName();
            string serializedLogEntry = JsonConvert.SerializeObject(logEntry) + "," + Environment.NewLine;

            File.AppendAllText(fileName, serializedLogEntry);
        }

        private string GetFileName()
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory; // Or specify another directory
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = Path.Combine(baseFolder, $"file-log-{date}.json");

            return fileName;
        }

        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Message { get; set; }
            public string? InnerException { get; set; }
        }
    }

}

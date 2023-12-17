using Core;

namespace Infrastructure.Loggers
{
    public class FileLogger(LogLevel level) : AppLogger(level)
    {
        protected override void Write(string message)
        {
            string fileName = GetFileName();
            string logMessage = $"[{DateTime.Now}] {message}";

            File.AppendAllText(fileName, logMessage + Environment.NewLine);
        }

        protected override void LogException(Exception ex)
        {
            string fileName = GetFileName();
            string logMessage = $"[{DateTime.Now}] Exception: {ex.Message}{(ex.InnerException != null ? $"\nInnerException: {ex.InnerException.Message}" : "")}";

            File.AppendAllText(fileName, logMessage + Environment.NewLine);
        }

        private string GetFileName()
        {
            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = Path.Combine(baseFolder, $"file-log-{date}.txt");

            return fileName;
        }
    }

}

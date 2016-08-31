using System;
using System.IO;
using System.Text;
using Edreamer.Framework.Context;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Logging;
using Edreamer.Framework.Media.Storage;
using Edreamer.Framework.Security;

namespace Rahnemun.Common.Logging
{
    public class FileLogger: ILogger
    {
        private readonly IStorageProvider _storageProvider;
        private readonly IWorkContextAccessor _workContextAccessor;
        private static readonly object Lock = new object();

        public FileLogger(IStorageProvider storageProvider, IWorkContextAccessor workContextAccessor)
        {
            _storageProvider = storageProvider;
            _workContextAccessor = workContextAccessor;
        }

        public bool IsEnabled(LogLevel level)
        {
            return level == LogLevel.Fatal || level == LogLevel.Error;
        }

        public void Log(LogLevel level, Exception exception, string message, params object[] args)
        {
            var filename = "Logs/" + GetCurrentTime("yy-MM-dd") + ".log";
            lock (Lock)
            {
                var file = _storageProvider.FileExists(filename)
                    ? _storageProvider.GetFile(filename)
                    : _storageProvider.CreateFile(filename);
                using (var stream = file.OpenWrite())
                {
                    
                    stream.Seek(0, SeekOrigin.End);
                    var buffer = Encoding.UTF8.GetBytes(GetLogMessage(exception, message));
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }

        private string GetLogMessage(Exception exception, string message)
        {
            var context = _workContextAccessor.Context;
            var request = context.CurrentHttpContext()?.Request;
            var user = context.CurrentUser()?.Username ?? "";
            var info = request != null ? (RequestInfoHelper.GetUserIP(request.ServerVariables) + " - " +RequestInfoHelper.GetUserAgent(request.ServerVariables)) : "";
            var url = request?.RawUrl ?? "";
            var sb = new StringBuilder();
            sb.AppendLine("========================================================================================================================================================================================================");
            sb.AppendLine("Timestamp: " + GetCurrentTime("yy/MM/dd HH:mm:ss"));
            sb.AppendLine("User: " + user);
            sb.AppendLine("Info: " + info);
            sb.AppendLine("Url: " + url);
            sb.AppendLine("Message: " + message);
            sb.AppendLine();
            var exceptionLevel = 1;
            while (true)
            {
                if (exception == null) break;
                sb.AppendLine("Exception Level {0}".FormatWith(exceptionLevel));
                sb.AppendLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                sb.AppendLine("Type: " + exception.GetType().FullName);
                sb.AppendLine("Message: " + exception.Message);
                if (exception.Data.Count > 0)
                {
                    sb.Append("Data: ");
                    foreach (var key in exception.Data.Keys)
                    {
                        if (key != null) sb.AppendFormat("[{0}: {1}]", key, exception.Data[key] ?? "-");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine("Stack: ");
                sb.AppendLine(exception.StackTrace);
                sb.AppendLine();
                exception = exception.InnerException;
                exceptionLevel++;
            }
            return sb.ToString();
        }

        private static string GetCurrentTime(string format)
        {
            var iranCurrentTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Iran Standard Time");
            return iranCurrentTime.ToString(format, new PersianCulture());
        }
    }
}
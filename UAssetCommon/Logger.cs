using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UAssetCommon
{
    public enum LogLevel
    {
        Error,
        Warning,
        Info,
        Trace
    }

    struct LogMessage
    {
        public LogLevel level;
        public string message;
    }

    public class Logger
    {
        private static readonly Logger Instance = new();
        private readonly List<ILogListener> Listeners = [];
        private List<LogMessage> messages = [];

        private Logger() 
        {
            var type = typeof(ILogListener);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
        }

        public static void AddListener(ILogListener listener)
        {
            Instance.Listeners.Add(listener);
            foreach (var message in Instance.messages)
            {
                SendMessage(message.level, message.message, listener);
            }
        } 

        private static void Log(LogLevel level, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg) 
        {
            string message = string.Format("[{0}] {1}: {2}",  level, DateTime.Now, string.Format(Console.Out.FormatProvider, format, arg));
            Instance.Listeners.ForEach(x => SendMessage(level, message, x));
            Instance.messages.Add(new LogMessage
            {
                level = level,
                message = message
            });
        }

        public static void SendMessage(LogLevel level, string message, ILogListener listener)
        {
            switch (level)
            {
                case LogLevel.Error: listener.OnError(message); break;
                case LogLevel.Warning: listener.OnWarning(message); break;
                case LogLevel.Info: listener.OnInfo(message); break;
                case LogLevel.Trace: listener.OnTrace(message); break;
            }

            listener.OnMessage(level, message);
        }

        private static void Log(LogLevel level, object obj)
        {
            string message = obj.ToString() ?? "null";
            Log(level, message);
        }

        public static void Warning([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
        {
            Log(LogLevel.Warning, format, arg);
        }

        public static void Error([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
        {
            Log(LogLevel.Error, format, arg);
        }

        public static void Info([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
        {
            Log(LogLevel.Info, format, arg);
        }

        public static void Trace([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] arg)
        {
            Log(LogLevel.Trace, format, arg);
        }

        public static void Warning(object obj)
        {
            Log(LogLevel.Warning, obj);
        }

        public static void Error(object obj)
        {
            Log(LogLevel.Error, obj);
        }

        public static void Info(object obj)
        {
            Log(LogLevel.Info, obj);
        }

        public static void Trace(object obj)
        {
            Log(LogLevel.Trace, obj);
        }
    }
}

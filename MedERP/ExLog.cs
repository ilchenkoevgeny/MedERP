using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MedERP
{
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
    }

    internal class ExLog
    {
        private static object _locker = new object();
        private static StreamWriter _sw;
        private static LogLevel _logLevel;
        private static Timer _changePathTimer;
        private static readonly int Changepathinterval = 60 * 1000;
        private static readonly string Logfilenameformat = "yyyyMMddHH";
        private static readonly string Loglineformat = "HH:mm:ss_ffff";

        static ExLog()
        {
            _changePathTimer = new Timer(state =>
            {
                _sw.Close();
                InitStreamWriter();
            }, null, Changepathinterval, Changepathinterval);
            InitStreamWriter();
        }

        public static void Dispose()
        {
            lock (_locker)
            {
                _changePathTimer.Dispose();
                _sw.Flush();
                _sw.Close();
            }
        }

        public static void SetLogLevel(LogLevel level)
        {
            _logLevel = level;
        }

        private static void InitStreamWriter()
        {
            _sw = new StreamWriter(GetLogFileName(), true, Encoding.UTF8, 1024);
            _sw.AutoFlush = true;
        }

        private static string GetLogFileName()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string file = DateTime.Now.ToString(Logfilenameformat) + ".txt";
            return Path.Combine(path, file);
        }

        public static void Debug(string s)
        {
            if ((int)_logLevel <= (int)LogLevel.Debug)
                Log(LogLevel.Debug, s);
        }

        public static void Debug(string format, params object[] args)
        {
            if ((int)_logLevel <= (int)LogLevel.Debug)
                Log(LogLevel.Debug, format, args);
        }

        public static void Info(string s)
        {
            if ((int)_logLevel <= (int)LogLevel.Info)
                Log(LogLevel.Info, s);
        }

        public static void Info(string format, params object[] args)
        {
            if ((int)_logLevel <= (int)LogLevel.Info)
                Log(LogLevel.Info, format, args);
        }

        public static void Warning(string s)
        {
            if ((int)_logLevel <= (int)LogLevel.Debug)
                Log(LogLevel.Warning, s);
        }

        public static void Warning(string format, params object[] args)
        {
            if ((int)_logLevel <= (int)LogLevel.Debug)
                Log(LogLevel.Warning, format, args);
        }

        public static void Error(string s)
        {
            if ((int)_logLevel <= (int)LogLevel.Error)
                Log(LogLevel.Error, s);
        }

        public static void Error(string format, params object[] args)
        {
            if ((int)_logLevel <= (int)LogLevel.Error)
                Log(LogLevel.Error, format, args);
        }

        private static void Log(LogLevel logLevel, string s)
        {
            lock (_locker)
            {
                _sw.WriteLine(WrapWithContext(logLevel, s));
            }
        }

        private static void Log(LogLevel logLevel, string format, params object[] args)
        {
            Log(logLevel, string.Format(format, args));
        }

        private static string WrapWithContext(LogLevel logLevel, string s)
        {
            StackTrace strackTrace = new StackTrace();
            StackFrame[] stackFrames = strackTrace.GetFrames();
            StackFrame stackFrame = null;
            for (int i = 0; i < stackFrames.Length; i++)
            {
                if (stackFrames[i].GetMethod().ReflectedType != typeof(ExLog))
                {
                    stackFrame = stackFrames[i];
                    break;
                }
            }

            MethodBase methodBase = stackFrame.GetMethod();
            string method = string.Format("{0}.{1}", methodBase.DeclaringType.Name, methodBase.Name);

            return string.Format("[{0}] @{3} #{4} {1} - {2}", logLevel, method, s, DateTime.Now.ToString(Loglineformat), Thread.CurrentThread.ManagedThreadId);
        }
    }
}

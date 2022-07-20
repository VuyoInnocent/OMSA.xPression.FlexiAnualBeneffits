using log4net;
using log4net.Config;

namespace SaGE.Common.Logging
{
    public class Log4NetLogger : ILog
    {
        private readonly ILog _log;

        public bool IsDebugEnabled
        {
            get
            {
                return _log.IsDebugEnabled;
            }
        }
        public bool IsInfoEnabled
        {
            get
            {
                return _log.IsInfoEnabled;
            }
        }
        public bool IsWarnEnabled
        {
            get
            {
                return _log.IsWarnEnabled;
            }
        }
        public bool IsErrorEnabled
        {
            get
            {
                return _log.IsErrorEnabled;
            }
        }
        public bool IsFatalEnabled
        {
            get
            {
                return _log.IsFatalEnabled;
            }
        }
        static Log4NetLogger()
        {
            if (!log4net.LogManager.GetRepository().Configured)
            {
                XmlConfigurator.Configure();
            }
        }
        public Log4NetLogger(System.Type type)
        {
            _log = LogManager.GetLogger(type);
        }
        public static void SetThreadContextProperty(string key, object value)
        {
            ThreadContext.Properties[key] = value;
        }
        public static void ClearThreadContextProperties()
        {
            ThreadContext.Properties.Clear();
        }
        public void Debug(object message)
        {
            _log.Debug(message);
        }
        public void Debug(object message, System.Exception exception)
        {
            _log.Debug(message, exception);
        }
        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }
        public void DebugFormat(string format, object arg0)
        {
            _log.DebugFormat(format, arg0);
        }
        public void DebugFormat(string format, object arg0, object arg1)
        {
            _log.DebugFormat(format, arg0, arg1);
        }
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.DebugFormat(format, arg0, arg1, arg2);
        }
        public void DebugFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }
        public void Info(object message)
        {
            _log.Info(message);
        }
        public void Info(object message, System.Exception exception)
        {
            _log.Info(message, exception);
        }
        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }
        public void InfoFormat(string format, object arg0)
        {
            _log.InfoFormat(format, arg0);
        }
        public void InfoFormat(string format, object arg0, object arg1)
        {
            _log.InfoFormat(format, arg0, arg1);
        }
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.InfoFormat(format, arg0, arg1, arg2);
        }
        public void InfoFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }
        public void Warn(object message)
        {
            _log.Warn(message);
        }
        public void Warn(object message, System.Exception exception)
        {
            _log.Warn(message, exception);
        }
        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }
        public void WarnFormat(string format, object arg0)
        {
            _log.WarnFormat(format, arg0);
        }
        public void WarnFormat(string format, object arg0, object arg1)
        {
            _log.WarnFormat(format, arg0, arg1);
        }
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.WarnFormat(format, arg0, arg1, arg2);
        }
        public void WarnFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(format, format, args);
        }
        public void Error(object message)
        {
            _log.Error(message);
        }
        public void Error(object message, System.Exception exception)
        {
            _log.Error(message, exception);
        }
        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }
        public void ErrorFormat(string format, object arg0)
        {
            _log.ErrorFormat(format, arg0);
        }
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            _log.ErrorFormat(format, arg0, arg1);
        }
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.ErrorFormat(format, arg0, arg1, arg2);
        }
        public void ErrorFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
        public void Fatal(object message, System.Exception exception)
        {
            _log.Fatal(message, exception);
        }
        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }
        public void FatalFormat(string format, object arg0)
        {
            _log.FatalFormat(format, arg0);
        }
        public void FatalFormat(string format, object arg0, object arg1)
        {
            _log.FatalFormat(format, arg0, arg1);
        }
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.FatalFormat(format, arg0, arg1, arg2);
        }
        public void FatalFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }
    }
}

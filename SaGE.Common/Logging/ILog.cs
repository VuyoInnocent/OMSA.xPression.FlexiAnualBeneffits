﻿namespace SaGE.Common.Logging
{
    public interface ILog
    {
        bool IsDebugEnabled
        {
            get;
        }
        bool IsInfoEnabled
        {
            get;
        }
        bool IsWarnEnabled
        {
            get;
        }
        bool IsErrorEnabled
        {
            get;
        }
        bool IsFatalEnabled
        {
            get;
        }
        void Debug(object message);
        void Debug(object message, System.Exception exception);
        void DebugFormat(string format, params object[] args);
        void DebugFormat(string format, object arg0);
        void DebugFormat(string format, object arg0, object arg1);
        void DebugFormat(string format, object arg0, object arg1, object arg2);
        void DebugFormat(System.IFormatProvider provider, string format, params object[] args);
        void Info(object message);
        void Info(object message, System.Exception exception);
        void InfoFormat(string format, params object[] args);
        void InfoFormat(string format, object arg0);
        void InfoFormat(string format, object arg0, object arg1);
        void InfoFormat(string format, object arg0, object arg1, object arg2);
        void InfoFormat(System.IFormatProvider provider, string format, params object[] args);
        void Warn(object message);
        void Warn(object message, System.Exception exception);
        void WarnFormat(string format, params object[] args);
        void WarnFormat(string format, object arg0);
        void WarnFormat(string format, object arg0, object arg1);
        void WarnFormat(string format, object arg0, object arg1, object arg2);
        void WarnFormat(System.IFormatProvider provider, string format, params object[] args);
        void Error(object message);
        void Error(object message, System.Exception exception);
        void ErrorFormat(string format, params object[] args);
        void ErrorFormat(string format, object arg0);
        void ErrorFormat(string format, object arg0, object arg1);
        void ErrorFormat(string format, object arg0, object arg1, object arg2);
        void ErrorFormat(System.IFormatProvider provider, string format, params object[] args);
        void Fatal(object message);
        void Fatal(object message, System.Exception exception);
        void FatalFormat(string format, params object[] args);
        void FatalFormat(string format, object arg0);
        void FatalFormat(string format, object arg0, object arg1);
        void FatalFormat(string format, object arg0, object arg1, object arg2);
        void FatalFormat(System.IFormatProvider provider, string format, params object[] args);
    }
}

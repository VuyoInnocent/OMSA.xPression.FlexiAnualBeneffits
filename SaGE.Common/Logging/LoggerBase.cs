using SaGE.Common.Utils;

namespace SaGE.Common.Logging
{
    public abstract class LoggerBase
    {
        private readonly ILog _logger;

        protected LoggerBase()
        {
            _logger = LogManager.GetLogger(GetType());
        }
        protected void LogInfo(string message)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(message);
            }
        }
        protected void LogInfo(string message, System.Exception e)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(message, e);
            }
        }
        protected void LogWarn(string message)
        {
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn(message);
            }
        }
        protected void LogWarn(string message, System.Exception e)
        {
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn(message, e);
            }
        }
        protected void LogError(string message)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(message);
            }
        }
        protected void LogError(string message, System.Exception e)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(message, e);
            }
        }
        protected void LogDebug(string message)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(message);
            }
        }
        protected void LogDebug(string message, System.Exception e)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(message, e);
            }
        }
        protected void LogFatal(string message)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.Fatal(message);
            }
        }
        protected void LogFatal(string message, System.Exception e)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.Fatal(message, e);
            }
        }
        protected void LogXml(string message, object data)
        {
            if (_logger.IsDebugEnabled)
            {
                if (data != null)
                {
                    string xml;
                    SerializationUtils.SerializeObject(data, out xml, true);
                    _logger.Debug(message + " - " + xml);
                }
            }
        }
    }
}

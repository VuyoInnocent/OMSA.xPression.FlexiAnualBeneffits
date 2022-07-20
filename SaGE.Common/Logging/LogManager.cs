namespace SaGE.Common.Logging
{
    public class LogManager
    {
        public static ILog GetLogger(System.Type type)
        {
            return new Log4NetLogger(type);
        }
    }
}

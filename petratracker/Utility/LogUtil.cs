
using System;
using log4net;
using log4net.Config;

namespace petratracker.Utility
{
    public static class LogUtil
    {
        #region Constructor

        static LogUtil()
        {
            XmlConfigurator.Configure();
        }

        #endregion

        /// <summary>
        /// A utility method to log debug information.
        /// </summary>
        /// <param name="className">The name of the class from where the log is being made.</param>
        /// <param name="methodName">The name of the method from where the log is being made.</param>
        /// <param name="information">The information text that needs to be logged.</param>
        public static void LogInfo(string className, string methodName, string information)
        {
            var logger = GetLogger(typeof(LogUtil));
            if (logger.IsDebugEnabled)
            {
                logger.Debug(string.Format("{0}.{1} --- {2}", className, methodName, information));
            }
        }

        /// <summary>
        /// A utility method to log exception details.
        /// </summary>
        /// <param name="className">The name of the class from where the log is being made.</param>
        /// <param name="methodName">The name of the method from where the log is being made.</param>
        /// <param name="exception">The instance of <see cref="System.Exception"/> that needs to be logged. </param>
        public static void LogError(string className, string methodName, Exception exception)
        {
            var logger = GetLogger(typeof(LogUtil));
            if (logger.IsErrorEnabled)
            {
                logger.Debug(string.Format("{0}.{1} --- {2}", className, methodName, exception));
            }
        }

        #region Private Methods

        /// <summary>
        ///     Gets the logger from the dictionary.
        /// </summary>
        /// <param name="source">The source of the logger.</param>
        /// <returns>Returns the <see cref="ILog" /> representing the logger for the source.</returns>
        private static ILog GetLogger(Type source)
        {
            if (null == _logger)
            {
                lock (SyncLock)
                {
                    if (null == _logger)
                    {
                        _logger = LogManager.GetLogger(source);
                    }
                }
            }
            return _logger;
        }

        #endregion

        #region Member Variables

        private static readonly object SyncLock = new object();
        private static volatile ILog _logger;

        #endregion
    }
}


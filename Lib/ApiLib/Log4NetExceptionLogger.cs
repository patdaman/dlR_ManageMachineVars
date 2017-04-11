using CommonUtils.Exception;
using CommonUtils.Logging;
using System;
using System.Web.Http.ExceptionHandling;

namespace ApiLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// Exception logger that outputs to a Log4Net logger. An ExceptionLogger can be used with a
    /// WebAPI 2.0 or greater project. It cannot be used with an MVC app.
    /// </summary>
    ///
    /// <remarks>   Pdelosreyes, 4/10/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly log4net.ILog iLog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoggingPatternUser logger = new LoggingPatternUser() { Logger = iLog };

        public override void Log(ExceptionLoggerContext context)
        {
            logger.Error("An unhandled exception occurred. Details follow.");
            logger.Error(String.Format("{0}\n\nStack Trace:\n{1}",
                ExamineException.GetInnerExceptionMessages(context.Exception),
                ExamineException.GetInnerExceptionStackTraces(context.Exception)));
        }


    }
}

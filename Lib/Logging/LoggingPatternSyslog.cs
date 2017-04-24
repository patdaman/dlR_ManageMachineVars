using log4net.Layout.Pattern;
///-------------------------------------------------------------------------------------------------
///-------------------------------------------------------------------------------------------------
using log4net.Core;
using System.IO;

namespace CommonUtils.Logging
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// A custom logging pattern for syslog. Syslog messages seem to be sent out each time there is a
    /// new line. Need to avoid additional newlines which will clutter up the log.
    /// </summary>
    ///
    /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class LoggingPatternSyslog : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent.MessageObject is CustomLoggingPatternPayload)
            {
                LoggingEvent le = loggingEvent;
                var xx = le.RenderedMessage;
                CustomLoggingPatternPayload lpp = (le.MessageObject as CustomLoggingPatternPayload);

                string output = "";

                if (lpp.IsCont)
                {
                    output = "..." + "[" + le.Level.Name + "]";
                }
                else
                {

                    output = "[" + le.ThreadName + "]" +
                        le.TimeStamp.ToString("yyyyMMdd HH:mm:ss.fff") + "[" + le.Level.Name + "] " +
                        " <" + le.LoggerName + "> ";
                }

                lpp.MessagePrefix = output;
                output = output + lpp.MessageObject.ToString();
                writer.Write(output);

            }
        }

    }
}

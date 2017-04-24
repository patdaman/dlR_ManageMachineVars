using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;

namespace CommonUtils.Logging
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A logging pattern. </summary>
    ///
    /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class LoggingPattern : PatternLayoutConverter
    {
        protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
        {
            if (loggingEvent.MessageObject is CustomLoggingPatternPayload)
            {
                LoggingEvent le = loggingEvent;
                CustomLoggingPatternPayload lpp = (le.MessageObject as CustomLoggingPatternPayload);
                string output = "";

                if (lpp.IsCont)
                {
                    output = "..." + "[" + le.Level.Name + "]" + lpp.MessageObject.ToString();
                }
                else
                {

                    output = writer.NewLine + 
                        "[" + le.ThreadName +  "]" +
                        le.TimeStamp.ToString("yyyyMMdd HH:mm:ss.fff") + "[" + le.Level.Name + "] " +                        
                        " <" + le.LoggerName + "> " + lpp.MessageObject.ToString();
                }

                writer.Write(output);

            }
        }
    }
}
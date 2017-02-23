///-------------------------------------------------------------------------------------------------
// <copyright file="LoggingPattern.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160211</date>
// <summary>Implements a custom logging pattern class</summary>
///-------------------------------------------------------------------------------------------------

using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Logging
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A logging pattern. </summary>
    ///
    /// <remarks>   Ssur, 20160211. </remarks>
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

///-------------------------------------------------------------------------------------------------
// <copyright file="LoggingPatternUser.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160211</date>
// <summary>Implements the logging pattern user class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CommonUtils.Logging
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A logging pattern user. </summary>
    ///
    /// <remarks>   Ssur, 20160301. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class LoggingPatternUser
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the logger. </summary>
        ///
        /// <value> The logger. </value>
        ///-------------------------------------------------------------------------------------------------

        public ILog Logger { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fatals. </summary>
        ///
        /// <remarks>   Ssur, 20160301. </remarks>
        ///
        /// <param name="Obj">      The object. </param>
        /// <param name="IsCont">   true if this object is container. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Fatal(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID};
            Logger.Fatal(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Debugs. </summary>
        ///
        /// <remarks>   Ssur, 20160301. </remarks>
        ///
        /// <param name="Obj">      The object. </param>
        /// <param name="IsCont">   true if this object is container. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Debug(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Debug(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Infoes. </summary>
        ///
        /// <remarks>   Ssur, 20160301. </remarks>
        ///
        /// <param name="Obj">      The object. </param>
        /// <param name="IsCont">   true if this object is container. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Info(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Info(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Errors. </summary>
        ///
        /// <remarks>   Ssur, 20160301. </remarks>
        ///
        /// <param name="Obj">      The object. </param>
        /// <param name="IsCont">   true if this object is container. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Error(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Error(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Warns. </summary>
        ///
        /// <remarks>   Ssur, 20160301. </remarks>
        ///
        /// <param name="Obj">      The object. </param>
        /// <param name="IsCont">   true if this object is container. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Warn(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Warn(lpp);
        }

    }
}

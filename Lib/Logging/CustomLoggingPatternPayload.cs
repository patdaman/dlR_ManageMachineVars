///-------------------------------------------------------------------------------------------------
// <copyright file="LoggingPatternPayload.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160301</date>
// <summary>Implements the logging pattern payload class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Logging
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A logging pattern payload. </summary>
    ///
    /// <remarks>   Ssur, 20160211. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class CustomLoggingPatternPayload
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is container. </summary>
        ///
        /// <value> true if this object is container, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsCont { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the message object. </summary>
        ///
        /// <value> The message object. </value>
        ///-------------------------------------------------------------------------------------------------

        public object MessageObject { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier of the tracking. </summary>
        ///
        /// <value> The identifier of the tracking. </value>
        ///-------------------------------------------------------------------------------------------------

        public String TrackingID { get; set; }
        public String MessagePrefix { get; set; }

        public override string ToString()
        {
            return MessageObject.ToString();
        }
    }
}

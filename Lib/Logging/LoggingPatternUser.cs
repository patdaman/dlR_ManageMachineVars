using log4net;

namespace CommonUtils.Logging
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A logging pattern user. </summary>
    ///
    /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class LoggingPatternUser
    {
        public ILog Logger { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Fatals. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="Obj">          The object. </param>
        /// <param name="IsCont">       (Optional) true if this object is container. </param>
        /// <param name="trackingID">   (Optional) Identifier for the tracking. </param>
        ///-------------------------------------------------------------------------------------------------
        public void Fatal(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID};
            Logger.Fatal(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Debugs. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="Obj">          The object. </param>
        /// <param name="IsCont">       (Optional) true if this object is container. </param>
        /// <param name="trackingID">   (Optional) Identifier for the tracking. </param>
        ///-------------------------------------------------------------------------------------------------
        public void Debug(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Debug(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Infoes. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="Obj">          The object. </param>
        /// <param name="IsCont">       (Optional) true if this object is container. </param>
        /// <param name="trackingID">   (Optional) Identifier for the tracking. </param>
        ///-------------------------------------------------------------------------------------------------
        public void Info(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Info(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Errors. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="Obj">          The object. </param>
        /// <param name="IsCont">       (Optional) true if this object is container. </param>
        /// <param name="trackingID">   (Optional) Identifier for the tracking. </param>
        ///-------------------------------------------------------------------------------------------------
        public void Error(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Error(lpp);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Warns. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/24/2017. </remarks>
        ///
        /// <param name="Obj">          The object. </param>
        /// <param name="IsCont">       (Optional) true if this object is container. </param>
        /// <param name="trackingID">   (Optional) Identifier for the tracking. </param>
        ///-------------------------------------------------------------------------------------------------
        public void Warn(object Obj, bool IsCont = false, string trackingID=null)
        {
            CustomLoggingPatternPayload lpp = new CustomLoggingPatternPayload() { IsCont = IsCont, MessageObject = Obj, TrackingID= trackingID };
            Logger.Warn(lpp);
        }

    }
}

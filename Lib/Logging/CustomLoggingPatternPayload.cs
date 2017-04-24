using System;

namespace CommonUtils.Logging
{
    public class CustomLoggingPatternPayload
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether this object is container. </summary>
        ///
        /// <value> true if this object is container, false if not. </value>
        ///-------------------------------------------------------------------------------------------------

        public bool IsCont { get; set; }
        public object MessageObject { get; set; }
        public String TrackingID { get; set; }
        public String MessagePrefix { get; set; }

        public override string ToString()
        {
            return MessageObject.ToString();
        }
    }
}

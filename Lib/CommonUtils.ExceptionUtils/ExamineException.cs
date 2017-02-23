using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Exception
{
    public class ExamineException
    {
        /// <summary>
        /// Returns, as a single string, an error message that consists of all InnerExceptions that are 
        /// associated with the given exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetInnerExceptionMessages(System.Exception ex)
        {
            string msg = "";            
            if (ex != null)
            {
                msg += "Exception: " + ex.Message;
                System.Exception innerException = ex.InnerException;
                while (innerException != null)
                {
                    msg += "\n\nInnerException: " + innerException.Message;
                    innerException = innerException.InnerException;
                }
            }
            return msg;
        }

        public static string GetInnerExceptionStackTraces(System.Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            
            if( ex != null )
            {
                int level = 0;                
                builder.AppendLine(" InnerException StackTrace level " + level++);
                builder.AppendLine(ex.StackTrace);
                System.Exception innerException = ex.InnerException;
                while( innerException != null )
                {
                    builder.AppendLine(" InnerException StackTrace level " + level++);
                    builder.AppendLine(ex.StackTrace);
                    innerException = innerException.InnerException;
                }
            }
            return builder.ToString(); 
        }

        public static string GetInnerExceptionAndStackTrackMessage(System.Exception ex)
        {
            return String.Format("{0}\n\nStacktrace:{1}",
                GetInnerExceptionMessages(ex),
                GetInnerExceptionStackTraces(ex)
                );
        }
    }
}

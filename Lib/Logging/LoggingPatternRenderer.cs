using log4net.ObjectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CommonUtils.Logging
{
    public class LoggingPatternRenderer : IObjectRenderer
    {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            CustomLoggingPatternPayload pp = obj as CustomLoggingPatternPayload;
            writer.Write(pp.ToString());
        }
    }
}

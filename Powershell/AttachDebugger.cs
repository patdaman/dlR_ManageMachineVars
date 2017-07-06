///-------------------------------------------------------------------------------------------------
// file:	AttachDebugger.cs
//
// summary:	Implements the attach debugger class
// 
// Help:
//  From command add a call to this before your command
///-------------------------------------------------------------------------------------------------
namespace DevOps
{
    public static class DebugHelper
    {
        public static void AttachDebugger()
        {
            System.Diagnostics.Debugger.Launch();
        }
    }
}

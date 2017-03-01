using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PowershellScript
    {
        public int? ScriptId { get; set; }
        public string ScriptName { get; set; }
        public string ScriptText { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsActive { get; set; }

        public PowershellScript()
        { }

        public PowershellScript(PowershellScript x)
        {
            ScriptId = x.ScriptId;
            ScriptName = x.ScriptName;
            ScriptText = x.ScriptText;
            CreateDate = x.CreateDate ?? DateTime.Now;
            LastModified = x.LastModified;
            IsActive = x.IsActive ?? true;
        }

        public PowershellScript(string title, string body)
        {
            ScriptId = null;
            ScriptName = title;
            ScriptText = body;
            CreateDate = DateTime.Now;
            LastModified = null;
            IsActive = true;
        }
    }

    public class PowershellScriptExecution
    {
        public int? MachineId { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public int? ScriptId { get; set; }
        public string ScriptName { get; set; }
        public string ScriptText { get; set; }
        public DateTime ExecuteDateTime { get; set; }
        public bool ViewOnly { get; set; }
        public string Output { get; set; }

        public PowershellScriptExecution()
        { }

        public PowershellScriptExecution(PowershellScriptExecution x)
        {
            MachineId = x.MachineId;
            MachineName = x.MachineName;
            UserName = x.UserName;
            ScriptId = x.ScriptId;
            ScriptText = x.ScriptText;
            ExecuteDateTime = x.ExecuteDateTime;
            ViewOnly = x.ViewOnly;
            Output = x.Output;
        }

        public PowershellScriptExecution(PowershellScript x)
        {
            MachineId = null;
            MachineName = Environment.MachineName;
            UserName = Environment.UserName;
            ScriptId = ScriptId ?? null;
            ScriptName = x.ScriptName;
            ScriptText = x.ScriptText;
            ExecuteDateTime = DateTime.Now;
            ViewOnly = true;
            Output = string.Empty;
        }
    }
}

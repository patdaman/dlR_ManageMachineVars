using CommonUtils.Powershell;
using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer
{
    public class ManagePowershell_Scripts
    {
        private PowershellTools scriptManager { get; set; }
        private DevOpsEntities devOpsContext { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string machineName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManagePowershell_Scripts()
        {
            devOpsContext = new DevOpsEntities();
            scriptManager = new PowershellTools();
            if (string.IsNullOrWhiteSpace(this.machineName))
                this.machineName = Environment.MachineName;
        }

        public ManagePowershell_Scripts(DevOpsEntities entities)
        {
            devOpsContext = entities;
            scriptManager = new PowershellTools();
            if (string.IsNullOrWhiteSpace(this.machineName))
                this.machineName = Environment.MachineName;
        }

        public ManagePowershell_Scripts(string conn)
        {
            devOpsContext = new DevOpsEntities(conn);
            scriptManager = new PowershellTools();
            if (string.IsNullOrWhiteSpace(this.machineName))
                this.machineName = Environment.MachineName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all scripts. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <returns>   all scripts. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<PowershellScript> GetAllScripts()
        {
            List<PowershellScript> scripts = new List<PowershellScript>();
            List<Script> efScripts = devOpsContext.Scripts.ToList();
            foreach (var script in efScripts)
            {
                scripts.Add(new PowershellScript()
                {
                    ScriptId = script.id,
                    ScriptName = script.script_name,
                    ScriptText = script.script_text,
                    CreateDate = script.create_date,
                    LastModified = script.modify_date,
                    IsActive = script.is_active,
                });
            }
            return scripts;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a script. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   The script. </returns>
        ///-------------------------------------------------------------------------------------------------
        public PowershellScript GetScript(int id)
        {
            Script efScript = devOpsContext.Scripts.Where(x => x.id == id).FirstOrDefault();
            return new PowershellScript()
            {
                ScriptId = efScript.id,
                ScriptName = efScript.script_name,
                ScriptText = efScript.script_text,
                CreateDate = efScript.create_date,
                LastModified = efScript.modify_date,
                IsActive = efScript.is_active
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a script. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="name"> The name. </param>
        ///
        /// <returns>   The script. </returns>
        ///-------------------------------------------------------------------------------------------------
        public PowershellScript GetScript(string name)
        {
            Script efScript = devOpsContext.Scripts.Where(x => x.script_name == name).FirstOrDefault();
            return new PowershellScript()
            {
                ScriptId = efScript.id,
                ScriptName = efScript.script_name,
                ScriptText = efScript.script_text,
                CreateDate = efScript.create_date,
                LastModified = efScript.modify_date,
                IsActive = efScript.is_active
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets script search. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="nameStartsWith"> The name begins with. </param>
        ///
        /// <returns>   The script search. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<PowershellScript> GetScriptSearch(string nameStartsWith)
        {
            List<Script> efScripts = devOpsContext.Scripts.Where(x => x.script_name.StartsWith(nameStartsWith)).ToList();
            List<PowershellScript> scripts = new List<PowershellScript>();
            foreach (var script in efScripts)
            {
                scripts.Add(new PowershellScript()
                {
                    ScriptId = script.id,
                    ScriptName = script.script_name,
                    ScriptText = script.script_text,
                    CreateDate = script.create_date,
                    LastModified = script.modify_date,
                    IsActive = script.is_active
                });
            }
            return scripts;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="scriptId">     Identifier for the script. </param>
        /// <param name="machineId">    Identifier for the machine. </param>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<string> ExecuteScript(int scriptId, int machineId)
        {
            Script efScript = devOpsContext.Scripts.Where(x => x.id == scriptId).FirstOrDefault();
            bool errors = false;
            List<string> scriptOutput = scriptManager.ExecuteScript(efScript.script_text);
            if (String.Join(",", scriptOutput).Contains("FullyQualifiedErrorId"))
            {
                errors = true;
            }
            ExecutionHistory log = new ExecutionHistory()
            {
                script_id = efScript.id,
                script_name = efScript.script_name,
                user_name = userName ?? Environment.UserName,
                execution_dt = DateTime.Now,
                contains_errors = errors,
                output = String.Join(Environment.NewLine, scriptOutput)
            };
            devOpsContext.ExecutionHistories.Add(log);
            devOpsContext.SaveChanges();
            return scriptOutput;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="scriptText">   The script text. </param>
        /// <param name="machineName">  (Optional) Name of the machine. </param>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<string> ExecuteScript(string scriptText, string machineName = null)
        {
            bool errors = false;
            if (string.IsNullOrWhiteSpace(machineName))
                if (!string.IsNullOrWhiteSpace(scriptManager.machineName))
                    machineName = scriptManager.machineName;
                else
                    machineName = System.Environment.MachineName.ToString();
            if (String.IsNullOrWhiteSpace(scriptManager.machineName))
                scriptManager = new PowershellTools(machineName);
            List<string> scriptOutput = scriptManager.ExecuteScript(scriptText);
            if (String.Join(",", scriptOutput).Contains("FullyQualifiedErrorId"))
            {
                errors = true;
            }
            ExecutionHistory log = new ExecutionHistory()
            {
                script_id = 0,
                script_name = null,
                user_name = userName ?? Environment.UserName,
                execution_dt = DateTime.Now,
                contains_errors = errors,
                output = String.Join(Environment.NewLine, scriptOutput)
            };
            devOpsContext.ExecutionHistories.Add(log);
            devOpsContext.SaveChanges();
            return scriptOutput;
        }
    }
}

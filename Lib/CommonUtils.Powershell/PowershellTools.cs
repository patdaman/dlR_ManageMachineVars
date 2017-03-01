using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace CommonUtils.Powershell
{
    public class PowershellTools
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the powershell script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="script">       The script. </param>
        /// <param name="parameters">   (Optional) Options for controlling the operation. </param>
        ///
        /// <returns>   A Collection&lt;PSObject&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public Collection<PSObject> RunPowershellScript(string script, List<KeyValuePair<string, string>> parameters = null)
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();

            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();

            //Here's how you add a new script with arguments
            Command myCommand = new Command(script);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    CommandParameter testParam = new CommandParameter(param.Key, param.Value);
                    myCommand.Parameters.Add(testParam);
                }
            }

            pipeline.Commands.Add(myCommand);

            // Execute PowerShell script
            var results = pipeline.Invoke();
            return results;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the powershell script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="scriptFile">       The script file. </param>
        /// <param name="scriptParameters"> Options for controlling the script. </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RunPowershellScript(string scriptFile, string scriptParameters)
        {
            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();
            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
            Pipeline pipeline = runspace.CreatePipeline();
            Command scriptCommand = new Command(scriptFile);
            Collection<CommandParameter> commandParameters = new Collection<CommandParameter>();
            foreach (string scriptParameter in scriptParameters.Split(' '))
            {
                CommandParameter commandParm = new CommandParameter(null, scriptParameter);
                commandParameters.Add(commandParm);
                scriptCommand.Parameters.Add(commandParm);
            }
            pipeline.Commands.Add(scriptCommand);
            Collection<PSObject> psObjects;
            psObjects = pipeline.Invoke();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Format ps output. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="psout">    The psout. </param>
        ///
        /// <returns>   The formatted ps output. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static List<string> FormatPSOutput(List<PSObject> psout)
        {
            List<string> psoutList = new List<string>();
            foreach (var result in psout)
            {
                psoutList.Add(FormatOutputObject(result));
            }
            return psoutList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Format output object. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="psoutLine">    The psout line. </param>
        ///
        /// <returns>   The formatted output object. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string FormatOutputObject(PSObject psoutLine)
        {
            var baseObj = psoutLine.BaseObject;
            if (baseObj is System.Diagnostics.Process)
            {
                var p = (System.Diagnostics.Process)baseObj;
                return string.Format("Handles:{0}, NPM:{1}, PM:{2}, etc", p.HandleCount, p.NonpagedSystemMemorySize64, p.PagedMemorySize64);
            }
            else
            {
                try
                {
                    return (psoutLine.ToString());
                }
                catch (Exception e)
                {
                    return "Output line could not be returned."
                        + Environment.NewLine
                        + "Exception:"
                        + Environment.NewLine
                        + e.Message;
                }
            }
            return string.Empty;
        }
    }
}

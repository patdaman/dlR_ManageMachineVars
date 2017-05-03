using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace CommonUtils.Powershell
{
    public class PowershellTools
    {
        public string machineName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public PowershellTools()
        {  }

        public PowershellTools(string machineName)
        {
            this.machineName = machineName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the powershell script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="script">       The script. </param>
        /// <param name="parameters">   (Optional) Options for controlling the operation. </param>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<string> ExecuteScript(string script, List<KeyValuePair<string, string>> parameters = null)
        {
            using (var execScript = new PowerShellEngine())
            {
                Collection<PSObject> runScript = execScript.ExecuteScript(script, parameters, machineName);
                return FormatPSOutput(runScript);
            }

            //Collection<PSObject> runScript = ExecuteScriptReturnObjects(script, parameters);
            //return FormatPSOutput(runScript);
        }

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
        public Collection<PSObject> ExecuteScriptReturnObjects(string script, List<KeyValuePair<string, string>> parameters = null)
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
        /// <summary>   Format ps output. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <param name="psout">    The psout. </param>
        ///
        /// <returns>   The formatted ps output. </returns>
        ///-------------------------------------------------------------------------------------------------
        private static List<string> FormatPSOutput(Collection<PSObject> psout)
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
        private static string FormatOutputObject(PSObject psoutLine)
        {
            string psResultLine;
            {
                try
                {
                    psResultLine = psoutLine.ToString();
                    //psResultLine = String.Join("|", psoutLine.Members["Id"].Value.ToString()
                    //                        , psoutLine.Members["ProcessName"].Value.ToString()
                    //                        , psoutLine.Members["PrivateMemorySize64"].Value.ToString()
                    //                        );
                }
                catch (Exception e)
                {
                    psResultLine = String.Join("|",
                        "Output line could not be returned.",
                        "Exception:",
                        e.Message);
                }
                //output.Add(psoutLine.ToString());
            }
            return psResultLine;
        }
    }
}

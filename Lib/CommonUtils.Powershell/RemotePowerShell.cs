using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Powershell
{
    public class PowerShellEngine : IDisposable
    {
        public string machineName { get; set; }
        public PSCredential credential { get; set; }

        private Dictionary<string, Runspace> _runspaceCache = new Dictionary<string, Runspace>();
        private int portNumber = 5986;
        private WSManConnectionInfo psConn;



        ~PowerShellEngine()
        {

            psConn = new WSManConnectionInfo(
            //useSsl
            true,
            //computerName,
            this.machineName,
            //port,
            this.portNumber,
            //appName,
            "/wsman",
            //shellUri,
            "http://schemas.microsoft.com/powershell/Microsoft.PowerShell",
            //credential
            new PSCredential("", new SecureString())
            );
            Clean();
        }

        public Collection<PSObject> ExecuteScriptFile(string scriptFilePath, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            return ExecuteScript(File.ReadAllText(scriptFilePath), arguments, machineAddress);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/1/2017. </remarks>
        ///
        /// <param name="script">           The script. </param>
        /// <param name="arguments">        (Optional) The arguments. </param>
        /// <param name="machineAddress">   (Optional) The machine address. </param>
        ///
        /// <returns>   A Collection&lt;PSObject&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public Collection<PSObject> ExecuteScript(string script, IEnumerable<object> arguments = null, string machineAddress = null)
        {
            //string password = string.Empty;
            //var secure = new SecureString();
            //foreach (char c in password)
            //{
            //    secure.AppendChar(c);
            //}
            //PSCredential remoteMachineCredentials = new PSCredential("", secure);
            int? portNumber = 5986;
            Runspace runspace = GetOrCreateRunspace(machineAddress);
            using (PowerShell ps = PowerShell.Create())
            {

                ps.Runspace = runspace;
                ps.AddScript(script);
                if (arguments != null)
                {
                    foreach (var argument in arguments)
                    {
                        ps.AddArgument(argument);
                    }
                }

                return ps.Invoke();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Executes the script operation. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/1/2017. </remarks>
        ///
        /// <param name="script">           The script. </param>
        /// <param name="parameters">       (Optional) Options for controlling the operation. </param>
        /// <param name="machineAddress">   (Optional) The machine address. </param>
        ///
        /// <returns>   A Collection&lt;PSObject&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public Collection<PSObject> ExecuteScript(string script, IEnumerable<KeyValuePair<string, string>> parameters = null, string machineAddress = null)
        {
            Runspace runspace = GetOrCreateRunspace(machineAddress);
            using (PowerShell ps = PowerShell.Create())
            {
                ps.Runspace = runspace;
                ps.AddScript(script);
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        ps.AddParameter(parameter.Key, parameter.Value);
                    }
                }

                return ps.Invoke();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/1/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public void Dispose()
        {
            Clean();
            GC.SuppressFinalize(this);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or create local runspace. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/1/2017. </remarks>
        ///
        /// <returns>   The or create local runspace. </returns>
        ///-------------------------------------------------------------------------------------------------
        private Runspace GetOrCreateLocalRunspace()
        {
            if (!_runspaceCache.ContainsKey("localhost"))
            {
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                _runspaceCache.Add("localhost", runspace);
            }

            return _runspaceCache["localhost"];
        }

        private Runspace GetOrCreateRunspace(string machineAddress)
        {
            if (string.IsNullOrWhiteSpace(machineAddress))
            {
                return GetOrCreateLocalRunspace();
            }

            machineAddress = machineAddress.ToLowerInvariant();
            if (!_runspaceCache.ContainsKey(machineAddress))
            {
                WSManConnectionInfo connectionInfo = new WSManConnectionInfo();
                connectionInfo.ComputerName = machineAddress;
                Runspace runspace = RunspaceFactory.CreateRunspace(connectionInfo);
                runspace.Open();
                _runspaceCache.Add(machineAddress, runspace);
            }

            return _runspaceCache[machineAddress];
        }

        private void Clean()
        {
            foreach (var runspaceEntry in _runspaceCache)
            {
                runspaceEntry.Value.Close();
            }

            _runspaceCache.Clear();
        }
    }
}

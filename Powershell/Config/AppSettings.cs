using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace DevOps
{
    [Cmdlet(VerbsCommon.Get, "AppSettings")]
    public class GetAppSettingsCmdlet : Cmdlet
    {
        private BusinessLayer.ManageIIS iisProcessor;

        #region parameters
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0
        )]
        [Alias("m")]
        public string machineName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [Alias("s")]
        public string siteName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [Alias("u")]
        public string username { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [Alias("p")]
        public string password { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true
        )]
        [Alias("d")]
        public string domain { get; set; }
        #endregion

        #region output
        private IEnumerable<ConfigKeyVal> _machineAppResults;
        #endregion
        #region overrides
        protected override void BeginProcessing()
        {

            base.BeginProcessing();

            iisProcessor = new BusinessLayer.ManageIIS();
            if (!string.IsNullOrWhiteSpace(this.username))
                iisProcessor.userName = this.username;
            if (!string.IsNullOrWhiteSpace(this.password))
                iisProcessor.password = this.password;
            if (!string.IsNullOrWhiteSpace(this.domain))
                iisProcessor.domain = this.domain;
        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            _machineAppResults = GetSiteAppSettings();
            _machineAppResults.ToList().ForEach(WriteObject);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
        #endregion

        #region private methods
        private List<ConfigKeyVal> GetSiteAppSettings()
        {
            if (string.IsNullOrWhiteSpace(this.username))
                iisProcessor.userName = this.username;
            if (string.IsNullOrWhiteSpace(this.password))
                iisProcessor.password = this.password;
            if (string.IsNullOrWhiteSpace(this.domain))
                iisProcessor.domain = this.domain;
            List<ConfigKeyVal> appSettingsList = new List<ConfigKeyVal>();
            if (this.siteName == null || !this.siteName.Any())
            {
                appSettingsList.AddRange(iisProcessor.GetAppSettings(machineName));
            }
            else
            {
                appSettingsList.AddRange(iisProcessor.GetAppSettings(machineName, siteName));
            }
            return appSettingsList;
        }
        #endregion
    }

    [Cmdlet(VerbsCommon.Set, "AppSettings")]
    public class SetAppSettingCmdlet: Cmdlet
    {
        private BusinessLayer.ManageIIS iisProcessor;
        private string[] trueValues = { "t", "true" };
        private string[] falseValues = { "f", "false" };
        private string addString;
        private bool addValue;
        private bool recycle;

        #region parameters
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("m")]
        public string machineName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("s")]
        public string siteName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("u")]
        public string username { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("p")]
        public string password { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("d")]
        public string domain { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        public string key { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        public string value { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("force")]
        public SwitchParameter forceAdd
        {
            get { return addValue; }
            set { addValue = value; }
        }
        #endregion

        #region input
        private List<IISAppSettings> _machineAppRequests;
        #endregion

        #region output
        private IEnumerable<ConfigKeyVal> _machineAppResults;
        #endregion
        #region overrides
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            iisProcessor = new BusinessLayer.ManageIIS();
            if (!string.IsNullOrWhiteSpace(this.username))
                iisProcessor.userName = this.username;
            if (!string.IsNullOrWhiteSpace(this.password))
                iisProcessor.password = this.password;
            if (!string.IsNullOrWhiteSpace(this.domain))
                iisProcessor.domain = this.domain;

            _machineAppRequests = new List<IISAppSettings>();
            List<ConfigKeyVal> configVals = new List<ConfigKeyVal>();
            if (this.siteName == null || !this.siteName.Any())
            {
                configVals.Add(new ConfigKeyVal()
                {
                    key = key,
                    value = value,
                });
                _machineAppRequests.Add(new IISAppSettings()
                {
                    serverName = machineName,
                    recycle = this.recycle,
                    configKeys = configVals,
                });
            }
            else
            {
                configVals.Add(new ConfigKeyVal()
                {
                    application = siteName,
                    key = key,
                    value = value,
                });
                _machineAppRequests.Add(new IISAppSettings()
                {
                    serverName = machineName,
                    name = siteName,
                    configKeys = configVals,
                });
            }
                if (this.addValue)
                    this.addString = "add";
        }
        protected override void ProcessRecord()
        {
            _machineAppResults = SetSiteAppSettings();
            _machineAppResults.ToList().ForEach(WriteObject);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
        #endregion

        #region private methods
        private IEnumerable<ConfigKeyVal> SetSiteAppSettings()
        {
            List<ConfigKeyVal> appSettingsList = new List<ConfigKeyVal>();
            appSettingsList.AddRange(iisProcessor.UpdateAppSettings(_machineAppRequests, addString));
            return appSettingsList;
        }
        #endregion
    }
}

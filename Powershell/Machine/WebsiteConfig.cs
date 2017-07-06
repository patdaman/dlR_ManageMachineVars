using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using ViewModel;

namespace DevOps
{
    [Cmdlet(VerbsCommon.Get, "SiteInformation")]
    [OutputType(typeof(IISAppSettings))]
    public class GetWebsiteConfigCmdlet : Cmdlet
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
        private IEnumerable<IISAppSettings> _machineAppResults;
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
            _machineAppResults = GetMachineAppSettings();
            _machineAppResults.ToList().ForEach(WriteObject);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
        #endregion

        #region private methods
        private IEnumerable<IISAppSettings> GetMachineAppSettings()
        {
            if (string.IsNullOrWhiteSpace(this.username))
                iisProcessor.userName = this.username;
            if (string.IsNullOrWhiteSpace(this.password))
                iisProcessor.password = this.password;
            if (string.IsNullOrWhiteSpace(this.domain))
                iisProcessor.domain = this.domain;
            List<IISAppSettings> appSettingsList = new List<IISAppSettings>();
            if (this.siteName == null || !this.siteName.Any())
            {
                appSettingsList.AddRange(iisProcessor.GetMachineApps(machineName));
            }
            else
            {
                appSettingsList.Add(iisProcessor.GetApplication(machineName, siteName));
            }
            return appSettingsList;
        }
        #endregion
    }

    [Cmdlet(VerbsCommon.Set, "Website")]
    [OutputType(typeof(IISAppSettings))]
    public class SetWebsiteConfigCmdlet : Cmdlet
    {
        private BusinessLayer.ManageIIS iisProcessor;
        private string[] trueValues = { "t", "true" };
        private string[] falseValues = { "f", "false" };
        private bool? keepAliveValue;
        private bool? startWebsiteValue;

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
        [Alias("k")]
        public string keepAlive { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("start")]
        public string startWebsite { get; set; }

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
        #endregion

        #region input
        private List<IISAppSettings> _machineAppRequests;
        #endregion

        #region output
        private IEnumerable<IISAppSettings> _machineAppResults;
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
            if (string.IsNullOrWhiteSpace(this.keepAlive))
                this.keepAlive = null;
            else
            {
                if (this.trueValues.Contains(this.keepAlive.ToLower()))
                    this.keepAliveValue = true;
                else if (falseValues.Contains(this.keepAlive.ToLower()))
                    this.keepAliveValue = false;
            }
            if (string.IsNullOrWhiteSpace(this.startWebsite))
                this.startWebsite = null;
            else
            {
                if (this.trueValues.Contains(this.startWebsite.ToLower()))
                    this.startWebsiteValue = true;
                else if (falseValues.Contains(this.startWebsite.ToLower()))
                    this.startWebsiteValue = false;
            }

            _machineAppRequests = new List<IISAppSettings>();
            if (this.siteName == null || !this.siteName.Any())
            {
                _machineAppRequests.Add(new IISAppSettings()
                {
                    serverName = machineName,
                    active = this.startWebsiteValue,
                    keepAlive = this.keepAliveValue,
                });
            }
            else
            {
                _machineAppRequests.Add(new IISAppSettings()
                {
                    serverName = machineName,
                    name = siteName,
                    active = this.startWebsiteValue,
                    keepAlive = this.keepAliveValue,
                });
            }
        }
        protected override void ProcessRecord()
        {
            _machineAppResults = SetMachineAppSettings();
            _machineAppResults.ToList().ForEach(WriteObject);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
        #endregion

        #region private methods
        private IEnumerable<IISAppSettings> SetMachineAppSettings()
        {
            List<IISAppSettings> appSettingsList = new List<IISAppSettings>();
            appSettingsList.AddRange(iisProcessor.UpdateApplicationSetting(_machineAppRequests));
            return appSettingsList;
        }
        #endregion
    }
}

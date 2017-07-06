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
        private bool startWebsiteValue;
        private bool stopWebsiteValue;
        private bool? active;
        private bool recycleApp;

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
        public SwitchParameter startWebsite
        {
            get { return startWebsiteValue; }
            set { startWebsiteValue = value; }
        }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("stop")]
        public SwitchParameter stopWebsite
        {
            get { return stopWebsiteValue; }
            set { stopWebsiteValue = value; }
        }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true
        )]
        [Alias("recycle")]
        public SwitchParameter recycleAppPool
        {
            get { return recycleApp; }
            set { recycleApp = value; }
        }
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
                this.keepAliveValue = null;
            else
            {
                if (this.trueValues.Contains(this.keepAlive.ToLower()))
                    this.keepAliveValue = true;
                else if (falseValues.Contains(this.keepAlive.ToLower()))
                    this.keepAliveValue = false;
            }

            if (!this.startWebsiteValue)
            {
                if (this.stopWebsiteValue)
                    this.active = false;
                else
                    this.active = null;
            }
            else
            {
                this.active = true;
            }

            _machineAppRequests = new List<IISAppSettings>();
            if (this.siteName == null || !this.siteName.Any())
            {
                _machineAppRequests.Add(new IISAppSettings()
                {
                    serverName = machineName,
                    active = this.active,
                    keepAlive = this.keepAliveValue,
                    recycle = this.recycleApp,
                });
            }
            else
            {
                _machineAppRequests.Add(new IISAppSettings()
                {
                    serverName = machineName,
                    name = siteName,
                    active = this.active,
                    keepAlive = this.keepAliveValue,
                    recycle = this.recycleApp,
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

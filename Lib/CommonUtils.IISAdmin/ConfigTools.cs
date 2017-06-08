using Microsoft.Web.Administration;
using System;
using System.Configuration;

namespace CommonUtils.IISAdmin
{
    public class ConfigTools
    {
        public string machineName { get; set; }
        public string webAppName { get; set; }
        public string path { get; set; }
        public string subPath { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/6/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ConfigTools(string machineName = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
        }
        public ConfigTools(string machineName, string appName)
        {
            this.machineName = machineName;
            this.webAppName = appName;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets current site configuration attributes. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/6/2017. </remarks>
        ///
        /// <param name="site"> The site. </param>
        ///
        /// <returns>   The current site configuration attributes. </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ConfigurationAttributeCollection GetCurrentSiteConfigAttributes(Site site)
        {
            return site.Attributes;
        }
        public static ConfigurationAttributeCollection GetDefaultSiteConfigAttributes(Site site)
        {
            ApplicationDefaults defaults = site.ApplicationDefaults;
            return defaults.Attributes;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update the configuration file using the credentials of the passed user. Tthen, call the
        /// routine that reads the configuration settings. Note that the system issues an error if the
        /// user does not have administrative privileges on the server.
        /// </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/6/2017. </remarks>
        ///
        /// <param name="machineName">  Name of the machine. </param>
        /// <param name="key">          The key. </param>
        /// <param name="value">        The value. </param>
        /// <param name="path">         (Optional) Full pathname of the file. </param>
        ///
        /// <returns>   A KeyValueConfigurationCollection. </returns>
        ///-------------------------------------------------------------------------------------------------
        public KeyValueConfigurationCollection UpdateConfiguration(string machineName, string key, string value, string path = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            if (string.IsNullOrWhiteSpace(this.machineName))
                this.machineName = Environment.MachineName;
            if (!string.IsNullOrWhiteSpace(path))
                this.path = path;
            if (string.IsNullOrWhiteSpace(this.path))
                this.path = @"/";
            if (string.IsNullOrWhiteSpace(this.webAppName))
                this.webAppName = @"Default Web Site";

            // Get the configuration.
            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                this.path, this.webAppName, this.machineName,
                this.machineName, this.userName, this.password);

            KeyValueConfigurationElement setting = config.AppSettings.Settings[key];
            if (setting == null)
                config.AppSettings.Settings.Add(key, value);
            else
                setting.Value = value;
            config.Save();

            return GetAppSettings(config);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Read the appSettings on the remote server. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/6/2017. </remarks>
        ///
        /// <param name="config">   The configuration. </param>
        ///
        /// <returns>   The application settings. </returns>
        ///-------------------------------------------------------------------------------------------------
        public KeyValueConfigurationCollection GetAppSettings(System.Configuration.Configuration config)
        {
            KeyValueConfigurationCollection keyValues = new KeyValueConfigurationCollection();
            foreach (KeyValueConfigurationElement key in config.AppSettings.Settings)
            {
                if (key.Key == "file")

                keyValues.Add(new KeyValueConfigurationElement(key.Key, key.Value));
            }
            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets application setting. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/6/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="config">   (Optional)
        ///                         The configuration. </param>
        ///
        /// <returns>   The application setting. </returns>
        ///-------------------------------------------------------------------------------------------------
        public KeyValueConfigurationElement GetAppSetting(string key, System.Configuration.Configuration config = null)
        {
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            if (string.IsNullOrWhiteSpace(this.machineName))
                this.machineName = Environment.MachineName;
            if (!string.IsNullOrWhiteSpace(path))
                this.path = path;
            if (string.IsNullOrWhiteSpace(this.path))
                this.path = @"/";
            if (string.IsNullOrWhiteSpace(this.webAppName))
                this.webAppName = @"Default Web Site";

            if (config == null)
                config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(
                        this.path, this.webAppName, this.machineName,
                        this.machineName, this.userName, this.password);
            return config.AppSettings.Settings[key];
        }
    }
}

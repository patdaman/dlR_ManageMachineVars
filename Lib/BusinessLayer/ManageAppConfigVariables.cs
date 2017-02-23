using CommonUtils.AppConfiguration;
using EFDataModel.DevOps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ViewModel.ConfigModels;
using static ViewModel.Enums;

namespace ManageConfigVariables
{
    public class ManageAppConfigVariables
    {
        public AppConfigFunctions appConfigVars { get; private set; }
        public string machineName { get; private set; }
        public string appName { get; set; }
        public string path { get; set; }
        public XDocument configFile { get; private set; }
        private string defaultPath { get; set; }

        DevOpsEntities DevOpsContext;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables()
        {
            appConfigVars = new AppConfigFunctions();
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
            if (String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(appName))
            {
                defaultPath = String.Format(@"D:\Apps\{0}\Config\AppConfig.xml", appName);
                path = defaultPath;
            }
            if (File.Exists(path))
            {
                configFile = XDocument.Load(path);
            }
            else
            {
                configFile = new XDocument();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="xDoc"> The document. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables(XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="entities"> The entities. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables(DevOpsEntities entities)
        {
            configFile = new XDocument();
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="entities"> The entities. </param>
        /// <param name="xDoc">     The document. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageAppConfigVariables(DevOpsEntities entities, XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = entities;
        }

        public ManageAppConfigVariables(string path)
        {
            this.path = path;
            configFile = XDocument.Load(path);
            appConfigVars = new AppConfigFunctions(configFile);
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets application configuration value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="attribute">    The attribute. </param>
        /// <param name="key">          The key. </param>
        ///
        /// <returns>   The application configuration value. </returns>
        ///-------------------------------------------------------------------------------------------------
        protected AttributeKeyValuePair GetAppConfigValue(string attribute, string key)
        {
            return appConfigVars.GetKeyValue(attribute, key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the application configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="attribute">    The attribute. </param>
        /// <param name="key">          The key. </param>
        ///
        /// <returns>   A ConfigModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        protected ConfigModifyResult RemoveAppConfigVariable(string attribute, string key)
        {
            var result = new ConfigModifyResult()
            {
                key = key,
                result = appConfigVars.RemoveKeyValue(attribute, key)
            };
            configFile.Save(path);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an application configuration variable to 'value'. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A ConfigModifyResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigModifyResult AddAppConfigVariable(string key, string value)
        {
            var result = new ConfigModifyResult()
            {
                key = key,
                result = appConfigVars.AddKeyValue(key, value)
            };
            configFile.Save(path);
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables from database. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListAllAppConfigVariablesFromDb(int? appId = null)
        {
            var keyValues = new List<AttributeKeyValuePair>();
            ICollection<Application> applications;

            if (appId == null)
            {
                applications = (from app in DevOpsContext.Applications
                                   where app.Machines.Contains(
                                        (from mac in DevOpsContext.Machines
                                            where mac.machine_name == machineName
                                            select mac).FirstOrDefault()
                                        )
                                   select app).ToList();
            }
            else
            {
                applications = (from app in DevOpsContext.Applications
                                where app.Machines.Contains(
                                     (from mac in DevOpsContext.Machines
                                      where mac.machine_name == machineName
                                      select mac).FirstOrDefault()
                                     )
                                where app.id == appId
                                select app).ToList();
            }
            foreach (var app in applications)
            {
                foreach (var dbKey in app.ConfigVariables)
                {
                    var keyValue = appConfigVars.GetKeyValue(dbKey.attribute, dbKey.key);
                    keyValues.Add(keyValue);
                }
            }

            return keyValues;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   List all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> ListAllAppConfigVariables()
        {
            return appConfigVars.ListConfigVariables(configFile);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> RemoveAllAppConfigVariables(int? appId = null)
        {
            var resultList = new List<ConfigModifyResult>();

            ICollection<ConfigVariable> configVars;
            ICollection<Application> applications;

            if (appId == null)
            {
                configVars = (from app in DevOpsContext.ConfigVariables
                                where app.Machines.Contains(
                                     (from mac in DevOpsContext.Machines
                                      where mac.machine_name == machineName
                                      select mac).FirstOrDefault()
                                     )
                                select app).ToList();
                foreach (var x in configVars)
                {
                    // May not be necessary since inactive keys can be removed anyways
                    // if (dbKey.active == true)
                    {
                        resultList.Add(new ConfigModifyResult() { key = x.key, result = appConfigVars.RemoveKeyValue(x.attribute, x.key, x.element) });
                    }
                }
            }
            else
            {
                applications = (from app in DevOpsContext.Applications
                                where app.Machines.Contains(
                                     (from mac in DevOpsContext.Machines
                                      where mac.machine_name == machineName
                                      select mac).FirstOrDefault()
                                     )
                                where app.id == appId
                                select app).ToList();
                foreach (var app in applications)
                {
                    foreach (var dbKey in app.ConfigVariables)
                    {
                        // May not be necessary since inactive keys can be removed anyways
                        // if (dbKey.active == true)
                        {
                            resultList.Add(new ConfigModifyResult() { key = dbKey.key, result = appConfigVars.RemoveKeyValue(dbKey.attribute, dbKey.key) });
                        }
                    }
                }
            }
            return resultList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all application configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigModifyResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> AddAllAppConfigVariables(int? appId = null)
        {
            var resultList = new List<ConfigModifyResult>();

            ICollection<ConfigVariable> configVars;
            ICollection<Application> applications;

            if (appId == null)
            {
                configVars = (from app in DevOpsContext.ConfigVariables
                              where app.Machines.Contains(
                                   (from mac in DevOpsContext.Machines
                                    where mac.machine_name == machineName
                                    select mac).FirstOrDefault()
                                   )
                              select app).ToList();
                foreach (var x in configVars)
                {
                    // May not be necessary since inactive keys can be removed anyways
                    // if (dbKey.active == true)
                    {
                        resultList.Add(new ConfigModifyResult() { key = x.key, result = appConfigVars.UpdateOrCreateAppSetting(x.attribute, x.key, x.value_name, x.value, x.element) });
                    }
                }
            }
            else
            {
                applications = (from app in DevOpsContext.Applications
                                where app.Machines.Contains(
                                     (from mac in DevOpsContext.Machines
                                      where mac.machine_name == machineName
                                      select mac).FirstOrDefault()
                                     )
                                where app.id == appId
                                select app).ToList();
                foreach (var app in applications)
                {
                    foreach (var dbKey in app.ConfigVariables)
                    {
                        // May not be necessary since inactive keys can be removed anyways
                        // if (dbKey.active == true)
                        {
                            resultList.Add(new ConfigModifyResult() { key = dbKey.key, result = appConfigVars.UpdateOrCreateAppSetting(dbKey.attribute, dbKey.key, dbKey.value_name, dbKey.value, dbKey.element) });
                        }
                    }
                }
            }
            return resultList;
        }
    }
}

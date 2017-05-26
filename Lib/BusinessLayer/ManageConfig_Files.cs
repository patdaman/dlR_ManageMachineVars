﻿using CommonUtils.AppConfiguration;
using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ViewModel;

namespace BusinessLayer
{
    public class ManageConfig_Files
    {
        public AppConfigFunctions appConfigVars { get; private set; }
        private static string ConfigFilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];

        public string userName { get; set; }
        public int? machineId { get; set; }
        public string machineName { get; set; }
        public int? appId { get; set; }
        public string appName { get; set; }
        public ViewModel.NameValuePair applications { get; set; }
        public int? componentId { get; set; }
        public string componentName { get; set; }

        public string path { get; set; }
        public string outputPath { get; set; }
        public string environment { get; set; }
        public string fileName { get; set; }
        public XDocument configFile { get; set; }
        private string defaultPath { get; set; }

        DevOpsEntities DevOpsContext;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageConfig_Files()
        {
            machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
            if (String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(componentName))
            {
                defaultPath = String.Format(@"D:\PrintableConfig\{0}\{0}.Config", componentName);
                path = defaultPath;
            }
            if (!string.IsNullOrWhiteSpace(path))
                appConfigVars = new AppConfigFunctions(path);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="xDoc"> The document. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageConfig_Files(XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/10/2017. </remarks>
        ///
        /// <param name="entities"> The entities. </param>
        ///-------------------------------------------------------------------------------------------------
        public ManageConfig_Files(DevOpsEntities entities)
        {
            configFile = new XDocument();
            appConfigVars = new AppConfigFunctions(configFile);
            DevOpsContext = new DevOpsEntities();
        }

        public ManageConfig_Files(DevOpsEntities entities, XDocument xDoc)
        {
            configFile = xDoc;
            appConfigVars = new AppConfigFunctions(configFile);
            DevOpsContext = entities;
        }

        public ManageConfig_Files(string path)
        {
            this.path = path;
            Uri uri;
            if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
            {
                configFile = XDocument.Load(uri.ToString());
            }
            else
                throw new UriFormatException("Path not a proper URI");
            appConfigVars = new AppConfigFunctions(configFile);
            DevOpsContext = new DevOpsEntities();
        }

        public ManageConfig_Files(string environment, string componentName, string outputPath, string machineName = null)
        {
            this.environment = environment;
            this.outputPath = outputPath;
            this.componentName = componentName;
            if (!string.IsNullOrWhiteSpace(machineName))
                this.machineName = machineName;
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Publish value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/26/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<AttributeKeyValuePair> PublishValue(List<AppVar> value)
        {
            throw new NotImplementedException();
        }

        public AttributeKeyValuePair PublishValue(AppVar value, string environment = null)
        {
            throw new NotImplementedException();
        }

        public List<AttributeKeyValuePair> GetPublishValues(int id)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Uploads a configuration file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/26/2017. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="configFile">   (Optional)
        ///                             The configuration file. </param>
        ///-------------------------------------------------------------------------------------------------
        public void UploadConfigFile(XDocument configFile = null)
        {
            if (configFile == null)
            {
                if (this.configFile == null)
                    throw new Exception("No Configuration File to upload.");
            }
            else
                this.configFile = configFile;
            ManageConfig_AppVariables appConfigProcessor = new ManageConfig_AppVariables(this.configFile)
            {
                environment = this.environment,
                componentName = this.componentName,
                path = this.outputPath,
                appName = this.appName,
                userName = this.userName,
            };
            List<ViewModel.AttributeKeyValuePair> configVars = appConfigProcessor.ImportAllAppConfigVariablesToDb();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Publish application files. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/25/2017. </remarks>
        ///
        /// <param name="applicationId">    Identifier for the application. </param>
        /// <param name="environment">      (Optional) The environment. </param>
        /// <param name="userName">         Name of the user. </param>
        ///
        /// <returns>   An ApplicationDto. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ApplicationDto PublishApplicationFiles(int applicationId, string environment, string userName)
        {
            EFDataModel.DevOps.Application efApp = DevOpsContext.Applications.Where(x => x.id == applicationId).FirstOrDefault();
            ViewModel.ApplicationDto appConfirm = new ApplicationDto()
            {
                id = efApp.id,
                name = efApp.application_name,
                release = efApp.release,
                published = true,
                last_modify_user = userName,
                components = new List<ComponentDto>(),
            };
            List<EFDataModel.DevOps.Component> components = efApp.Components.ToList();
            foreach (var c in components)
            {
                appConfirm.components.Add(new ComponentDto()
                {
                    componentName = c.component_name,
                    filePath = c.relative_path,
                    published = true,
                });
                string path = string.Format((ConfigFilePath + @"{0}\{1}\{2}\"), efApp.application_name, environment, c.component_name).Replace(@"\\", @"\");
                foreach (var f in c.ConfigFiles)
                {
                    string fileName = path + f.file_name;
                    try
                    {
                        SaveFile(c.id, fileName, environment);
                    }
                    catch (Exception e)
                    {
                        appConfirm.components.Where(x => x.componentName == c.component_name).FirstOrDefault().published = false;
                        appConfirm.published = false;
                    }
                }
            }
            return appConfirm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Publish component. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/26/2017. </remarks>
        ///
        /// <param name="componentName">    Name of the component. </param>
        /// <param name="environment">      (Optional) The environment. </param>
        /// <param name="userName">         Name of the user. </param>
        ///
        /// <returns>   A ComponentDto. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ComponentDto PublishComponent(string componentName, string environment, string userName)
        {
            EFDataModel.DevOps.Component efComp = DevOpsContext.Components.Where(x => x.component_name == componentName).FirstOrDefault();
            ViewModel.ComponentDto compConfirm = new ComponentDto()
            {
                id = efComp.id,
                componentName = efComp.component_name,
                published = true,
                last_modify_user = userName,
                applications = new List<ApplicationDto>(),
            };
            foreach (var a in efComp.Applications)
            {
                string applicationName = a.application_name;
                string path = string.Format((ConfigFilePath + @"{0}\{1}\{2}\"), applicationName, environment, efComp.component_name).Replace(@"\\", @"\");
                foreach (var f in efComp.ConfigFiles)
                {
                    string fileName = path + f.file_name;
                    try
                    {
                        SaveFile(efComp.id, fileName, environment);
                    }
                    catch (Exception e)
                    {
                        compConfirm.published = false;
                    }
                }
            }
            return compConfirm;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Saves a file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <param name="componentName">    Name of the component. </param>
        /// <param name="outputPath">       Full pathname of the output file. </param>
        /// <param name="environment">      (Optional) The environment. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SaveFile(string componentName, string outputPath = null, string environment = null)
        {
            var componentObject = DevOpsContext.Components.Where(x => x.component_name == componentName).FirstOrDefault();
            this.componentName = componentName;
            SaveFile(componentObject.id, outputPath, environment);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Saves a file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="componentId">  Identifier for the component. </param>
        /// <param name="outputPath">   Full pathname of the output file. </param>
        /// <param name="environment">  (Optional) The environment. </param>
        ///-------------------------------------------------------------------------------------------------
        public void SaveFile(int componentId, string outputPath = null, string environment = null)
        {
            if (outputPath == null)
            {
                if (this.outputPath == null)
                    throw new Exception("No Path set for saving file.");
            }
            else
                this.outputPath = outputPath;
            if (string.IsNullOrEmpty(environment))
                if (this.environment == null)
                    throw new Exception("No environment type provided file.");
                else
                    environment = this.environment;
            string fileName = Path.GetFileName(outputPath);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("No file name for component id: " + componentId + " provided.");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            configFile = GetConfigFile(componentId, fileName);
            configFile.Save(outputPath);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration file names. </summary>
        ///
        /// <remarks>   Pdelosreyes, 5/10/2017. </remarks>
        ///
        /// <exception cref="NullReferenceException">   Thrown when a value was unexpectedly null. </exception>
        ///
        /// <returns>   The configuration file names. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigFiles> GetConfigFileNames()
        {
            if (string.IsNullOrWhiteSpace(this.componentName))
                throw new NullReferenceException("No Component Provided");
            return GetConfigFileNames(this.componentName, this.environment);
        }

        public List<ViewModel.ConfigFiles> GetConfigFileNames(string componentName, string environment = null)
        {
            List<EFDataModel.DevOps.ConfigFile> files = new List<EFDataModel.DevOps.ConfigFile>();
            List<ViewModel.ConfigFiles> vmFiles = new List<ConfigFiles>();

            if (!string.IsNullOrWhiteSpace(componentName))
                this.componentName = componentName;
            if (string.IsNullOrWhiteSpace(this.componentName))
                throw new NullReferenceException("No Component Provided");
            if (!string.IsNullOrWhiteSpace(environment))
                this.environment = environment;

            files = DevOpsContext.Components.Where(x => x.component_name == this.componentName).FirstOrDefault()
                                     .ConfigFiles.ToList();
            foreach (var file in files)
            {
                vmFiles.Add(new ViewModel.ConfigFiles()
                {
                    fileId = file.id,
                    fileName = file.file_name,
                    path = file.Component.relative_path,
                    createDate = file.create_date,
                    modifyDate = file.modify_date,
                    last_modify_user = file.last_modify_user,
                });
            }
            return vmFiles;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration XML. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/6/2017. </remarks>
        ///
        /// <exception cref="KeyNotFoundException"> Thrown when a Key Not Found error condition occurs. </exception>
        ///
        /// <param name="componentId">  (Optional)
        ///                                                        Identifier for the component. </param>
        ///
        /// <returns>   The configuration XML. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ConfigXml GetConfigXml(int? componentId = null, string filename = null)
        {
            EFDataModel.DevOps.Component component;
            if (string.IsNullOrWhiteSpace(this.componentName))
                this.componentName = "";
            if (componentId != null)
            {
                this.componentId = componentId;
                component = DevOpsContext.Components.Where(x => x.id == this.componentId).FirstOrDefault();
            }
            else
            {
                component = DevOpsContext.Components.Where(x => x.component_name.ToLower() == this.componentName.ToLower()).FirstOrDefault();
            }
            if (component == null)
                throw new KeyNotFoundException("Component not found");
            this.componentName = component.component_name;
            this.componentId = component.id;
            if (!string.IsNullOrWhiteSpace(filename))
                this.fileName = filename;
            this.path = component.relative_path + @"\" +
            (component.ConfigFiles.FirstOrDefault().file_name) ?? "";

            XDocument xmlDoc = GetConfigFile(this.componentId, this.fileName);
            if (xmlDoc == null)
            {
                throw new ArgumentNullException(string.Format("Config File {0} for {1} not found", this.fileName ?? @"(File not Returned)", this.componentName ?? "(No Component Returned)"));
            }
            string xmlText = xmlDoc.Declaration + Environment.NewLine + xmlDoc.ToString();
            ConfigXml configXml = new ConfigXml()
            {
                title = component.component_name,
                componentId = this.componentId,
                componentName = this.componentName,
                fileName = this.fileName ?? "",
                path = this.path,
                text = xmlText,
            };
            return configXml;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/6/2017. </remarks>
        ///
        /// <exception cref="KeyNotFoundException">     Thrown when a Key Not Found error condition
        ///                                             occurs. </exception>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="componentId">  (Optional)
        ///                             Identifier for the component. </param>
        ///
        /// <returns>   The configuration file. </returns>
        ///-------------------------------------------------------------------------------------------------
        public XDocument GetConfigFile(int? componentId = null, string filename = null)
        {
            if (componentId != null)
                this.componentId = componentId;
            if (string.IsNullOrWhiteSpace(this.componentName))
            {
                this.componentName = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault().component_name;
            }
            if (string.IsNullOrWhiteSpace(this.componentName))
                throw new KeyNotFoundException("Component not found.");

            if (string.IsNullOrWhiteSpace(environment))
            {
                if (string.IsNullOrWhiteSpace(this.environment))
                    throw new ArgumentNullException(string.Format("No environment selected for {0} config file.", this.componentName));
                environment = this.environment;
            }

            ManageConfig_AppVariables variableProcessor = new ManageConfig_AppVariables(DevOpsContext)
            {
                environment = environment ?? string.Empty
            };
            List<AttributeKeyValuePair> elements = variableProcessor.ListAllAppConfigVariablesFromDb(componentId, environment, filename);
            AttributeKeyValuePair efRootElement = elements.Where(x => string.IsNullOrWhiteSpace(x.parentElement)).FirstOrDefault();

            if (efRootElement != null)
            {
                configFile = new XDocument(XElement.Parse(efRootElement.fullElement))
                {
                    Declaration = new XDeclaration("1.0", "utf-8", "no")
                };
            }
            else
            {
                configFile = new XDocument()
                {
                    Declaration = new XDeclaration("1.0", "utf-8", "no")
                };
            }


            //elements.RemoveAll(x => string.IsNullOrWhiteSpace(x.parentElement));
            elements.Remove(efRootElement);
            AppConfigFunctions configProcessor = new AppConfigFunctions(configFile);
            var results = configProcessor.AddKeyValue(elements);
            return configFile;
        }
    }
}

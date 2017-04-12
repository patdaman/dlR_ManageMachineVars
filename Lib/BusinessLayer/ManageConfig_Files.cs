using CommonUtils.AppConfiguration;
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

        public int? machineId { get; set; }
        public string machineName { get; set; }
        public int? appId { get; set; }
        public string appName { get; set; }
        public int? componentId { get; set; }
        public string componentName { get; set; }

        public string path { get; set; }
        public string outputPath { get; set; }
        public string environment { get; set; }
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

        public List<AttributeKeyValuePair> PublishValue(List<AppVar> value)
        {
            throw new NotImplementedException();
        }

        public List<AttributeKeyValuePair> GetPublishValues()
        {
            throw new NotImplementedException();
        }

        public List<AttributeKeyValuePair> PublishValue()
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
                componentName = this.componentName
            };
            List<ViewModel.AttributeKeyValuePair> configVars = appConfigProcessor.ImportAllAppConfigVariablesToDb();
        }

        public void PublishFile(XDocument configFile)
        {
            this.configFile = configFile;
            if (this.componentId == null)
            {
                if (string.IsNullOrWhiteSpace(this.componentName))
                    throw new Exception("No Component Provided.");
                else
                    PublishFile(this.componentName, this.environment);
            }
            else
            {
                PublishFile(this.componentId.Value, this.environment);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Publish file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <param name="componentName">    Name of the component. </param>
        /// <param name="environment">      (Optional) The environment. </param>
        ///-------------------------------------------------------------------------------------------------
        public void PublishFile(string componentName, string environment = null)
        {
            var componentObject = DevOpsContext.Components.Where(x => x.component_name == componentName).FirstOrDefault();
            PublishFile(componentObject.id, environment);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Publish file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <param name="componentId">  Identifier for the component. </param>
        /// <param name="environment">  (Optional) The environment. </param>
        ///
        /// <returns>   A List&lt;AttributeKeyValuePair&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public void PublishFile(int componentId, string environment)
        {
            var componentObject = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault();
            SaveFile(componentId, this.outputPath, environment);
            foreach (var c in componentObject.ConfigVariables.ToList())
            {
                foreach (var cv in c.ConfigVariableValues.Where(x => x.environment_type == environment))
                {
                    cv.published = true;
                    cv.published_date = DateTime.Now;
                }
            }
            DevOpsContext.SaveChanges();
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
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
            configFile = GetConfigFile(componentId);
#if DEBUG
            outputPath = string.Format(@"D:\DevOps\Config\{0}.config", this.componentName);
#endif
            configFile.Save(outputPath);
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
        public ConfigXml GetConfigXml(int? componentId = null)
        {
            EFDataModel.DevOps.Component component;
            if (string.IsNullOrWhiteSpace(this.componentName))
                this.componentName = "";
            component = DevOpsContext.Components.Where(x => x.component_name.ToLower() == this.componentName.ToLower()).FirstOrDefault();
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
            this.path = component.relative_path + @"\" + 
                (component.ConfigFiles.FirstOrDefault().file_name + @".config") ?? "";

            XDocument xmlDoc = GetConfigFile(this.componentId);
            if (xmlDoc == null)
            {
                throw new ArgumentNullException(string.Format("Config File for {0} not found", this.componentName));
            }
            string xmlText = xmlDoc.Declaration + Environment.NewLine + xmlDoc.ToString();
            ConfigXml configXml = new ConfigXml()
            {
                title = component.component_name,
                componentId = this.componentId,
                componentName = this.componentName,
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
        public XDocument GetConfigFile(int? componentId = null)
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
            List<AttributeKeyValuePair> elements = variableProcessor.ListAllAppConfigVariablesFromDb(componentId, environment);
            AttributeKeyValuePair efRootElement = elements.Where(x => string.IsNullOrWhiteSpace(x.parentElement)).FirstOrDefault();

            configFile = new XDocument(new XElement(efRootElement.element, string.Empty))
            {
                Declaration = new XDeclaration("1.0", "utf-8", "yes")
            };

            elements.RemoveAll(x => string.IsNullOrWhiteSpace(x.parentElement));
            //elements.Remove(efRootElement);
            AppConfigFunctions configProcessor = new AppConfigFunctions(configFile);
            var results = configProcessor.AddKeyValue(elements);
            return configFile;
        }
    }
}

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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Import configuration file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/31/2017. </remarks>
        ///
        /// <exception cref="FileNotFoundException">    Thrown when the requested file is not present. </exception>
        /// <exception cref="UriFormatException">       Thrown when an URI Format error condition occurs. </exception>
        ///
        /// <param name="filePath"> (Optional) Full pathname of the file. </param>
        ///-------------------------------------------------------------------------------------------------
        public void ImportConfigFile(string filePath = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                if (string.IsNullOrWhiteSpace(this.path) && configFile.FirstNode == null)
                    throw new FileNotFoundException("No file path provided.");
                filePath = this.path;
            }
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                Uri uri;
                if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out uri))
                {
                    configFile = XDocument.Load(uri.ToString());
                }
                else
                    throw new UriFormatException("Path not a proper URI");
            }

            if (string.IsNullOrWhiteSpace(componentName))
            {
                if (string.IsNullOrWhiteSpace(filePath))
                    componentName = configFile.Root.Name.ToString().Replace("config", "");
                else
                    componentName = Path.GetFileNameWithoutExtension(filePath);
            }

            EFDataModel.DevOps.Component newComp = new EFDataModel.DevOps.Component();
            EFDataModel.DevOps.Component efComp = new EFDataModel.DevOps.Component();
            IQueryable<EFDataModel.DevOps.Component> CompQuery;
            if (componentId != null || !string.IsNullOrWhiteSpace(componentName))
            {
                if (componentId == null)
                    CompQuery = (from Comp in DevOpsContext.Components
                                 where Comp.component_name.ToLower() == componentName.ToLower()
                                 select Comp);
                else
                    CompQuery = (from Comp in DevOpsContext.Components
                                 where Comp.id == componentId
                                 select Comp);
                efComp = CompQuery.FirstOrDefault();
                componentId = efComp.id;
            }
            else
            {
                newComp = new EFDataModel.DevOps.Component()
                {
                    component_name = this.componentName,
                    relative_path = Path.GetDirectoryName(filePath).Substring(Path.GetPathRoot(filePath).Length),
                };
            }


            EFDataModel.DevOps.Machine efMac;
            IQueryable<EFDataModel.DevOps.Machine> macQuery;
            if (machineId != null || !string.IsNullOrWhiteSpace(this.machineName))
            {
                if (machineId == null)
                    macQuery = (from Mac in DevOpsContext.Machines
                                where Mac.machine_name.ToLower() == machineName.ToLower()
                                select Mac);
                else
                    macQuery = (from Mac in DevOpsContext.Machines
                                where Mac.id == machineId
                                select Mac);
                efMac = macQuery.FirstOrDefault();
                machineId = efMac.id;
                EFDataModel.DevOps.MachineComponentPathMap machinePath;
                if (efComp == null)
                {
                    machinePath = new EFDataModel.DevOps.MachineComponentPathMap()
                    {
                        machine_id = this.machineId.Value,
                        config_path = Path.GetDirectoryName(filePath)
                    };
                    newComp.MachineComponentPathMaps.Add(machinePath);
                }
                else
                {
                    machinePath = efComp.MachineComponentPathMaps.Where(x => x.machine_id == this.machineId).FirstOrDefault();
                    if (machinePath == null)
                    {
                        machinePath = new EFDataModel.DevOps.MachineComponentPathMap()
                        {
                            machine_id = this.machineId.Value,
                            component_id = efComp.id,
                            config_path = Path.GetDirectoryName(filePath)
                        };
                        efComp.MachineComponentPathMaps.Add(machinePath);
                    }
                    else if (machinePath.config_path != filePath)
                        machinePath.config_path = filePath;
                }
            }

            var xmlDefinition = configFile.Declaration.ToString();
            XElement rootElement = configFile.Root;

            /// ------------------------------------------------------------------------------------- ///
            // Todo:
            //  Make import account for all elements in file
            //      - Include elements with no attributes or values
            //      - Include elements with multiple attributes and / or values
            //      - Use xml declaration and root for publishing
            //      - Be able to import / export web.config or other complex xml
            /// ------------------------------------------------------------------------------------- ///


            //List<AttributeKeyValuePair> configVars = appConfigVars.ListConfigVariables(configFile);
            //foreach (var x in configVars)
            //{
            //    EFDataModel.DevOps.ConfigVariable configVar;
            //    EFDataModel.DevOps.ConfigVariableValue efValue;
            //    configVar = (from n in DevOpsContext.ConfigVariables
            //                 where n.element == x.element
            //                 where n.key == x.key
            //                 where n.key_name == x.keyName
            //                 where n.parent_element == x.parentElement
            //                 where n.value_name == x.valueName
            //                 select n).FirstOrDefault();
            //    if (configVar == null)
            //    {
            //        //configVar = CreateAppConfigEntity(x);
            //        efComp.ConfigVariables.Add(configVar);
            //    }
            //    else
            //    {
            //        efValue = configVar.ConfigVariableValues.Where(c => c.environment_type == environment).FirstOrDefault();
            //        if (efValue == null)
            //            configVar.ConfigVariableValues.Add(
            //                new EFDataModel.DevOps.ConfigVariableValue()
            //                {
            //                    environment_type = environment.ToLower(),
            //                    configvar_id = configVar.id,
            //                    value = x.value,
            //                    create_date = DateTime.Now,
            //                    modify_date = DateTime.Now
            //                });
            //        else
            //        {
            //            efValue.value = x.value;
            //            efValue.modify_date = DateTime.Now;
            //        }
            //    }
            //}
            //DevOpsContext.SaveChanges();
        }

        public List<AttributeKeyValuePair> PublishValue(List<AppVar> value)
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

        public List<AttributeKeyValuePair> GetPublishValues()
        {
            throw new NotImplementedException();
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
        public void PublishFile(int componentId, string environment = null)
        {
            var componentObject = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault();

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
        public void SaveFile(string componentName, string outputPath, string environment = null)
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
        public void SaveFile(int componentId, string outputPath, string environment = null)
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
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

            elements.Remove(efRootElement);

            configFile = new XDocument(new XElement(efRootElement.element, string.Empty))
            {
                Declaration = new XDeclaration("1.0", "utf-8", "yes")
            };

            AppConfigFunctions configProcessor = new AppConfigFunctions(configFile);
            var results = configProcessor.AddKeyValue(elements);
#if DEBUG
            outputPath = string.Format(@"D:\DevOps\{0}.config", this.componentName);
#endif
            configFile.Save(outputPath);
        }
    }
}

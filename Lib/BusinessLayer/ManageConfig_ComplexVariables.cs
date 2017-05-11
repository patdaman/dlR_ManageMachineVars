using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDataModel.DevOps;
using ViewModel;

namespace BusinessLayer
{
    public class ManageConfig_ComplexVariables
    {
        DevOpsEntities DevOpsContext;
        ConvertObjects_Reflection EfToVmConverter = new ConvertObjects_Reflection();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageConfig_ComplexVariables()
        {
            DevOpsContext = new DevOpsEntities();
            // EfToVmConverter = new ConvertObjects();
        }

        public ManageConfig_ComplexVariables(DevOpsEntities entities)
        {
            DevOpsContext = entities;
        }

        public ManageConfig_ComplexVariables(string conn)
        {
            DevOpsContext = new DevOpsEntities(conn);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <returns>   all variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.MachineAppVars> GetAllVariables()
        {
            List<ViewModel.MachineAppVars> allVars = new List<ViewModel.MachineAppVars>();
            List<ViewModel.ConfigVariable> configVars = new List<ViewModel.ConfigVariable>();
            List<ViewModel.EnvironmentDtoVariable> enVars = new List<ViewModel.EnvironmentDtoVariable>();

            configVars = GetAllAppConfigVariables();
            //enVars = GetAllEnVariables();

            return allVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <returns>   The application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Application> GetApplication()
        {
            List<ViewModel.Application> applications = new List<ViewModel.Application>();
            var efApplications = DevOpsContext.Applications.ToList();
            foreach (var c in efApplications)
            {
                applications.Add(ReturnApplicationVariable(c));
            }
            return applications;
        }

        public ViewModel.Application GetApplication(int id)
        {
            var efApplication = DevOpsContext.Applications.Where(x => x.id == id).FirstOrDefault();
            return new ViewModel.Application()
            {
                active = efApplication.active,
                application_name = efApplication.application_name,
                create_date = efApplication.create_date,
                modify_date = efApplication.modify_date,
                id = efApplication.id,
                release = efApplication.release,
            };
        }

        public ViewModel.Application GetApplication(string applicationName)
        {
            var efApplication = DevOpsContext.Applications.Where(x => x.application_name == applicationName).FirstOrDefault();
            return new ViewModel.Application()
            {
                active = efApplication.active,
                application_name = efApplication.application_name,
                create_date = efApplication.create_date,
                modify_date = efApplication.modify_date,
                id = efApplication.id,
                release = efApplication.release,
                Components = EfToVmConverter.EfComponentListToVm(efApplication.Components).ToList(),
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the component. </summary>
        ///
        /// <remarks>   Patman, 4/14/2017. </remarks>
        ///
        /// <returns>   The component. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<ViewModel.Component> GetComponent()
        {
            List<ViewModel.Component> components = new List<ViewModel.Component>();
            var efComponents = DevOpsContext.Components.ToList();
            foreach (var c in efComponents)
            {
                components.Add(ReturnComponentVariable(c));
            }
            return components;
        }

        public ViewModel.Component GetComponent(int componentId, bool noVal = false)
        {
            var efComponent = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault();
            return ReturnComponentVariable(efComponent, noVal);
        }

        public ViewModel.Component GetComponent(string componentName, bool noVal = false)
        {
            var efComponent = DevOpsContext.Components.Where(x => x.component_name == componentName).FirstOrDefault();
            return ReturnComponentVariable(efComponent, noVal);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an update component. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <param name="component">    The component. </param>
        ///
        /// <returns>   A ViewModel.Component. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Component AddUpdateComponent(ViewModel.Component component)
        {
            EFDataModel.DevOps.Component efComp;
            if (component.id != null && component.id != 0)
                efComp = DevOpsContext.Components.Where(x => x.id == component.id).FirstOrDefault();
            else
                efComp = DevOpsContext.Components.Where(x => x.component_name == component.component_name).FirstOrDefault();
            if (efComp == null)
                DevOpsContext.Components.Add(new EFDataModel.DevOps.Component()
                {
                    component_name = component.component_name,
                    create_date = component.create_date,
                    modify_date = component.modify_date ?? DateTime.Now,
                    MachineComponentPathMaps = new List<MachineComponentPathMap>(),
                    active = component.active,
                    relative_path = component.relative_path,
                    ConfigFiles = new List<EFDataModel.DevOps.ConfigFile>(),
                    ConfigVariables = new List<EFDataModel.DevOps.ConfigVariable>(),
                    Applications = GetEfApplication(component.Applications),
                });
            else
            {
                efComp.component_name = component.component_name;
                efComp.create_date = component.create_date;
                efComp.modify_date = component.modify_date ?? DateTime.Now;
                efComp.active = component.active;
                efComp.relative_path = component.relative_path;
                var efApps = GetEfApplication(component.Applications);
                var oldEfApps = efComp.Applications.Where(x => !efApps.Any(y => y.id == x.id)).ToList();
                foreach (var oldApp in oldEfApps)
                {
                    efComp.Applications.Remove(oldApp);
                }
                var newEfApps = efApps.Where(x => !efComp.Applications.Any(y => y.id == x.id)).ToList();
                foreach (var newApp in newEfApps)
                {
                    efComp.Applications.Add(newApp);
                }
            }
            DevOpsContext.SaveChanges();
            return GetComponent(component.component_name, true);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an update application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <param name="application">  The application. </param>
        ///
        /// <returns>   A ViewModel.Application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Application AddUpdateApplication(ViewModel.Application application)
        {
            EFDataModel.DevOps.Application efApp;
            if (application.id != null && application.id != 0)
                efApp = DevOpsContext.Applications.Where(x => x.id == application.id).FirstOrDefault();
            else
                efApp = DevOpsContext.Applications.Where(x => x.application_name == application.application_name).FirstOrDefault();
            if (efApp == null)
                DevOpsContext.Applications.Add(new EFDataModel.DevOps.Application()
                {
                    application_name = application.application_name,
                    create_date = application.create_date,
                    modify_date = application.modify_date ?? DateTime.Now,
                    active = application.active,
                    release = application.release,
                    Components = GetEfComponent(application.Components),
                });
            else
            {
                efApp.application_name = application.application_name;
                efApp.create_date = application.create_date;
                efApp.modify_date = application.modify_date ?? DateTime.Now;
                efApp.active = application.active;
                efApp.release = application.release;
                var efComps = GetEfComponent(application.Components);
                var oldEfComps = efApp.Components.Where(x => !efComps.Any(y => y.id == x.id)).ToList();
                foreach (var oldComp in oldEfComps)
                {
                    efApp.Components.Remove(oldComp);
                }
                var newEfComps = efComps.Where(x => !efApp.Components.Any(y => y.id == x.id)).ToList();
                foreach (var newComp in newEfComps)
                {
                    efApp.Components.Add(newComp);
                }
            }
            DevOpsContext.SaveChanges();
            return GetApplication(application.application_name);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets drop down values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/12/2017. </remarks>
        ///
        /// <param name="type"> The type. </param>
        ///
        /// <returns>   The drop down values. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<NameValuePair> GetDropDownValues(string type)
        {
            List<NameValuePair> values = new List<NameValuePair>();
            if (type.ToLower().StartsWith("comp"))
            {
                var components = DevOpsContext.Components.ToList();
                foreach (var c in components)
                    values.Add(new NameValuePair()
                    {
                        id = c.id,
                        name = c.component_name,
                        value = c.component_name,
                        active = c.active
                    });
            }
            else if (type.ToLower().StartsWith("env"))
            {
                var environments = DevOpsContext.Enum_EnvironmentType.ToList();
                foreach (var e in environments)
                    values.Add(new NameValuePair()
                    {
                        id = null,
                        name = e.name,
                        value = e.name,
                        active = e.active
                    });
            }
            else if (type.ToLower().StartsWith("app"))
            {
                var apps = DevOpsContext.Applications.ToList();
                foreach (var a in apps)
                    values.Add(new NameValuePair()
                    {
                        id = a.id,
                        name = a.application_name,
                        value = a.application_name,
                        active = a.active
                    });
            }
            else if (type.ToLower().StartsWith("machine"))
            {
                var machines = DevOpsContext.Machines.ToList();
                foreach (var m in machines)
                {
                    values.Add(new NameValuePair()
                    {
                        id = m.id,
                        name = m.machine_name,
                        value = m.machine_name,
                        active = m.active,
                    });
                }
            }
            else
                throw new ArgumentException("Invalid value type requested.");
            return values;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets an application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/20/2017. </remarks>
        ///
        /// <param name="apps"> The apps. </param>
        ///
        /// <returns>   The application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Application> GetApplication(List<ApplicationDto> apps)
        {
            List<ViewModel.Application> vmApps = new List<ViewModel.Application>();
            if (apps.Count > 0)
            {
                //List<EFDataModel.DevOps.Application> efApps = DevOpsContext.Applications.Where(x => apps.Select(y => y.name).FirstOrDefault().Contains(x.application_name)).ToList();
                List<string> appList = new List<string>();

                foreach (var vmApp in apps)
                {
                    var app = DevOpsContext.Applications.Where(x => x.application_name.Contains(vmApp.name)).FirstOrDefault();
                    //var app = efApps.Where(x => x.application_name == vmApp.name).FirstOrDefault();
                    if (app == null)
                    {
                        vmApps.Add(new ViewModel.Application(vmApp)
                        {
                            active = true,
                            create_date = DateTime.Now,
                            modify_date = DateTime.Now,
                            release = vmApp.release ?? ".42",
                            EnvironmentVariables = new List<EnvironmentDtoVariable>(),
                        });
                    }
                    else
                    {
                        vmApps.Add(new ViewModel.Application()
                        {
                            id = app.id,
                            active = app.active,
                            application_name = app.application_name,
                            create_date = app.create_date,
                            modify_date = app.modify_date,
                            release = app.release,
                            EnvironmentVariables = new List<EnvironmentDtoVariable>(),
                        });
                    }
                }
            }
            return vmApps;
        }

        public List<ViewModel.Component> GetComponent(ApplicationDto applicationModel)
        {
            var comps = new List<ViewModel.Component>();
            // todo:
            //  Make this work!
            return comps;
        }

        public ViewModel.Application GetApplication(ApplicationDto appDto)
        {
            var app = DevOpsContext.Applications.Where(x => x.application_name.Contains(appDto.name)).FirstOrDefault();
            if (app == null)
            {
                return new ViewModel.Application(appDto)
                {
                    active = true,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    release = ".42",
                    EnvironmentVariables = new List<EnvironmentDtoVariable>(),
                };
            }
            else
            {
                return new ViewModel.Application()
                {
                    id = app.id,
                    active = app.active,
                    application_name = app.application_name,
                    create_date = app.create_date,
                    modify_date = app.modify_date,
                    release = app.release,
                    EnvironmentVariables = new List<EnvironmentDtoVariable>(),
                };
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets ef application. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/20/2017. </remarks>
        ///
        /// <param name="vmApps">   The view model apps. </param>
        ///
        /// <returns>   The ef application. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EFDataModel.DevOps.Application> GetEfApplication(List<ViewModel.Application> vmApps)
        {
            List<EFDataModel.DevOps.Application> efApps = new List<EFDataModel.DevOps.Application>();
            foreach (var vmApp in vmApps)
            {
                efApps.Add(GetEfApplication(vmApp));
            }
            return efApps;
        }

        public EFDataModel.DevOps.Application GetEfApplication(ViewModel.Application vmApp)
        {
            EFDataModel.DevOps.Application app;
            app = DevOpsContext.Applications.Where(x => x.application_name == vmApp.application_name).FirstOrDefault();
            if (app == null)
            {
                if (string.IsNullOrWhiteSpace(vmApp.release))
                    vmApp.release = ".42";
                app = new EFDataModel.DevOps.Application()
                {
                    application_name = vmApp.application_name,
                    active = vmApp.active, // ?? true,
                    create_date = vmApp.create_date, // ?? DateTime.Now,
                    modify_date = vmApp.modify_date ?? DateTime.Now,
                    release = vmApp.release,
                    EnvironmentVariables = new List<EFDataModel.DevOps.EnvironmentVariable>(),
                };
            }
            else
            {
                app.id = vmApp.id;
                app.active = vmApp.active;
                app.application_name = vmApp.application_name;
                app.create_date = vmApp.create_date;
                app.modify_date = vmApp.modify_date ?? DateTime.Now;
                app.release = vmApp.release;
            }
            return app;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a component. </summary>
        ///
        /// <remarks>   Pdelosreyes, 4/27/2017. </remarks>
        ///
        /// <param name="comps">    The comps. </param>
        ///
        /// <returns>   The component. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.Component> GetComponentsFromCsv(string comps)
        {
            List<string> compList = new List<string>();
            List<ViewModel.Component> vmCompList = new List<ViewModel.Component>();
            if (!string.IsNullOrWhiteSpace(comps))
            {
                compList = comps.Split(',').ToList();
            }
            foreach (string comp in compList)
            {
                vmCompList.Add(GetComponent(comp));
            }
            return vmCompList;
        }

        public ViewModel.Component GetComponent(string compName)
        {
            var comp = DevOpsContext.Components.Where(x => x.component_name.Contains(compName)).FirstOrDefault();
            if (comp == null)
            {
                return new ViewModel.Component()
                {
                    component_name = compName,
                    active = true,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    relative_path = "",
                };
            }
            else
            {
                return new ViewModel.Component()
                {
                    id = comp.id,
                    active = comp.active,
                    component_name = comp.component_name,
                    create_date = comp.create_date,
                    modify_date = comp.modify_date,
                    relative_path = comp.relative_path,
                };
            }
        }

        public List<ViewModel.Component> GetComponent(List<ComponentDto> comps)
        {
            List<ViewModel.Component> vmComps = new List<ViewModel.Component>();
            if (comps.Count > 0)
            {
                foreach (var vmComp in comps)
                {
                    vmComps.Add(GetComponent(vmComp.componentName));
                }
            }
            return vmComps;
        }

        public ViewModel.Component GetComponent(ComponentDto comp)
        {
            var component = DevOpsContext.Components.Where(x => x.component_name.Contains(comp.componentName)).FirstOrDefault();
            if (component == null)
            {
                return new ViewModel.Component(comp)
                {
                    active = true,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                    relative_path = "",
                };
            }
            else
            {
                return new ViewModel.Component(comp)
                {
                    id = component.id,
                    active = component.active,
                    component_name = component.component_name,
                    create_date = component.create_date,
                    modify_date = component.modify_date,
                    relative_path = component.relative_path,
                };
            }
        }

        public List<EFDataModel.DevOps.Component> GetEfComponent(List<ViewModel.Component> vmComps)
        {
            List<EFDataModel.DevOps.Component> efComps = new List<EFDataModel.DevOps.Component>();
            foreach (var vmComp in vmComps)
            {
                efComps.Add(GetEfComponent(vmComp));
            }
            return efComps;
        }

        public EFDataModel.DevOps.Component GetEfComponent(ViewModel.Component vmComp)
        {
            EFDataModel.DevOps.Component comp;
            comp = DevOpsContext.Components.Where(x => x.component_name == vmComp.component_name).FirstOrDefault();
            if (comp == null)
            {
                comp = new EFDataModel.DevOps.Component()
                {
                    component_name = vmComp.component_name,
                    active = vmComp.active, // ?? true,
                    create_date = vmComp.create_date, // ?? DateTime.Now,
                    modify_date = vmComp.modify_date ?? DateTime.Now,
                    relative_path = vmComp.relative_path,
                };
            }
            else
            {
                comp.id = vmComp.id;
                comp.active = vmComp.active;
                comp.component_name = vmComp.component_name;
                comp.create_date = vmComp.create_date;
                comp.modify_date = vmComp.modify_date ?? DateTime.Now;
                comp.relative_path = vmComp.relative_path;
            }
            return comp;
        }
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all machine configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/17/2017. </remarks>
        ///
        /// <returns>   all machine configuration variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.AppVar> GetAllConfigVariables()
        {
            List<ViewModel.AppVar> allVars = new List<ViewModel.AppVar>();
            var allConfigVars = DevOpsContext.ConfigVariables.ToList();
            foreach (var appVar in allConfigVars)
            {
                var varComponents = appVar.Components.ToList();

                foreach (var vc in varComponents)
                {
                    ViewModel.ConfigVariable configVar = ReturnConfigVariable(appVar);
                    //if (configVar.attribute != configVar.key && !string.IsNullOrWhiteSpace(configVar.value_name))
                    if (!string.IsNullOrWhiteSpace(configVar.parent_element))
                    {
                        AppVar appVarModel = new AppVar(configVar);
                        appVarModel.componentId = vc.id;
                        appVarModel.componentName = vc.component_name ?? string.Empty;

                        foreach (var app in vc.Applications)
                        {
                            if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                                appVarModel.applicationNames = app.application_name;
                            else
                                //appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                                appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", app.application_name);
                        }
                        appVarModel.values.AddRange(configVar.ConfigVariableValues);
                        allVars.Add(appVarModel);
                    }
                }
            }
            return allVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all configuration variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/28/2017. </remarks>
        ///
        /// <returns>   all configuration variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigVariable> GetAllAppConfigVariables()
        {
            var configVars = new List<ViewModel.ConfigVariable>();
            var allConfigVars = (from vars in DevOpsContext.ConfigVariables
                                 select vars).ToList();

            foreach (var config in allConfigVars)
            {
                configVars.Add(ReturnConfigVariable(config));
            }
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <returns>   The configuration values. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigVariableValue> GetConfigValues()
        {
            List<ViewModel.ConfigVariableValue> configVars = new List<ViewModel.ConfigVariableValue>();
            List<EFDataModel.DevOps.ConfigVariableValue> efConfigVals = DevOpsContext.ConfigVariableValues.ToList();
            foreach (EFDataModel.DevOps.ConfigVariableValue efVal in efConfigVals)
            {
                configVars.Add(ReturnConfigValue(efVal));
            }
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="configvar_id"> Identifier for the configvar. </param>
        ///
        /// <returns>   The configuration values. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.ConfigVariableValue> GetConfigValues(int configvar_id)
        {
            List<ViewModel.ConfigVariableValue> configVars = new List<ViewModel.ConfigVariableValue>();
            List<EFDataModel.DevOps.ConfigVariableValue> efConfigVals = DevOpsContext.ConfigVariableValues.Where(x => x.configvar_id == configvar_id).ToList();
            foreach (EFDataModel.DevOps.ConfigVariableValue efVal in efConfigVals)
            {
                configVars.Add(ReturnConfigValue(efVal));
            }
            return configVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="varId">    Identifier for the variable. </param>
        /// <param name="envType">  Type of the environment. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar GetVariable(int varId, string envType = null)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the value described by value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/28/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A ViewModel.ConfigVariableValue. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.ConfigVariableValue UpdateValue(ViewModel.ConfigVariableValue value)
        {
            EFDataModel.DevOps.ConfigVariableValue efValue = DevOpsContext.ConfigVariableValues.Where(x => x.configvar_id == value.configvar_id && x.environment_type == value.environment).FirstOrDefault();
            if (efValue == null)
            {
                EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(y => y.id == value.configvar_id).FirstOrDefault();
                if (efConfigVar == null)
                    throw new KeyNotFoundException("Config Variable not found");
                efValue = new EFDataModel.DevOps.ConfigVariableValue()
                {
                    configvar_id = value.configvar_id,
                    environment_type = value.environment,
                    value = value.value,
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                };
                efConfigVar.ConfigVariableValues.Add(efValue);
            }
            else
            {
                if (efValue.value != value.value)
                {
                    efValue.value = value.value;
                    efValue.modify_date = DateTime.Now;
                }
            }
            DevOpsContext.SaveChanges();
            return GetConfigValues(value.configvar_id, value.environment);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the variable described by appValue. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="appValue"> The application value. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar UpdateVariable(AppVar appValue)
        {
            return AddUpdateVariable(appValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar AddVariable(AppVar appValue)
        {
            return AddUpdateVariable(appValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the variable described by ID. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="id">   The identifier. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        public AppVar DeleteVariable(int id)
        {
            throw new NotImplementedException();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Deletes the component described by componentId. </summary>
        ///
        /// <remarks>   Patman, 4/14/2017. </remarks>
        ///
        /// <param name="componentId">  Identifier for the component. </param>
        ///
        /// <returns>   A ViewModel.Component. </returns>
        ///-------------------------------------------------------------------------------------------------
        public ViewModel.Component DeleteComponent(int componentId)
        {
            var comp = DevOpsContext.Components.Where(x => x.id == componentId).FirstOrDefault();
            DevOpsContext.Components.Remove(comp);
            DevOpsContext.SaveChanges();
            return new ViewModel.Component();
        }

        ///-------------------------------------------------------------------------------------------------
        //  <section> Begin Private Methods </section>
        ///-------------------------------------------------------------------------------------------------


        private ViewModel.Component ReturnComponentVariable(EFDataModel.DevOps.Component component, bool noVal = false)
        {
            if (noVal)
            {
                return new ViewModel.Component()
                {
                    id = component.id,
                    active = component.active,
                    component_name = component.component_name,
                    create_date = component.create_date,
                    relative_path = component.relative_path,
                    modify_date = component.modify_date,
                    MachineComponentPaths = EfToVmConverter.EfMachineComponentPathListToVm(component.MachineComponentPathMaps),
                    Applications = EfToVmConverter.EfAppListToVm(component.Applications),
                    ConfigVariables = new List<ViewModel.ConfigVariable>(),
                };
            }
            else
            {
                return new ViewModel.Component()
                {
                    id = component.id,
                    active = component.active,
                    component_name = component.component_name,
                    create_date = component.create_date,
                    relative_path = component.relative_path,
                    modify_date = component.modify_date,
                    MachineComponentPaths = EfToVmConverter.EfMachineComponentPathListToVm(component.MachineComponentPathMaps),
                    Applications = EfToVmConverter.EfAppListToVm(component.Applications),
                    ConfigVariables = EfToVmConverter.EfConfigListToVm(component.ConfigVariables).ToList(),
                };
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets configuration values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/28/2017. </remarks>
        ///
        /// <exception cref="KeyNotFoundException"> Thrown when a Key Not Found error condition occurs. </exception>
        ///
        /// <param name="configvar_id"> Identifier for the configvar. </param>
        /// <param name="environment">  The environment. </param>
        ///
        /// <returns>   The configuration values. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariableValue GetConfigValues(int configvar_id, string environment)
        {
            EFDataModel.DevOps.ConfigVariableValue efValue = DevOpsContext.ConfigVariableValues.Where(x => x.configvar_id == configvar_id && x.environment_type == environment).FirstOrDefault();
            if (efValue == null)
            {
                EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(y => y.id == configvar_id).FirstOrDefault();
                if (efConfigVar == null)
                    throw new KeyNotFoundException("Configuration Variable not found, please add / save new variable");
                throw new KeyNotFoundException(string.Format("Config Variable {0} does not contain a value for {1} environment."));
            }
            return new ViewModel.ConfigVariableValue()
            {
                id = efValue.id,
                configvar_id = efValue.configvar_id,
                environment = efValue.environment_type,
                create_date = efValue.create_date,
                modify_date = efValue.modify_date,
                publish_date = efValue.published_date,
                published = efValue.published
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns configuration variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/17/2017. </remarks>
        ///
        /// <param name="config">   The configuration. </param>
        ///
        /// <returns>   The configuration variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariable ReturnConfigVariable(EFDataModel.DevOps.ConfigVariable config)
        {
            var environments = DevOpsContext.Enum_EnvironmentType.ToList();
            var file = config.ConfigFile;
            if (file == null)
                file = new EFDataModel.DevOps.ConfigFile();
            var configVars = config.ConfigVariableValues;
            if (configVars == null)
                configVars = new List<EFDataModel.DevOps.ConfigVariableValue>();
            return new ViewModel.ConfigVariable()
            {
                id = config.id,
                active = config.active,
                attribute = config.attribute,
                create_date = config.create_date,
                element = config.element,
                key = config.key,
                modify_date = config.modify_date,
                value_name = config.value_name ?? "",
                parent_element = config.parent_element,
                ConfigFile = EfToVmConverter.EfConfigFileToVm(file),
                ConfigVariableValues = EfToVmConverter.EfConfigValueListToVm(configVars, environments),
                Components = EfToVmConverter.EfComponentListToVm(config.Components)
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns configuration value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="configVal">    The configuration value. </param>
        ///
        /// <returns>   The configuration value. </returns>
        ///-------------------------------------------------------------------------------------------------
        private ViewModel.ConfigVariableValue ReturnConfigValue(EFDataModel.DevOps.ConfigVariableValue configVal)
        {
            return new ViewModel.ConfigVariableValue()
            {
                configvar_id = configVal.configvar_id,
                id = configVal.id,
                environment = configVal.environment_type,
                value = configVal.value,
                create_date = configVal.create_date,
                modify_date = configVal.modify_date,
                publish_date = configVal.published_date,
                published = configVal.published
            };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private List<AppVar> GetVariable(int value)
        {
            List<ViewModel.AppVar> appVars = new List<ViewModel.AppVar>();
            EFDataModel.DevOps.ConfigVariable efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.id == value).FirstOrDefault();
            List<EFDataModel.DevOps.Component> varComponents = efConfigVar.Components.ToList();
            foreach (EFDataModel.DevOps.Component vc in varComponents)
            {
                ViewModel.ConfigVariable configVar = ReturnConfigVariable(efConfigVar);
                AppVar appVarModel = new AppVar(configVar);
                appVarModel.componentId = vc.id;
                appVarModel.componentName = vc.component_name ?? string.Empty;

                foreach (var app in vc.Applications)
                {
                    if (string.IsNullOrWhiteSpace(appVarModel.applicationNames))
                        appVarModel.applicationNames = app.application_name;
                    else
                        //appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                        appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", app.application_name);
                }
                appVarModel.values.AddRange(configVar.ConfigVariableValues);
                appVars.Add(appVarModel);
            }
            return appVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the variable described by appValue. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <exception cref="InvalidOperationException">    Thrown when the requested operation is
        ///                                                 invalid. </exception>
        ///
        /// <param name="appValue"> The application value. </param>
        ///
        /// <returns>   An AppVar. </returns>
        ///-------------------------------------------------------------------------------------------------
        private AppVar AddUpdateVariable(AppVar appValue)
        {
            EFDataModel.DevOps.ConfigVariable efConfig = DevOpsContext.ConfigVariables.Where(x => x.id == appValue.configvar_id).FirstOrDefault();
            if (efConfig == null)
            {
                efConfig = DevOpsContext.ConfigVariables.Where(x => x.attribute == appValue.attribute
                                                                        && x.ConfigFile.file_name == appValue.fileName
                                                                        && x.element == appValue.configElement
                                                                        && x.key == appValue.key
                                                                        && x.value_name == appValue.valueName
                                                                        && x.parent_element == appValue.configParentElement).FirstOrDefault();
            }

            List<EFDataModel.DevOps.ConfigVariableValue> efConfigValueList = new List<EFDataModel.DevOps.ConfigVariableValue>();
            EFDataModel.DevOps.Component efComponent;
            EFDataModel.DevOps.ConfigFile efConfigFile = new EFDataModel.DevOps.ConfigFile();

            if (appValue.componentId != null)
            {
                efComponent = DevOpsContext.Components.Where(y => y.id == appValue.componentId).FirstOrDefault();
            }
            else if (!string.IsNullOrWhiteSpace(appValue.componentName))
            {
                efComponent = DevOpsContext.Components.Where(y => y.component_name == appValue.componentName).FirstOrDefault();
            }
            else
                throw new ArgumentNullException("No Component provided to add variable.");
            if (!string.IsNullOrWhiteSpace(appValue.fileName))
                efConfigFile = DevOpsContext.ConfigFiles.Where(x => x.file_name == appValue.fileName).FirstOrDefault();
            if (efConfig == null)
            {
                if (string.IsNullOrWhiteSpace(appValue.configElement))
                    appValue.configElement = appValue.key;
                efConfig = new EFDataModel.DevOps.ConfigVariable()
                {
                    active = true,
                    element = appValue.configElement,

                    // ToDo: Create XML element based on AppVar
                    // full_element = appValue.fullElement,
                    // full_element = getFullElement(appValue),

                    key = appValue.key ?? appValue.configElement,
                    attribute = appValue.attribute ?? "",
                    parent_element = appValue.configParentElement ?? "",
                    value_name = appValue.valueName ?? "",
                    full_element = appValue.fullElement ?? "",
                    ConfigFile = efConfigFile ?? new EFDataModel.DevOps.ConfigFile(),
                    create_date = DateTime.Now,
                    modify_date = DateTime.Now,
                };
                if (appValue.values.Count > 0)
                {
                    foreach (var val in appValue.values)
                    {
                        efConfig.ConfigVariableValues.Add(new EFDataModel.DevOps.ConfigVariableValue()
                        {
                            configvar_id = val.configvar_id,
                            create_date = val.create_date ?? DateTime.Now,
                            modify_date = val.modify_date ?? DateTime.Now,
                            environment_type = val.environment,
                            published = val.published ?? false,
                            published_date = val.publish_date,
                            value = val.value
                        });
                    }
                }
                else
                {
                    efConfig.ConfigVariableValues.Add(new EFDataModel.DevOps.ConfigVariableValue()
                    {
                        create_date = DateTime.Now,
                        modify_date = DateTime.Now,
                        environment_type = "development",
                        published = false,
                        value = "",
                    });
                }
                efConfig.Components.Add(efComponent);
                DevOpsContext.ConfigVariables.Add(efConfig);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(appValue.fileName))
                    efConfigFile = efConfig.ConfigFile;
                efConfigValueList = efConfig.ConfigVariableValues.ToList();
                if (!(efConfig.element == appValue.configElement &&
                    efConfig.key == appValue.key &&
                    efConfig.attribute == appValue.attribute &&
                    efConfig.parent_element == appValue.configParentElement &&
                    efConfig.value_name == appValue.valueName))
                {
                    efConfig.element = appValue.configElement;
                    efConfig.key = appValue.key ?? appValue.configElement;
                    efConfig.attribute = appValue.attribute ?? "";
                    efConfig.parent_element = appValue.configParentElement ?? "";
                    efConfig.full_element = appValue.fullElement ?? "";
                    efConfig.value_name = appValue.valueName ?? "";

                    // ToDo: Create XML element based on AppVar
                    // efConfig.full_element = getFullElement(appValue),

                    efConfig.ConfigFile = efConfigFile = new EFDataModel.DevOps.ConfigFile();
                    //efConfig.create_date = efConfig.create_date;
                    efConfig.modify_date = DateTime.Now;
                }
                foreach (var val in appValue.values)
                {
                    var efConfigValue = efConfigValueList.Where(x => x.configvar_id == val.configvar_id
                        && x.environment_type == val.environment).FirstOrDefault();
                    if (efConfigValue == null)
                    {
                        efConfigValue = new EFDataModel.DevOps.ConfigVariableValue()
                        {
                            configvar_id = appValue.configvar_id.Value,
                            environment_type = val.environment,
                            value = val.value,
                            create_date = DateTime.Now,
                            modify_date = DateTime.Now,
                            published_date = val.publish_date
                        };
                        DevOpsContext.ConfigVariableValues.Add(efConfigValue);
                    }
                }
            }
            try
            {
                DevOpsContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error saving to Database", e);
            }
            return GetVariable(appValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/24/2017. </remarks>
        ///
        /// <param name="appValue"> The application value. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        private AppVar GetVariable(AppVar appValue)
        {
            EFDataModel.DevOps.ConfigVariable efConfigVar;
            if (appValue.configvar_id != null)
                efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.id == appValue.configvar_id).FirstOrDefault();
            else
                efConfigVar = DevOpsContext.ConfigVariables.Where(x => x.attribute == appValue.attribute
                                                                        && x.ConfigFile.file_name == appValue.fileName
                                                                        && x.element == appValue.configElement
                                                                        && x.key == appValue.key
                                                                        && x.value_name == appValue.valueName
                                                                        && x.parent_element == appValue.configParentElement)
                                                           .FirstOrDefault();
            //EFDataModel.DevOps.Component varComponent = efConfigVar.Components.Where(y => y.id == appValue.componentId).FirstOrDefault();
            var varComponent = efConfigVar.Components.Where(y => y.id == appValue.componentId).FirstOrDefault();
            ViewModel.ConfigVariable configVar = ReturnConfigVariable(efConfigVar);
            AppVar appVar = new AppVar(configVar);
            appVar.componentId = varComponent.id;
            appVar.componentName = varComponent.component_name ?? string.Empty;

            foreach (var app in varComponent.Applications)
            {
                if (string.IsNullOrWhiteSpace(appVar.applicationNames))
                    appVar.applicationNames = app.application_name;
                else
                    //appVarModel.applicationNames = string.Concat(appVarModel.applicationNames, ", ", Environment.NewLine, app.application_name);
                    appVar.applicationNames = string.Concat(appVar.applicationNames, ", ", app.application_name);
            }
            appVar.values.AddRange(configVar.ConfigVariableValues);
            return appVar;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns application variable. </summary>
        ///
        /// <remarks>   Patman, 5/10/2017. </remarks>
        ///
        /// <param name="a">    The Application to process. </param>
        ///
        /// <returns>   The application variable. </returns>
        ///-------------------------------------------------------------------------------------------------

        private ViewModel.Application ReturnApplicationVariable(EFDataModel.DevOps.Application a)
        {
            return new ViewModel.Application()
            {
                active = a.active,
                application_name = a.application_name,
                create_date = a.create_date,
                id = a.id,
                modify_date = a.modify_date,
                release = a.release,
                Components = EfToVmConverter.EfComponentListToVm(a.Components).ToList(),
            };
        }
    }
}

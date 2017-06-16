using EFDataModel.DevOps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ManageComponents
    {
        public string userName { get; set; }
        DevOpsEntities DevOpsContext;
        ConvertObjects_Reflection EfToVmConverter = new ConvertObjects_Reflection();

        public ManageComponents()
        {
            DevOpsContext = new DevOpsEntities();
        }
        public ManageComponents(DevOpsEntities entities)
        {
            DevOpsContext = entities;
        }

        public ManageComponents(string conn)
        {
            DevOpsContext = new DevOpsEntities(conn);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the component. </summary>
        ///
        /// <remarks>   Pdelosreyes, 6/15/2017. </remarks>
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
                    last_modify_user = this.userName,
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
                    last_modify_user = comp.last_modify_user,
                    relative_path = comp.relative_path,
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
                    create_date = DateTime.Now,
                    modify_date = vmComp.modify_date ?? DateTime.Now,
                    last_modify_user = vmComp.last_modify_user,
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
                comp.last_modify_user = vmComp.last_modify_user;
                comp.relative_path = vmComp.relative_path;
            }
            return comp;
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

        public ViewModel.Component ReturnComponentVariable(EFDataModel.DevOps.Component component, bool noVal = false)
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
                    last_modify_user = component.last_modify_user,
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
                    last_modify_user = component.last_modify_user,
                    MachineComponentPaths = EfToVmConverter.EfMachineComponentPathListToVm(component.MachineComponentPathMaps),
                    Applications = EfToVmConverter.EfAppListToVm(component.Applications),
                    ConfigVariables = EfToVmConverter.EfConfigListToVm(component.ConfigVariables).ToList(),
                };
            }
        }
    }
}

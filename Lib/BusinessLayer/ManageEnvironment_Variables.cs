using System;
using System.Collections.Generic;
using CommonUtils.EnvironmentVariables;
using System.Linq;
using EFDataModel.DevOps;
using ViewModel;

namespace BusinessLayer
{
    public class ManageEnvironment_Variables
    {
        public EnvironmentVariableFunctions enVars { get; private set; }
        public string userName { get; set; }
        public string machineName { get; private set; }
        DevOpsEntities DevOpsContext;
        ConvertObjects_Reflection EfToVmConverter = new ConvertObjects_Reflection();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///-------------------------------------------------------------------------------------------------
        public ManageEnvironment_Variables()
        {
            if (enVars == null)
                enVars = new EnvironmentVariableFunctions();
            if (string.IsNullOrEmpty(machineName))
                machineName = Environment.MachineName.ToString();
            DevOpsContext = new DevOpsEntities();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets environment value. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The environment value. </returns>
        ///-------------------------------------------------------------------------------------------------
        public EnvVariable GetEnvValue(string key)
        {
            return enVars.GetEnvironmentVariable(key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the environment variable described by key. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   A ConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult RemoveEnvVariable(string key, string enVarType = null)
        {
            return enVars.RemoveEnvironmentVariable(key, enVarType);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds an environment variable to 'value'. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <param name="key">      The key. </param>
        /// <param name="value">    The value. </param>
        ///
        /// <returns>   A ConfigVariableResult. </returns>
        ///-------------------------------------------------------------------------------------------------
        public Enums.ModifyResult AddEnvVariable(string key, string value, string keyType = null)
        {
            return enVars.SetEnvironmentVariable(key, value, keyType);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <returns>   all environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetAllDbEnvVariables()
        {
            List<EnvVariable> enVariables = new List<EnvVariable>();
            List<EFDataModel.DevOps.EnvironmentVariable> dbEnVars = (from en in DevOpsContext.EnvironmentVariables
                          where en.Machines.Contains(
                              (from mac in DevOpsContext.Machines
                              where mac.machine_name == machineName
                              select mac).FirstOrDefault()
                              )
                          select en).ToList();
            foreach (var x in dbEnVars)
            {
                var type = (EnvironmentVariableTarget)Enum.Parse(typeof(EnvironmentVariableTarget), x.type);

                enVariables.Add(new EnvVariable()
                {
                    key = x.key,
                    value = x.value,
                    varType = type
                });
            }
            return enVariables;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all en variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <returns>   all en variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.EnvironmentDtoVariable> GetAllEnVariables(int? appId = null)
        {
            var enVars = new List<ViewModel.EnvironmentDtoVariable>();
            var allEnVars = new List<ViewModel.EnvironmentDtoVariable>();
            if (appId != null)
            {
                allEnVars = allEnVars;
                //allEnVars = (from vars in DevOpsContext.EnvironmentVariables
                //             where vars.Applications.Contains(
                //                (from app in DevOpsContext.Applications
                //                 where app.id == appId
                //                 select app).FirstOrDefault()
                //                )
                //             select vars).ToList();
            }
            else
            {
                allEnVars = allEnVars;
                //allEnVars = (from vars in DevOpsContext.EnvironmentVariables
                //             where vars.Machines.Contains(
                //                (from mac in DevOpsContext.Machines
                //                 where mac.machine_name == machineName
                //                 select mac).FirstOrDefault()
                //                )
                //             select vars).ToList();
            }
            foreach (var env in allEnVars)
            {
                enVars.Add(new ViewModel.EnvironmentDtoVariable()
                {
                    id = env.id,
                    active = env.active,
                    create_date = env.create_date,
                    key = env.key,
                    modify_date = env.modify_date,
                    path = env.path,
                    type = env.type,
                    value = env.value,
                    //Applications = EfToVmConverter.EfAppListToVm(env.Applications),
                    //Machines = EfToVmConverter.EfMachineListToVm(env.Machines)
                });
            }
            return enVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <returns>   all environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetAllEnvVariables(string suffix = null)
        {
            if (string.IsNullOrEmpty(suffix))
            {
                EnvironmentVariableFunctions enVarProc = new EnvironmentVariableFunctions()
                {
                    myVarSuffix = "marcom"
                };
                return enVarProc.GetAllEnvironmentVariables();
            }
            return enVars.GetAllEnvironmentVariables();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all en variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <returns>   all en variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ViewModel.EnvironmentDtoVariable> GetAllEnVariables()
        {
            var enVars = new List<ViewModel.EnvironmentDtoVariable>();
            var allEnVars = (from vars in DevOpsContext.EnvironmentVariables
                             select vars).ToList();

            foreach (var env in allEnVars)
            {
                enVars.Add(new ViewModel.EnvironmentDtoVariable()
                {
                    id = env.id,
                    active = env.active,
                    create_date = env.create_date,
                    key = env.key,
                    modify_date = env.modify_date,
                    path = env.path,
                    type = env.type,
                    value = env.value,
                    Applications = EfToVmConverter.EfAppListToVm(env.Applications),
                    Machines = EfToVmConverter.EfMachineListToVm(env.Machines)
                });
            }
            return enVars;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets all difference environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 3/30/2017. </remarks>
        ///
        /// <param name="suffix">   (Optional) The suffix. </param>
        ///
        /// <returns>   all difference environment variables. </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<EnvVariable> GetAllDiffEnvVariables(string suffix = null)
        {
            List<EnvVariable> localEnVars = new List<EnvVariable>();
            if (string.IsNullOrEmpty(suffix))
            {
                EnvironmentVariableFunctions enVarProc = new EnvironmentVariableFunctions()
                {
                    myVarSuffix = "marcom"
                };
                localEnVars = enVarProc.GetAllEnvironmentVariables();
            }
            else
            {
                localEnVars = enVars.GetAllEnvironmentVariables();
            }
            List<EnvVariable> dbVars = GetAllDbEnvVariables();

            List<EnvVariable> combinedDiff = new List<EnvVariable>();

            combinedDiff.AddRange(
                        (from keys in localEnVars
                         join db in dbVars
                             on new
                             {
                                 JoinProperty1 = keys.key,
                                 JoinProperty2 = keys.value,
                                 JoinProperty3 = keys.varType
                             }
                             equals
                             new
                             {
                                 JoinProperty1 = db.key,
                                 JoinProperty2 = db.value,
                                 JoinProperty3 = db.varType
                             }
                             into combined
                         from both in combined.DefaultIfEmpty()
                         where both == null
                         select keys).ToList());

            combinedDiff.AddRange(
                        (from db in dbVars
                         join keys in localEnVars
                             on new
                             {
                                 JoinProperty1 = db.key,
                                 JoinProperty2 = db.value,
                                 JoinProperty3 = db.varType
                             }
                             equals new
                             {
                                 JoinProperty1 = keys.key,
                                 JoinProperty2 = keys.value,
                                 JoinProperty3 = keys.varType
                             }
                             into combined
                         from both in combined.DefaultIfEmpty()
                         where both == null
                         select db).ToList());
            return combinedDiff;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigVariableResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> RemoveAllEnvVariables()
        {
            var keyList = new List<string>();

            var resultList = new List<ConfigModifyResult>();
            foreach (var key in keyList)
            {
                resultList.Add(new ConfigModifyResult()
                {
                    key = key,
                    result = enVars.RemoveEnvironmentVariable(key)
                });
            }
            return resultList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds all environment variables. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/9/2017. </remarks>
        ///
        /// <returns>   A List&lt;ConfigVariableResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------
        public List<ConfigModifyResult> AddAllEnvVariables()
        {
            var keyValueList = new List<AttributeKeyValuePair>();
            var resultList = new List<ConfigModifyResult>();
            foreach (var keyValue in keyValueList)
            {
                resultList.Add(new ConfigModifyResult()
                {
                    key = keyValue.key,
                    result = enVars.SetEnvironmentVariable(keyValue.key, keyValue.value)
                });
            }
            return resultList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a variable. </summary>
        ///
        /// <remarks>   Pdelosreyes, 2/23/2017. </remarks>
        ///
        /// <param name="machineId">    Identifier for the machine. </param>
        /// <param name="varId">        Identifier for the variable. </param>
        /// <param name="varType">      Type of the variable. </param>
        ///
        /// <returns>   The variable. </returns>
        ///-------------------------------------------------------------------------------------------------
        public MachineAppVars GetVariable(int machineId, int varId, string varType)
        {
            throw new NotImplementedException();
        }

        public MachineAppVars AddVariable(MachineAppVars value)
        {
            throw new NotImplementedException();
        }

        public MachineAppVars DeleteVariable(int id)
        {
            throw new NotImplementedException();
        }

        public List<MachineAppVars> GetMachineVariables(int machineId)
        {
            throw new NotImplementedException();
        }

        public MachineAppVars GetGlobalVariable(int varId, string varType)
        {
            throw new NotImplementedException();
        }
    }
}

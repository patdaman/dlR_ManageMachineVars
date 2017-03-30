﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFDataModel.DevOps
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DevOpsEntities : DbContext
    {
        public DevOpsEntities()
            : base("name=DevOpsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Component> Components { get; set; }
        public virtual DbSet<ConfigFileAttribute> ConfigFileAttributes { get; set; }
        public virtual DbSet<ConfigFileElement> ConfigFileElements { get; set; }
        public virtual DbSet<ConfigVariable> ConfigVariables { get; set; }
        public virtual DbSet<ConfigVariableValue> ConfigVariableValues { get; set; }
        public virtual DbSet<Enum_EnvironmentType> Enum_EnvironmentType { get; set; }
        public virtual DbSet<Enum_EnvironmentVariableType> Enum_EnvironmentVariableType { get; set; }
        public virtual DbSet<Enum_Locations> Enum_Locations { get; set; }
        public virtual DbSet<EnvironmentVariable> EnvironmentVariables { get; set; }
        public virtual DbSet<MachineComponentPathMap> MachineComponentPathMaps { get; set; }
        public virtual DbSet<Machine> Machines { get; set; }
        public virtual DbSet<ServerGroup> ServerGroups { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<ExecutionHistory> ExecutionHistories { get; set; }
        public virtual DbSet<Script> Scripts { get; set; }
        public virtual DbSet<vi_ConfigVariables> vi_ConfigVariables { get; set; }
        public virtual DbSet<ConfigFile> ConfigFiles { get; set; }
    
        public virtual int usp_AddComponentsToMachine(Nullable<int> configVarId, Nullable<int> machineId, string machineName, Nullable<int> appId, string appName, string rootConfigPath)
        {
            var configVarIdParameter = configVarId.HasValue ?
                new ObjectParameter("ConfigVarId", configVarId) :
                new ObjectParameter("ConfigVarId", typeof(int));
    
            var machineIdParameter = machineId.HasValue ?
                new ObjectParameter("MachineId", machineId) :
                new ObjectParameter("MachineId", typeof(int));
    
            var machineNameParameter = machineName != null ?
                new ObjectParameter("MachineName", machineName) :
                new ObjectParameter("MachineName", typeof(string));
    
            var appIdParameter = appId.HasValue ?
                new ObjectParameter("AppId", appId) :
                new ObjectParameter("AppId", typeof(int));
    
            var appNameParameter = appName != null ?
                new ObjectParameter("AppName", appName) :
                new ObjectParameter("AppName", typeof(string));
    
            var rootConfigPathParameter = rootConfigPath != null ?
                new ObjectParameter("RootConfigPath", rootConfigPath) :
                new ObjectParameter("RootConfigPath", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_AddComponentsToMachine", configVarIdParameter, machineIdParameter, machineNameParameter, appIdParameter, appNameParameter, rootConfigPathParameter);
        }
    
        public virtual int usp_GenerateAuditTables(string tableName, string auditNameExtention, Nullable<bool> dropAuditTable)
        {
            var tableNameParameter = tableName != null ?
                new ObjectParameter("TableName", tableName) :
                new ObjectParameter("TableName", typeof(string));
    
            var auditNameExtentionParameter = auditNameExtention != null ?
                new ObjectParameter("AuditNameExtention", auditNameExtention) :
                new ObjectParameter("AuditNameExtention", typeof(string));
    
            var dropAuditTableParameter = dropAuditTable.HasValue ?
                new ObjectParameter("DropAuditTable", dropAuditTable) :
                new ObjectParameter("DropAuditTable", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_GenerateAuditTables", tableNameParameter, auditNameExtentionParameter, dropAuditTableParameter);
        }
    
        public virtual int usp_GetConfigVariables(Nullable<int> machineId, string machineName, Nullable<int> applicationId, string applicationName, string environment, Nullable<bool> displayOnlyActive)
        {
            var machineIdParameter = machineId.HasValue ?
                new ObjectParameter("MachineId", machineId) :
                new ObjectParameter("MachineId", typeof(int));
    
            var machineNameParameter = machineName != null ?
                new ObjectParameter("MachineName", machineName) :
                new ObjectParameter("MachineName", typeof(string));
    
            var applicationIdParameter = applicationId.HasValue ?
                new ObjectParameter("ApplicationId", applicationId) :
                new ObjectParameter("ApplicationId", typeof(int));
    
            var applicationNameParameter = applicationName != null ?
                new ObjectParameter("ApplicationName", applicationName) :
                new ObjectParameter("ApplicationName", typeof(string));
    
            var environmentParameter = environment != null ?
                new ObjectParameter("Environment", environment) :
                new ObjectParameter("Environment", typeof(string));
    
            var displayOnlyActiveParameter = displayOnlyActive.HasValue ?
                new ObjectParameter("DisplayOnlyActive", displayOnlyActive) :
                new ObjectParameter("DisplayOnlyActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_GetConfigVariables", machineIdParameter, machineNameParameter, applicationIdParameter, applicationNameParameter, environmentParameter, displayOnlyActiveParameter);
        }
    
        public virtual int usp_InsertErrorDetails()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_InsertErrorDetails");
        }
    
        public virtual int usp_SearchAuditTablesForInvalidUser(string validUserPrefix, string validCommaSeparatedList, string invalidCommaSeparatedList, string validSchemaCommaSeparatedList, string invalidSchemaCommaSeparatedList, string validTableCommaSeparatedList, string invalidTableCommaSeparatedList, Nullable<bool> printQuery, Nullable<bool> deleteRows)
        {
            var validUserPrefixParameter = validUserPrefix != null ?
                new ObjectParameter("ValidUserPrefix", validUserPrefix) :
                new ObjectParameter("ValidUserPrefix", typeof(string));
    
            var validCommaSeparatedListParameter = validCommaSeparatedList != null ?
                new ObjectParameter("ValidCommaSeparatedList", validCommaSeparatedList) :
                new ObjectParameter("ValidCommaSeparatedList", typeof(string));
    
            var invalidCommaSeparatedListParameter = invalidCommaSeparatedList != null ?
                new ObjectParameter("InvalidCommaSeparatedList", invalidCommaSeparatedList) :
                new ObjectParameter("InvalidCommaSeparatedList", typeof(string));
    
            var validSchemaCommaSeparatedListParameter = validSchemaCommaSeparatedList != null ?
                new ObjectParameter("ValidSchemaCommaSeparatedList", validSchemaCommaSeparatedList) :
                new ObjectParameter("ValidSchemaCommaSeparatedList", typeof(string));
    
            var invalidSchemaCommaSeparatedListParameter = invalidSchemaCommaSeparatedList != null ?
                new ObjectParameter("InvalidSchemaCommaSeparatedList", invalidSchemaCommaSeparatedList) :
                new ObjectParameter("InvalidSchemaCommaSeparatedList", typeof(string));
    
            var validTableCommaSeparatedListParameter = validTableCommaSeparatedList != null ?
                new ObjectParameter("ValidTableCommaSeparatedList", validTableCommaSeparatedList) :
                new ObjectParameter("ValidTableCommaSeparatedList", typeof(string));
    
            var invalidTableCommaSeparatedListParameter = invalidTableCommaSeparatedList != null ?
                new ObjectParameter("InvalidTableCommaSeparatedList", invalidTableCommaSeparatedList) :
                new ObjectParameter("InvalidTableCommaSeparatedList", typeof(string));
    
            var printQueryParameter = printQuery.HasValue ?
                new ObjectParameter("PrintQuery", printQuery) :
                new ObjectParameter("PrintQuery", typeof(bool));
    
            var deleteRowsParameter = deleteRows.HasValue ?
                new ObjectParameter("DeleteRows", deleteRows) :
                new ObjectParameter("DeleteRows", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_SearchAuditTablesForInvalidUser", validUserPrefixParameter, validCommaSeparatedListParameter, invalidCommaSeparatedListParameter, validSchemaCommaSeparatedListParameter, invalidSchemaCommaSeparatedListParameter, validTableCommaSeparatedListParameter, invalidTableCommaSeparatedListParameter, printQueryParameter, deleteRowsParameter);
        }
    
        public virtual int usp_ViewErrorTables(Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate, Nullable<int> rows)
        {
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("StartDate", startDate) :
                new ObjectParameter("StartDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            var rowsParameter = rows.HasValue ?
                new ObjectParameter("Rows", rows) :
                new ObjectParameter("Rows", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_ViewErrorTables", startDateParameter, endDateParameter, rowsParameter);
        }
    }
}

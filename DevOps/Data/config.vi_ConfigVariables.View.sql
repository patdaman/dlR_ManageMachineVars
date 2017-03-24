USE [DevOps]
GO
/****** Object:  View [config].[vi_ConfigVariables]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[vi_ConfigVariables]'))
DROP VIEW [config].[vi_ConfigVariables]
GO
/****** Object:  View [config].[vi_ConfigVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[vi_ConfigVariables]'))
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		Patrick de los Reyes
-- Create date: 2017-03-01
-- Description:	Returns Config variables based on Machine and Application
-- =============================================
CREATE VIEW [config].[vi_ConfigVariables] 

AS

	SELECT DISTINCT 
		ConfigVar.id					AS config_id
		, Mac.id						AS machine_id
		, Mac.machine_name
		, App.id						AS application_id
		, App.application_name
		, Com.id						AS component_id
		, Com.component_name
		, ConVal.environment_type
		, ConfigVar.parent_element
		, ConfigVar.element
		, ConfigVar.[key_name]
		, ConfigVar.[key]
		, ConfigVar.value_name
		, ConVal.[value]
		, varPath.[config_path]
		, ConVal.modify_date
	FROM [config].[ConfigVariables] AS ConfigVar
		INNER JOIN [config].[ComponentConfigVariables] AppVar
			ON ConfigVar.id = AppVar.configvariable_id
		INNER JOIN [config].[Components] AS Com
			ON AppVar.component_id = Com.id
		INNER JOIN [config].[MachineComponentPath] AS [varPath]
			ON Com.id = varPath.component_id
		INNER JOIN [config].[Machines] AS Mac
			ON varPath.machine_id = Mac.id
		LEFT OUTER JOIN [config].[AppComponents] AS AppCom
			ON Com.id = AppCom.component_id
		LEFT OUTER JOIN [config].[Applications] AS App
			ON AppCom.application_id = App.id
		LEFT OUTER JOIN [config].[ConfigVariableValue] AS ConVal
			ON ConfigVar.id = ConVal.configvar_id


' 
GO

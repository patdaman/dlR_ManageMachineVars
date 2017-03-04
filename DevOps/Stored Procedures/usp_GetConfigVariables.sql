-- =============================================
-- Author:		Patrick de los Reyes
-- Create date: 2017-03-01
-- Description:	Returns Config variables based on Machine and Application
-- =============================================
CREATE PROCEDURE [config].[usp_GetConfigVariables] 
	@MachineId			int = 0, 
	@MachineName		varchar(128) = '',
	@ApplicationId		int = 0,
	@ApplicationName	varchar(128) = '',
	@Environment		VARCHAR(128) = '',
	@DisplayOnlyActive	BIT = 1
AS
BEGIN
	SET NOCOUNT ON;

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
		, ConfigVar.key_name
		, ConfigVar.[key]
		, ConfigVar.value_name
		, ConVal.[value]
		, varPath.[config_path] + Com.relative_path	AS config_path
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
	WHERE 1=1
		AND (Mac.id = @MachineId
			OR @MachineId = 0)
		AND (App.id = @ApplicationId
			OR @ApplicationId = 0)
		AND (@MachineName LIKE Mac.machine_name + '%'
			OR @MachineName = '')
		AND (@ApplicationName LIKE App.application_name + '%'
			OR @ApplicationName = '')
		AND (ConVal.environment_type = @Environment
			OR @Environment = '')
		AND (Mac.active = @DisplayOnlyActive
			AND App.active = @DisplayOnlyActive
			AND ConfigVar.active = @DisplayOnlyActive
			)
END
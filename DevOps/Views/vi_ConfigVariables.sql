








-- =============================================
-- Author:		Patrick de los Reyes
-- Create date: 2017-03-01
-- Description:	Returns Config variables based on Machine and Application
-- =============================================
CREATE VIEW [config].[vi_ConfigVariables] 

AS

	SELECT DISTINCT 
		ConfigVar.id					AS config_id
		, Com.id						AS component_id
		, Com.component_name
		, ConFile.[file_name]
		, ConVal.environment_type
		, ConfigVar.parent_element
		, ConfigVar.element
		, ConfigVar.attribute
		, ConfigVar.[key]
		, ConfigVar.value_name
		, ConVal.[value]
		, ConfigVar.create_date
		, ConVal.modify_date
		, ConVal.published_date
		, ConVal.published
	FROM [config].[ConfigVariables] AS ConfigVar
		INNER JOIN [config].[ComponentConfigVariablesMap] AppVar
			ON ConfigVar.id = AppVar.configvariable_id
		INNER JOIN [config].[Components] AS Com
			ON AppVar.component_id = Com.id
		LEFT OUTER JOIN [config].[ConfigVariableValue] AS ConVal
			ON ConfigVar.id = ConVal.configvar_id
		LEFT OUTER JOIN [config].[ConfigFile] AS ConFile
			ON ConfigVar.configfile_id = ConFile.id
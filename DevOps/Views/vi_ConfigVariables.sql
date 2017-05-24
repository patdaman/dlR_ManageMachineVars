










-- =============================================
-- Author:		Patrick de los Reyes
-- Create date: 2017-03-01
-- Description:	Returns Config variables based on Machine and Application
-- =============================================
CREATE VIEW [config].[vi_ConfigVariables] 

AS

	SELECT DISTINCT 
		ConfigVar.id							AS config_id
		, Com.id								AS component_id
		, MAX(Com.component_name)				AS component_name
		, ConFile.[file_name]					AS [fileName]
		, STUFF((SELECT ', ' + application_name + ' ' + release
			FROM [config].[Applications] a 
				INNER JOIN [config].[AppComponents] b ON a.id = b.application_id
			WHERE b.component_id = Com.id 
			FOR XML PATH('')), 1, 2, '') AS applications
		, ConVal.environment_type				AS environment
		, MAX(ConfigVar.parent_element)			AS parent_element
		, MAX(ConfigVar.full_element)			AS full_element
		, MAX(ConfigVar.element)				AS element
		, MAX(ConfigVar.attribute)				AS attribute
		, MAX(ConfigVar.[key])					AS [key]
		, MAX(ConfigVar.value_name)				AS value_name
		, MAX(ConVal.[value])					AS value
		, MAX(ConfigVar.create_date)			AS create_date
		, MAX(ConVal.modify_date)				AS modify_date
		, MAX(ConVal.last_modify_user)			AS last_modify_user
		, MAX(ConVal.published_date)			AS published_date
		, CAST(MAX(CAST(ConVal.published AS int)) AS BIT)	AS published
	FROM [config].[ConfigVariables] AS ConfigVar
		INNER JOIN [config].[ComponentConfigVariablesMap] AppVar
			ON ConfigVar.id = AppVar.configvariable_id
		INNER JOIN [config].[Components] AS Com
			ON AppVar.component_id = Com.id
		LEFT OUTER JOIN
			(SELECT Apps.id, AppCompMap.component_id, Apps.application_name, Apps.release
			FROM [config].[Applications] AS Apps
				INNER JOIN [config].[AppComponents] AS AppCompMap
					ON Apps.id = AppCompMap.application_id
			) AS Apps 
			ON Com.id = Apps.component_id
		LEFT OUTER JOIN [config].[ConfigVariableValue] AS ConVal
			ON ConfigVar.id = ConVal.configvar_id
		LEFT OUTER JOIN [config].[ConfigFile] AS ConFile
			ON ConfigVar.configfile_id = ConFile.id
	GROUP BY ConfigVar.id, Com.id, ConVal.environment_type, ConFile.[file_name]
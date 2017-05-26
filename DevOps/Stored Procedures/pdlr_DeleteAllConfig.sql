-- =============================================
-- Author:		Patrick de los Reyes
-- Create date: 2017-05-09
-- Description:	Delete All Imported Config Data!
-- =============================================
CREATE PROCEDURE [config].[pdlr_DeleteAllConfig] 
	-- Add the parameters for the stored procedure here
	@PrintOnly int = 1, 
	@Confirm int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @SQL VARCHAR(2000)
	SET @SQL = 'DELETE FROM [config].[AppComponents];
DELETE FROM [config].[Applications];
DELETE FROM [config].[ComponentConfigVariablesMap];
DELETE FROM [config].[MachineComponentPathMap];
DELETE FROM [config].[ConfigFileElements];
DELETE FROM [config].[ConfigFileAttributes];
DELETE FROM [config].[ConfigVariableValue];
DELETE FROM [config].[ConfigVariables];
DELETE FROM [config].[ConfigFile];
DELETE FROM [config].[Components];
EXEC [config].[usp_GenerateAuditTables]
		@TableName = N''ConfigVariableValue'',
		@Schema = NULL,
		@ExcludedColumnCSV = N''last_modify_user,modify_date,create_date'',
		@AuditNameExtention = NULL,
		@OnInsert = 0,
		@OnUpdate = 1,
		@OnDelete = 1,
		@PrintOnly = 0,
		@DropAuditTable = 1
		;
EXEC [config].[usp_GenerateAuditTables]
		@TableName = N''ConfigVariables'',
		@Schema = NULL,
		@ExcludedColumnCSV = N''last_modify_user,modify_date,create_date'',
		@AuditNameExtention = NULL,
		@OnInsert = 0,
		@OnUpdate = 1,
		@OnDelete = 1,
		@PrintOnly = 0,
		@DropAuditTable = 1
		;
EXEC [config].[usp_GenerateAuditTables]
		@TableName = N''Components'',
		@Schema = NULL,
		@ExcludedColumnCSV = N''last_modify_user,modify_date,create_date'',
		@AuditNameExtention = NULL,
		@OnInsert = 0,
		@OnUpdate = 1,
		@OnDelete = 1,
		@PrintOnly = 0,
		@DropAuditTable = 1
		;
EXEC [config].[usp_GenerateAuditTables]
		@TableName = N''Applications'',
		@Schema = NULL,
		@ExcludedColumnCSV = N''last_modify_user,modify_date,create_date'',
		@AuditNameExtention = NULL,
		@OnInsert = 0,
		@OnUpdate = 1,
		@OnDelete = 1,
		@PrintOnly = 0,
		@DropAuditTable = 1
		;
		'
	IF @PrintOnly = 1
	BEGIN
		PRINT @SQL
	END
	ELSE
	BEGIN
		IF @Confirm = 1 
		BEGIN
			PRINT @SQL
			EXEC (@SQL)
		END
	END
END
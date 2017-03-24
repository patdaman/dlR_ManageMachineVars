USE [DevOps]
GO
/****** Object:  StoredProcedure [config].[usp_AddComponentsToMachine]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[usp_AddComponentsToMachine]') AND type in (N'P', N'PC'))
DROP PROCEDURE [config].[usp_AddComponentsToMachine]
GO
/****** Object:  StoredProcedure [config].[usp_AddComponentsToMachine]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[usp_AddComponentsToMachine]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [config].[usp_AddComponentsToMachine] AS' 
END
GO
-- =============================================
-- Author:		Patrick de los Reyes
-- Create date: 2017-03-01
-- Description:	Add Config Variables
-- =============================================
ALTER PROCEDURE [config].[usp_AddComponentsToMachine] 
	@ConfigVarId		INT				= 0,
	@MachineId			INT				= 0, 
	@MachineName		VARCHAR(256)	= '',
	@AppId				INT				= 0,
	@AppName			VARCHAR(256)	= '',
	@RootConfigPath		VARCHAR(256)	= NULL
AS
BEGIN
	IF (@MachineId = 0 AND @MachineName = '')
	BEGIN
		DECLARE @msg NVARCHAR(2048) = FORMATMESSAGE(60000, 500, N'Missing Machine', N'Either Machine ID or NAME is required from [config].[Machines] table.');   
  		THROW 60000, @msg, 1;
	END

	IF (@AppId = 0 AND @AppName = '')
	BEGIN
		DECLARE @msg2 NVARCHAR(2048) = FORMATMESSAGE(60000, 500, N'Missing Application', N'Either the Application Name OR ID is required from [config].[Applications] table.');   
  		THROW 60000, @msg2, 1;
	END

	IF COALESCE(@AppId,0) = 0
		SELECT @AppId = A.id
		FROM [config].Applications A
		WHERE @AppName LIKE A.application_name

	IF COALESCE(@MachineID,0) = 0
		SELECT @MachineId = M.id
		FROM [config].Machines M
		WHERE @MachineName LIKE M.machine_name

	SET @RootConfigPath = COALESCE(@RootConfigPath, 'D:\US\printableConfig\')

	DECLARE @PathUpdate		BIT = 0
	DECLARE @PathInsert		BIT = 0

	-- ***************************************************************************************************** --
	-- Create Temp Table for Components
	-- ***************************************************************************************************** --
	DECLARE @Components AS TABLE 
		(machine_id int, machine_name varchar(128)
		, machine_path_inserted bit, path_updated bit
		, component_id int, component_name varchar(256)
		, config_path varchar(256))

	-- ***************************************************************************************************** --
	-- Create Cursor for Components within Application
	-- ***************************************************************************************************** --
	DECLARE @CompId				INT = 0
	DECLARE @CompName			VARCHAR(256) = ''
	DECLARE @CompPath			VARCHAR(256) = ''
	DECLARE @FullPath			VARCHAR(512) = ''

	DECLARE component_cursor CURSOR LOCAL FOR
	SELECT C.id
		, C.component_name
		, C.relative_path
	FROM Components C
	INNER JOIN AppComponents AC
		ON C.id = AC.component_id
	INNER JOIN Applications A
		ON AC.application_id = A.id
	WHERE 1=1
		AND A.id = @AppId
		AND C.active = 1

	OPEN component_cursor
	FETCH NEXT FROM component_cursor INTO @CompId, @CompName, @CompPath

	-- ***************************************************************************************************** --
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @FullPath = REPLACE(@RootConfigPath + @CompPath,'//','/')
		IF (@MachineId NOT IN (SELECT machine_id
								FROM [config].[MachineComponentPath]
								WHERE component_id = @CompId
								))
		BEGIN
			INSERT INTO [config].[MachineComponentPath]
					(machine_id, component_id, config_path)
			SELECT @MachineId, @CompId, @FullPath
			SET @PathInsert = 1
		END
		ELSE
		BEGIN
			UPDATE [config].[MachineComponentPath] SET
				[config_path] = @FullPath
			WHERE machine_id = @MachineId 
				AND component_id = @CompId
				AND [config_path] <> @FullPath
			SET @PathUpdate = 1
		END
		INSERT INTO @Components
			(machine_id, machine_name, machine_path_inserted, path_updated, component_id, component_name, config_path)
		SELECT 
			@MachineId, @MachineName, @PathInsert, @PathUpdate, @CompId, @CompName, @FullPath

		SET @PathInsert = 0
		SET @PathUpdate = 0
		FETCH NEXT FROM component_cursor INTO @CompId, @CompName, @CompPath
	END
	-- ***************************************************************************************************** --
	CLOSE component_cursor
	DEALLOCATE component_cursor

	SELECT *
	FROM @Components
END

GO

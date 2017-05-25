


CREATE PROCEDURE [config].[usp_GenerateAuditTables]
	-- ***************************************** --
	-- Must match the Table Name
	-- ***************************************** --
	@TableName					varchar(128) = 'ENTER_MY_TABLE_NAME'
	-- ***************************************** --
	-- @Schema Not yet implemented, 
	-- Code will automatically retrieve 
	--	the schema based on table name
	, @Schema					varchar(128)
	-- ***************************************** --
	, @ExcludedColumnCSV		varchar(512)
	, @AuditNameExtention		varchar(128)
	-- ***************************************** --
	-- 1 will include 'FOR INSERT, UPDATE, DELETE'
	-- NULL default will set all to true
	, @OnInsert					bit = 1
	, @OnUpdate					bit = 1
	, @OnDelete					bit = 1
	-- ***************************************** --
	-- 1 *default* will only print the query, not execute
	-- 0 will execute query as well as print
	, @PrintOnly				bit = 1
	-- ***************************************** --
	-- 1 will drop and recreate the audit table
	--	Anything else will not drop the audit table,
	--	null value will be set to 0.
	, @DropAuditTable			bit = 0
	-- ***************************************** --
AS
BEGIN --> 1
	SET NOCOUNT ON
	 
	DECLARE @sql				nvarchar(max)
	DECLARE @CreateStatement	varchar(150)
	DECLARE @SelectKeys			varchar(150)
	DECLARE @SelectFirstKey		varchar(150)
	DECLARE @SelectDelKeys		varchar(150)
	DECLARE @SelectKeyValues	varchar(150)
	DECLARE @SelectKeyJoin		varchar(150)
	DECLARE @SelectIndexKeys	varchar(150)
	DECLARE @SelectInsertKeys	varchar(250)
	DECLARE @TABLE_SCHEMA		sysname
	DECLARE @CRLF				char(2)

	-- Declare temp variable to fetch records into
	DECLARE @ColumnName			varchar(128)
	DECLARE @ColumnType			varchar(128)
	DECLARE @ColumnLength		int
	DECLARE @ColumnNullable		varchar(10)
	DECLARE @ColumnCollation	sysname
	DECLARE @ColumnPrecision	int
	DECLARE @ColumnScale		int

	DECLARE @Insert				varchar(128)
	DECLARE @Update				varchar(128)
	DECLARE @Delete				varchar(128)
	DECLARE @TriggerEvents		varchar(128)

	SET @CRLF = Char(13) + Char(10)
	SELECT @sql = ''
	, @CreateStatement = ''
	, @SelectKeys = ''
	, @SelectKeyJoin = ''
	, @SelectKeyValues = ''
	, @SelectDelKeys = ''
	, @SelectFirstKey = ''
	, @SelectIndexKeys = ''
	, @SelectInsertKeys = ''
	IF @AuditNameExtention IS NULL
		SET @AuditNameExtention = '_audit'
	IF @Schema IS NULL
		SET @Schema = ''
	IF @PrintOnly IS NULL
		SET @PrintOnly = 1
	IF @DropAuditTable IS NULL
		SET @DropAuditTable = 0

	IF COALESCE(@OnInsert, 0) = 1
		SET @Insert = 'INSERT, '
	ELSE
		SET @Insert = ''
	IF COALESCE(@OnUpdate, 0) = 1
		SET @Update = 'UPDATE, '
	ELSE
		SET @Update = ''
	IF COALESCE(@OnDelete, 0) = 1
		SET @Delete = 'DELETE, '
	ELSE
		SET @Delete = ''

	IF LEN(COALESCE(@ExcludedColumnCSV, '')) = 0
		SET @ExcludedColumnCSV = '('''''')'
	ELSE
		SET @ExcludedColumnCSV = '(''' + REPLACE(REPLACE(@ExcludedColumnCSV,' ',''),',',''',''') + ''')'

	IF LEN(@Insert + @Update + @Delete) = 0
		RAISERROR (15600,-1,-1, 'config.usp_GenerateAuditTables'); 
	ELSE
	BEGIN
		SET @TriggerEvents = @Insert + @Update + @Delete
		SET @TriggerEvents = LEFT(@TriggerEvents, LEN(@TriggerEvents) - 1)
	END

	SELECT TOP 1 @TABLE_SCHEMA = TABLE_SCHEMA
	FROM INFORMATION_SCHEMA.Tables 
	WHERE 
		TABLE_TYPE= 'BASE TABLE' 
			AND TABLE_NAME!= 'sysdiagrams'
			AND TABLE_NAME NOT LIKE  ('%' + @AuditNameExtention)
			--AND TABLE_NAME LIKE ('%' + @TableName + '%')
			AND TABLE_NAME LIKE (@TableName)

		DECLARE TABLECOLUMNS CURSOR LOCAL FOR
		SELECT KCU.COLUMN_NAME, C.DATA_TYPE, CASE WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN 0 ELSE C.CHARACTER_MAXIMUM_LENGTH END AS CHARACTER_MAXIMUM_LENGTH, C.IS_NULLABLE, C.NUMERIC_PRECISION, C.NUMERIC_SCALE
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
			JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU ON KCU.CONSTRAINT_SCHEMA = TC.CONSTRAINT_SCHEMA
			JOIN INFORMATION_SCHEMA.COLUMNS C ON KCU.COLUMN_NAME = C.COLUMN_NAME
				AND KCU.CONSTRAINT_NAME = TC.CONSTRAINT_NAME
				AND KCU.TABLE_SCHEMA = TC.TABLE_SCHEMA
				AND KCU.TABLE_NAME = TC.TABLE_NAME
		WHERE TC.CONSTRAINT_TYPE IN ('PRIMARY KEY','UNIQUE')
			AND KCU.TABLE_NAME LIKE @TableName
			AND C.TABLE_NAME LIKE @TableName
			--AND COLUMNPROPERTY(OBJECT_ID(@TableName),c.COLUMN_NAME,'isidentity')<>1
		ORDER BY C.ORDINAL_POSITION

		OPEN TABLECOLUMNS
		
		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnPrecision, @ColumnScale
		WHILE @@FETCH_STATUS = 0
		BEGIN --> 3
			--IF @ColumnType in ('int') AND @ColumnName = 'id'
			--	SET @ColumnName = @TableName + '_id'
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN --> 4
		
				SELECT @CreateStatement = @CreateStatement + '[' + @ColumnName + '] [' + @ColumnType + '] '
					, @SelectKeys = @SelectKeys + '[' + @ColumnName + '], '
					, @SelectKeyValues = @SelectKeyValues + ', ''[' + @ColumnName + ']'''
					, @SelectKeyJoin = @SelectKeyJoin + 'AND i.[' + @ColumnName + '] = d.[' + @ColumnName + '] '
					, @SelectInsertKeys = @SelectInsertKeys + 'COALESCE(d.[' + @ColumnName + '], i.[' + @ColumnName + ']), '
				
				IF @ColumnType in ('binary', 'char', 'nchar', 'nvarchar', 'varbinary', 'varchar')
				BEGIN --> 5
					IF (@ColumnLength = -1)
						Set @CreateStatement = @CreateStatement + '(max) '	 	
					ELSE
						SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnLength as varchar(10)) + ') '	 	
				END --< 5
		
				IF @ColumnType in ('decimal', 'numeric')
					SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnPrecision as varchar(10)) + ',' + cast(@ColumnScale as varchar(10)) + ') '	 	
		
				IF @ColumnNullable = 'NO'
					SET @CreateStatement = @CreateStatement + 'NOT '	 	
		
				SET @CreateStatement = @CreateStatement + 'NULL ' + @CRLF + ' , '	 	
			END --< 4
			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnPrecision, @ColumnScale
		END --< 3

		CLOSE TableColumns
		DEALLOCATE TableColumns

		IF LEN(@SelectKeyJoin) > 3
			SET @SelectKeyJoin = RIGHT(@SelectKeyJoin, LEN(@SelectKeyJoin) - 3)
		IF LEN(@SelectKeyValues) > 3
			SET @SelectKeyValues = '''' + RIGHT(@SelectKeyValues, LEN(@SelectKeyValues) - 3)
		SET @SelectDelKeys = 'd.' + REPLACE(@SelectKeys,', ',', d.')
		IF LEN(@SelectDelKeys) > 3
			SET @SelectDelKeys = LEFT(@SelectDelKeys,LEN(@SelectDelKeys) - 3)
		SET @SelectFirstKey = LEFT(@SelectKeys, (CHARINDEX(',',@SelectKeys,0)) - 1)
		SET @SelectIndexKeys = REPLACE(@SelectKeys, ',',' ASC,')
		SET @SelectIndexKeys = LEFT(@SelectIndexKeys, LEN(@SelectIndexKeys) - 1)

		SET @sql = ' IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME= ''' + @TableName + @AuditNameExtention + ''') ' + @CRLF
			+ ' BEGIN ' + @CRLF
			+ '		IF (' + CAST(@DropAuditTable AS VARCHAR(1)) + ' = 1) ' + @CRLF
			+ '			 DROP TABLE ' + @TABLE_SCHEMA + '.' + @TableName + @AuditNameExtention + @CRLF
			+ ' END ' + @CRLF
PRINT @sql
PRINT ( @CRLF + @CRLF + ' GO ' + @CRLF + @CRLF)
IF @PrintOnly = 0

		BEGIN TRY --> 2
	EXEC (@sql)
		END TRY --< 2

		BEGIN CATCH --> 2
			EXEC config.usp_InsertErrorDetails
		END CATCH --< 2

		SET @sql = ' IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME= ''' + @TableName + @AuditNameExtention + ''' AND TABLE_SCHEMA = ''' + @TABLE_SCHEMA + ''') ' + @CRLF
			+ ' CREATE TABLE ' + @TABLE_SCHEMA + '.' + @TableName + @AuditNameExtention + @CRLF
			+ ' ( AuditID [int]IDENTITY(1,1) NOT NULL ' + @CRLF
			+ ' , Type char(1) ' + @CRLF
			+ ' , ' + @CreateStatement
			+ ' FieldName varchar(128) ' + @CRLF
			+ ' , OldValue varchar(1000) ' + @CRLF
			+ ' , NewValue varchar(1000) ' + @CRLF
			+ ' , UpdateUtcDate datetime2(3) DEFAULT (GetUTCDate()) ' + @CRLF
			+ ' , UserName varchar(128) ' + @CRLF

			+ ' , CONSTRAINT [PK_' + @TableName + @AuditNameExtention + '] PRIMARY KEY CLUSTERED ' + @CRLF
			+ ' ( ' + @CRLF
			+ '		[AuditID] ASC ' + @CRLF
			+ '		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] ' + @CRLF
			+ ' ) ON [PRIMARY] ' + @CRLF
			+ @CRLF
			+ ' CREATE NONCLUSTERED INDEX [IX_' + @TableName + @AuditNameExtention + '_' + REPLACE(REPLACE(REPLACE(@SelectFirstKey,' ','_'),'[',''),']','') + '] ON ' + @TABLE_SCHEMA + '.[' + @TableName + @AuditNameExtention + '] ' + @CRLF
			+ ' (' + @CRLF
			+ @SelectIndexKeys + @CRLF
			+ ' )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ' + @CRLF
PRINT @sql
PRINT ( @CRLF + @CRLF + ' GO ' + @CRLF + @CRLF)

		BEGIN TRY --> 3
IF @PrintOnly = 0
	EXEC (@sql)
		END TRY --< 3

		BEGIN CATCH --> 3
			EXEC config.usp_InsertErrorDetails
		END CATCH --< 3
		
		SET @sql = 'IF OBJECT_ID (''' + @TABLE_SCHEMA + '.' + @TableName + '_changeLog'', ''TR'') IS NOT NULL DROP TRIGGER ' + @TABLE_SCHEMA + '.' + @TableName + '_changeLog'
PRINT @sql
PRINT ( @CRLF + @CRLF + ' GO ' + @CRLF + @CRLF)

		BEGIN TRY --> 3
IF @PrintOnly = 0
	EXEC (@sql)
		END TRY --< 3

		BEGIN CATCH --> 3
			EXEC config.usp_InsertErrorDetails
		END CATCH --< 3
		SET @sql = 
			' CREATE TRIGGER ' + @TABLE_SCHEMA + '.' + @TableName + '_changeLog ON ' + @TABLE_SCHEMA + '.' + @TableName + ' FOR ' + @TriggerEvents + @CRLF
			+ ' AS ' + @CRLF
			+ '	DECLARE @BIT INT ' + @CRLF
			+ ' , @FIELD INT ' + @CRLF
			+ ' , @MAXFIELD INT ' + @CRLF
			+ ' , @CHAR INT ' + @CRLF
			+ '	, @FIELDNAME VARCHAR(128) ' + @CRLF
			+ ' , @TYPE CHAR(1) ' + @CRLF
			+ ' , @sql varchar(8000) ' + @CRLF
			+ ' IF EXISTS (SELECT * FROM INSERTED) ' + @CRLF
			+ ' IF EXISTS (SELECT * FROM DELETED) ' + @CRLF
			+ ' SELECT @TYPE = ''U'' ' + @CRLF
			+ ' ELSE ' + @CRLF
			+ ' SELECT @TYPE = ''I'' ' + @CRLF
			+ ' ELSE ' + @CRLF
			+ ' SELECT @TYPE = ''D'' ' + @CRLF
			+ ' SELECT * INTO #INS FROM INSERTED ' + @CRLF
			+ ' SELECT * INTO #DEL FROM DELETED ' + @CRLF
			+ ' SELECT @FIELD = 0, @MAXFIELD = MAX(column_id) FROM Sys.COLUMNS WHERE object_id = OBJECT_ID(''' + @TABLE_SCHEMA + '.' + @TableName + ''')' + @CRLF
			+ ' WHILE @FIELD < @MAXFIELD ' + @CRLF
			+ ' BEGIN ' + @CRLF
			+ ' SELECT @FIELD = MIN(column_id) FROM Sys.COLUMNS WHERE object_id = OBJECT_ID(''' + @TABLE_SCHEMA + '.' + @TableName + ''') AND column_id > @FIELD ' + @CRLF
			+ '			AND NAME NOT IN ' + @ExcludedColumnCSV + @CRLF
			--+ '		AND NAME NOT LIKE ''Last%'' ' + @CRLF
			--+ '		AND NAME NOT LIKE ''Modify%'' ' + @CRLF
			+ ' IF (sys.fn_IsBitSetInBitmask(COLUMNS_UPDATED(), @field)) <> 0 OR @TYPE IN (''D'') ' + @CRLF
			+ ' BEGIN ' + @CRLF
			+ ' SELECT @fieldname = name from Sys.COLUMNS WHERE object_id = OBJECT_ID(''' + @TABLE_SCHEMA + '.' + @TableName + ''') and column_id = @field ' + @CRLF
			+ ' SET @SQL = '' INSERT INTO ' + @TABLE_SCHEMA + '.' + @TableName + @AuditNameExtention + ' ''' + @CRLF
			+ '+ '' (Type, ' + @SelectKeys + ' FieldName, OldValue, NewValue, UpdateUtcDate, UserName) ''' + @CRLF
			+ '+ '' SELECT '''''' + @TYPE + '''''', ' + @SelectInsertKeys + ''''''' + @fieldname + '''''', d.['' + @fieldname + ''], i.['' + @fieldname + ''], GETUTCDATE(), COALESCE(i.[last_modify_user], SUSER_NAME()) ''' + @CRLF 
			+ '+ '' FROM #INS i ''' + @CRLF
			+ '+ ''		FULL OUTER JOIN #DEL d ON ' + @SelectKeyJoin + '''' + @CRLF
			+ '+ '' WHERE i.['' + @fieldname + ''] <> d.['' + @fieldname + '']'' ' + @CRLF
			+ '+ ''	 or (i.['' + @fieldname + ''] is null and  d.['' + @fieldname + ''] is not null) ''' + @CRLF 
			+ '+ ''	 or (i.['' + @fieldname + ''] is not null and  d.['' + @fieldname + ''] is null) ''' + @CRLF 
			+ ' EXEC(@SQL) ' + @CRLF
			+ '	 END ' + @CRLF
			+ ' END ' + @CRLF
			+ @CRLF
PRINT @sql
PRINT ( @CRLF + @CRLF + ' GO ' + @CRLF + @CRLF)

		BEGIN TRY --> 3
IF @PrintOnly = 0
	EXEC (@sql)
		END TRY --< 3

		BEGIN CATCH --> 3
			EXEC config.usp_InsertErrorDetails
		END CATCH --< 3
	END --> 2	
--END --< 1


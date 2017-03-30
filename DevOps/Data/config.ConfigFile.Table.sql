USE [DevOps]
GO
DELETE FROM [config].[ConfigFile]
GO
SET IDENTITY_INSERT [config].[ConfigFile] ON 

INSERT [config].[ConfigFile] ([id], [component_id], [file_name], [environment], [xml_declaration], [create_date], [modify_date]) VALUES (0, 0, N'root', N'development', N'', CAST(N'2017-03-30T14:16:00.677' AS DateTime), CAST(N'2017-03-30T14:16:00.677' AS DateTime))
SET IDENTITY_INSERT [config].[ConfigFile] OFF

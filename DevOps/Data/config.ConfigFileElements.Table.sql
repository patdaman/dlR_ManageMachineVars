USE [DevOps]
GO
DELETE FROM [config].[ConfigFileElements]
GO
SET IDENTITY_INSERT [config].[ConfigFileElements] ON 

INSERT [config].[ConfigFileElements] ([id], [configfile_id], [element_name], [parent_element_id], [create_date], [modify_date]) VALUES (0, 0, N'Root', 0, CAST(N'2017-03-30T14:17:08.933' AS DateTime), CAST(N'2017-03-30T14:17:08.933' AS DateTime))
SET IDENTITY_INSERT [config].[ConfigFileElements] OFF

USE [DevOps]
GO
SET IDENTITY_INSERT [config].[EnvironmentVariables] ON 

INSERT [config].[EnvironmentVariables] ([id], [key], [value], [type], [path], [create_date], [modify_date], [active]) VALUES (100000, N'test1', N'marcom', N'user', NULL, CAST(N'2017-02-23T15:57:16.227' AS DateTime), NULL, 1)
INSERT [config].[EnvironmentVariables] ([id], [key], [value], [type], [path], [create_date], [modify_date], [active]) VALUES (100001, N'test2', N'password', N'machine', NULL, CAST(N'2017-02-23T15:57:25.460' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [config].[EnvironmentVariables] OFF

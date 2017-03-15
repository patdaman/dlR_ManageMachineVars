USE [DevOps]
GO
INSERT [config].[Enum_EnvironmentVariableType] ([name], [value], [active]) VALUES (N'machine', N'machine', 1)
INSERT [config].[Enum_EnvironmentVariableType] ([name], [value], [active]) VALUES (N'process', N'process', 1)
INSERT [config].[Enum_EnvironmentVariableType] ([name], [value], [active]) VALUES (N'user', N'user', 1)

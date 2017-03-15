USE [DevOps]
GO
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'development', N'development', 1)
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'dr', N'dr', 1)
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'production', N'production', 1)
INSERT [config].[Enum_EnvironmentType] ([name], [value], [active]) VALUES (N'qa', N'qa', 1)

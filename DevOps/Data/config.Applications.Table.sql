USE [DevOps]
GO
SET IDENTITY_INSERT [config].[Applications] ON 

INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100000, N'app1', N'.42', CAST(N'2017-02-23T09:09:44.067' AS DateTime), NULL, 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100001, N'app2', N'.42', CAST(N'2017-02-23T09:09:52.487' AS DateTime), NULL, 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100002, N'app3', N'.42', CAST(N'2017-02-23T09:10:00.620' AS DateTime), NULL, 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100003, N'Admin', NULL, CAST(N'2017-03-01T15:41:26.057' AS DateTime), NULL, 1)
INSERT [config].[Applications] ([id], [application_name], [release], [create_date], [modify_date], [active]) VALUES (100004, N'ManagerServices', NULL, CAST(N'2017-03-01T15:41:32.763' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [config].[Applications] OFF

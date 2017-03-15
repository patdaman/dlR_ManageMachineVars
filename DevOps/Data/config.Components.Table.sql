USE [DevOps]
GO
SET IDENTITY_INSERT [config].[Components] ON 

INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100000, N'Commerce', N'Commerce\Commerce.config', CAST(N'2017-03-02T12:17:26.673' AS DateTime), NULL, 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100001, N'DAL', N'DAL\DAL.config', CAST(N'2017-03-02T12:18:00.380' AS DateTime), NULL, 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100002, N'ManagerI18N', N'ManagerI18N\App.Config', CAST(N'2017-03-02T12:19:07.447' AS DateTime), NULL, 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100003, N'ManagerI18N', N'ManagerI18N\log4net.config', CAST(N'2017-03-02T12:20:29.953' AS DateTime), NULL, 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100004, N'Services', N'Services\Manager\app.config', CAST(N'2017-03-02T12:21:28.840' AS DateTime), NULL, 1)
INSERT [config].[Components] ([id], [component_name], [relative_path], [create_date], [modify_date], [active]) VALUES (100005, N'Services', N'Services\Manager\log4net.config', CAST(N'2017-03-02T12:21:48.693' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [config].[Components] OFF

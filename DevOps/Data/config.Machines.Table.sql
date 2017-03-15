USE [DevOps]
GO
SET IDENTITY_INSERT [config].[Machines] ON 

INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100000, N'hqdev04', NULL, N'Las Vegas', N'development', CAST(N'2017-02-10T15:49:46.100' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100001, N'hqdev07', NULL, N'Las Vegas', N'development', CAST(N'2017-02-10T15:50:13.743' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100002, N'168LYR1', NULL, N'Solana Beach', N'development', CAST(N'2017-02-22T14:38:56.070' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100003, N'bender', NULL, N'Solana Beach', N'development', CAST(N'2017-02-23T09:18:06.763' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100004, N'lvmgr01', NULL, N'Las Vegas', N'QA', CAST(N'2017-03-01T15:40:51.587' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100005, N'hqdev01', NULL, N'Las Vegas', N'development', CAST(N'2017-03-03T16:18:23.770' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100007, N'eumgr01', NULL, N'Azure', N'production', CAST(N'2017-03-14T16:49:43.297' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100008, N'eumgr02', NULL, N'Azure', N'production', CAST(N'2017-03-14T16:49:53.090' AS DateTime), NULL, 1)
INSERT [config].[Machines] ([id], [machine_name], [ip_address], [location], [usage], [create_date], [modify_date], [active]) VALUES (100009, N'hqdev08', NULL, N'Solana Beach', N'development', CAST(N'2017-03-14T16:50:35.443' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [config].[Machines] OFF

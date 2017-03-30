USE [DevOps]
GO
DELETE FROM [config].[MachineComponentPathMap]
GO
INSERT [config].[MachineComponentPathMap] ([machine_id], [component_id], [config_path]) VALUES (100005, 100000, N'C:\Users\pdelosreyes\Desktop\printableConfig\Commerce\Commerce.config')
INSERT [config].[MachineComponentPathMap] ([machine_id], [component_id], [config_path]) VALUES (100005, 100001, N'C:\Users\pdelosreyes\Desktop\printableConfig\DAL\DAL.config')
INSERT [config].[MachineComponentPathMap] ([machine_id], [component_id], [config_path]) VALUES (100005, 100002, N'C:\Users\pdelosreyes\Desktop\printableConfig\ManagerI18N\App.Config')
INSERT [config].[MachineComponentPathMap] ([machine_id], [component_id], [config_path]) VALUES (100005, 100002, N'C:\Users\pdelosreyes\Desktop\printableConfig\ManagerI18N\log4net.config')
INSERT [config].[MachineComponentPathMap] ([machine_id], [component_id], [config_path]) VALUES (100005, 100004, N'C:\Users\pdelosreyes\Desktop\printableConfig\Services\Manager\app.config')

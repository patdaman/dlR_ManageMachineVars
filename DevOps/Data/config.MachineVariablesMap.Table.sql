USE [DevOps]
GO
DELETE FROM [config].[MachineVariablesMap]
GO
INSERT [config].[MachineVariablesMap] ([environmentVariable_id], [machine_id]) VALUES (100000, 100002)
INSERT [config].[MachineVariablesMap] ([environmentVariable_id], [machine_id]) VALUES (100000, 100003)
INSERT [config].[MachineVariablesMap] ([environmentVariable_id], [machine_id]) VALUES (100001, 100002)
INSERT [config].[MachineVariablesMap] ([environmentVariable_id], [machine_id]) VALUES (100001, 100003)

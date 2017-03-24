USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineVariables_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachineVariables]'))
ALTER TABLE [config].[MachineVariables] DROP CONSTRAINT [FK_MachineVariables_Machines]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineVariables_EnvironmentVariables]') AND parent_object_id = OBJECT_ID(N'[config].[MachineVariables]'))
ALTER TABLE [config].[MachineVariables] DROP CONSTRAINT [FK_MachineVariables_EnvironmentVariables]
GO
/****** Object:  Table [config].[MachineVariables]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[MachineVariables]') AND type in (N'U'))
DROP TABLE [config].[MachineVariables]
GO
/****** Object:  Table [config].[MachineVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[MachineVariables]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[MachineVariables](
	[environmentVariable_id] [int] NOT NULL,
	[machine_id] [int] NOT NULL,
 CONSTRAINT [PK_MachineVariables] PRIMARY KEY CLUSTERED 
(
	[environmentVariable_id] ASC,
	[machine_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [config].[MachineVariables] ([environmentVariable_id], [machine_id]) VALUES (100000, 100002)
INSERT [config].[MachineVariables] ([environmentVariable_id], [machine_id]) VALUES (100000, 100003)
INSERT [config].[MachineVariables] ([environmentVariable_id], [machine_id]) VALUES (100001, 100002)
INSERT [config].[MachineVariables] ([environmentVariable_id], [machine_id]) VALUES (100001, 100003)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineVariables_EnvironmentVariables]') AND parent_object_id = OBJECT_ID(N'[config].[MachineVariables]'))
ALTER TABLE [config].[MachineVariables]  WITH CHECK ADD  CONSTRAINT [FK_MachineVariables_EnvironmentVariables] FOREIGN KEY([environmentVariable_id])
REFERENCES [config].[EnvironmentVariables] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineVariables_EnvironmentVariables]') AND parent_object_id = OBJECT_ID(N'[config].[MachineVariables]'))
ALTER TABLE [config].[MachineVariables] CHECK CONSTRAINT [FK_MachineVariables_EnvironmentVariables]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineVariables_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachineVariables]'))
ALTER TABLE [config].[MachineVariables]  WITH CHECK ADD  CONSTRAINT [FK_MachineVariables_Machines] FOREIGN KEY([machine_id])
REFERENCES [config].[Machines] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineVariables_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachineVariables]'))
ALTER TABLE [config].[MachineVariables] CHECK CONSTRAINT [FK_MachineVariables_Machines]
GO

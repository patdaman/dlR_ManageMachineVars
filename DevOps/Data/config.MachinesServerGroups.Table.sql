USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachinesServerGroups_ServerGroups]') AND parent_object_id = OBJECT_ID(N'[config].[MachinesServerGroups]'))
ALTER TABLE [config].[MachinesServerGroups] DROP CONSTRAINT [FK_MachinesServerGroups_ServerGroups]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachinesServerGroups_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachinesServerGroups]'))
ALTER TABLE [config].[MachinesServerGroups] DROP CONSTRAINT [FK_MachinesServerGroups_Machines]
GO
/****** Object:  Table [config].[MachinesServerGroups]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[MachinesServerGroups]') AND type in (N'U'))
DROP TABLE [config].[MachinesServerGroups]
GO
/****** Object:  Table [config].[MachinesServerGroups]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[MachinesServerGroups]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[MachinesServerGroups](
	[machine_id] [int] NOT NULL,
	[servergroup_id] [int] NOT NULL,
 CONSTRAINT [PK_MachinesServerGroups] PRIMARY KEY CLUSTERED 
(
	[machine_id] ASC,
	[servergroup_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachinesServerGroups_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachinesServerGroups]'))
ALTER TABLE [config].[MachinesServerGroups]  WITH CHECK ADD  CONSTRAINT [FK_MachinesServerGroups_Machines] FOREIGN KEY([machine_id])
REFERENCES [config].[Machines] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachinesServerGroups_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachinesServerGroups]'))
ALTER TABLE [config].[MachinesServerGroups] CHECK CONSTRAINT [FK_MachinesServerGroups_Machines]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachinesServerGroups_ServerGroups]') AND parent_object_id = OBJECT_ID(N'[config].[MachinesServerGroups]'))
ALTER TABLE [config].[MachinesServerGroups]  WITH CHECK ADD  CONSTRAINT [FK_MachinesServerGroups_ServerGroups] FOREIGN KEY([servergroup_id])
REFERENCES [config].[ServerGroups] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachinesServerGroups_ServerGroups]') AND parent_object_id = OBJECT_ID(N'[config].[MachinesServerGroups]'))
ALTER TABLE [config].[MachinesServerGroups] CHECK CONSTRAINT [FK_MachinesServerGroups_ServerGroups]
GO

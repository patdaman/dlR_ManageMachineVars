USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineComponentPath_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachineComponentPath]'))
ALTER TABLE [config].[MachineComponentPath] DROP CONSTRAINT [FK_MachineComponentPath_Machines]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineComponentPath_Components]') AND parent_object_id = OBJECT_ID(N'[config].[MachineComponentPath]'))
ALTER TABLE [config].[MachineComponentPath] DROP CONSTRAINT [FK_MachineComponentPath_Components]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_MachineComponentPath_root_path]') AND type = 'D')
BEGIN
ALTER TABLE [config].[MachineComponentPath] DROP CONSTRAINT [DF_MachineComponentPath_root_path]
END

GO
/****** Object:  Table [config].[MachineComponentPath]    Script Date: 3/23/2017 5:01:25 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[MachineComponentPath]') AND type in (N'U'))
DROP TABLE [config].[MachineComponentPath]
GO
/****** Object:  Table [config].[MachineComponentPath]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[MachineComponentPath]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[MachineComponentPath](
	[machine_id] [int] NOT NULL,
	[component_id] [int] NOT NULL,
	[config_path] [varchar](256) NOT NULL,
 CONSTRAINT [PK_MachineComponentPath] PRIMARY KEY CLUSTERED 
(
	[machine_id] ASC,
	[component_id] ASC,
	[config_path] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [config].[MachineComponentPath] ([machine_id], [component_id], [config_path]) VALUES (100005, 100000, N'C:\Users\pdelosreyes\Desktop\printableConfig\Commerce\Commerce.config')
INSERT [config].[MachineComponentPath] ([machine_id], [component_id], [config_path]) VALUES (100005, 100001, N'C:\Users\pdelosreyes\Desktop\printableConfig\DAL\DAL.config')
INSERT [config].[MachineComponentPath] ([machine_id], [component_id], [config_path]) VALUES (100005, 100002, N'C:\Users\pdelosreyes\Desktop\printableConfig\ManagerI18N\App.Config')
INSERT [config].[MachineComponentPath] ([machine_id], [component_id], [config_path]) VALUES (100005, 100002, N'C:\Users\pdelosreyes\Desktop\printableConfig\ManagerI18N\log4net.config')
INSERT [config].[MachineComponentPath] ([machine_id], [component_id], [config_path]) VALUES (100005, 100004, N'C:\Users\pdelosreyes\Desktop\printableConfig\Services\Manager\app.config')
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[DF_MachineComponentPath_root_path]') AND type = 'D')
BEGIN
ALTER TABLE [config].[MachineComponentPath] ADD  CONSTRAINT [DF_MachineComponentPath_root_path]  DEFAULT ('D:\US\printableConfig\') FOR [config_path]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineComponentPath_Components]') AND parent_object_id = OBJECT_ID(N'[config].[MachineComponentPath]'))
ALTER TABLE [config].[MachineComponentPath]  WITH CHECK ADD  CONSTRAINT [FK_MachineComponentPath_Components] FOREIGN KEY([component_id])
REFERENCES [config].[Components] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineComponentPath_Components]') AND parent_object_id = OBJECT_ID(N'[config].[MachineComponentPath]'))
ALTER TABLE [config].[MachineComponentPath] CHECK CONSTRAINT [FK_MachineComponentPath_Components]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineComponentPath_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachineComponentPath]'))
ALTER TABLE [config].[MachineComponentPath]  WITH CHECK ADD  CONSTRAINT [FK_MachineComponentPath_Machines] FOREIGN KEY([machine_id])
REFERENCES [config].[Machines] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_MachineComponentPath_Machines]') AND parent_object_id = OBJECT_ID(N'[config].[MachineComponentPath]'))
ALTER TABLE [config].[MachineComponentPath] CHECK CONSTRAINT [FK_MachineComponentPath_Machines]
GO

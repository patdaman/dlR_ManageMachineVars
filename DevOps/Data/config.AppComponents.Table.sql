USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_AppComponents_Components]') AND parent_object_id = OBJECT_ID(N'[config].[AppComponents]'))
ALTER TABLE [config].[AppComponents] DROP CONSTRAINT [FK_AppComponents_Components]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_AppComponents_Applications]') AND parent_object_id = OBJECT_ID(N'[config].[AppComponents]'))
ALTER TABLE [config].[AppComponents] DROP CONSTRAINT [FK_AppComponents_Applications]
GO
/****** Object:  Table [config].[AppComponents]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[AppComponents]') AND type in (N'U'))
DROP TABLE [config].[AppComponents]
GO
/****** Object:  Table [config].[AppComponents]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[AppComponents]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[AppComponents](
	[application_id] [int] NOT NULL,
	[component_id] [int] NOT NULL,
 CONSTRAINT [PK_AppComponents] PRIMARY KEY CLUSTERED 
(
	[application_id] ASC,
	[component_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100003, 100000)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100003, 100001)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100003, 100002)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100003, 100003)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100003, 100004)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100003, 100005)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100004, 100000)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100004, 100001)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100004, 100002)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100004, 100003)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100004, 100004)
INSERT [config].[AppComponents] ([application_id], [component_id]) VALUES (100004, 100005)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_AppComponents_Applications]') AND parent_object_id = OBJECT_ID(N'[config].[AppComponents]'))
ALTER TABLE [config].[AppComponents]  WITH CHECK ADD  CONSTRAINT [FK_AppComponents_Applications] FOREIGN KEY([application_id])
REFERENCES [config].[Applications] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_AppComponents_Applications]') AND parent_object_id = OBJECT_ID(N'[config].[AppComponents]'))
ALTER TABLE [config].[AppComponents] CHECK CONSTRAINT [FK_AppComponents_Applications]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_AppComponents_Components]') AND parent_object_id = OBJECT_ID(N'[config].[AppComponents]'))
ALTER TABLE [config].[AppComponents]  WITH CHECK ADD  CONSTRAINT [FK_AppComponents_Components] FOREIGN KEY([component_id])
REFERENCES [config].[Components] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_AppComponents_Components]') AND parent_object_id = OBJECT_ID(N'[config].[AppComponents]'))
ALTER TABLE [config].[AppComponents] CHECK CONSTRAINT [FK_AppComponents_Components]
GO

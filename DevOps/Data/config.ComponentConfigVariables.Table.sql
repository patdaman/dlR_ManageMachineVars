USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ComponentConfigVariables_Components]') AND parent_object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]'))
ALTER TABLE [config].[ComponentConfigVariables] DROP CONSTRAINT [FK_ComponentConfigVariables_Components]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationConfigVariables_ConfigVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]'))
ALTER TABLE [config].[ComponentConfigVariables] DROP CONSTRAINT [FK_ApplicationConfigVariables_ConfigVariables]
GO
/****** Object:  Table [config].[ComponentConfigVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]') AND type in (N'U'))
DROP TABLE [config].[ComponentConfigVariables]
GO
/****** Object:  Table [config].[ComponentConfigVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[ComponentConfigVariables](
	[component_id] [int] NOT NULL,
	[configvariable_id] [int] NOT NULL,
 CONSTRAINT [PK_ComponentConfigVariables] PRIMARY KEY CLUSTERED 
(
	[component_id] ASC,
	[configvariable_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100147)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100148)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100149)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100150)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100151)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100152)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100153)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100154)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100155)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100156)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100000, 100157)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100140)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100141)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100142)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100143)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100144)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100145)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100001, 100146)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100128)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100129)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100130)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100131)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100132)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100133)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100134)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100135)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100136)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100137)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100138)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100002, 100139)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100004, 100125)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100004, 100126)
INSERT [config].[ComponentConfigVariables] ([component_id], [configvariable_id]) VALUES (100004, 100127)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationConfigVariables_ConfigVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]'))
ALTER TABLE [config].[ComponentConfigVariables]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationConfigVariables_ConfigVariables] FOREIGN KEY([configvariable_id])
REFERENCES [config].[ConfigVariables] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationConfigVariables_ConfigVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]'))
ALTER TABLE [config].[ComponentConfigVariables] CHECK CONSTRAINT [FK_ApplicationConfigVariables_ConfigVariables]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ComponentConfigVariables_Components]') AND parent_object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]'))
ALTER TABLE [config].[ComponentConfigVariables]  WITH CHECK ADD  CONSTRAINT [FK_ComponentConfigVariables_Components] FOREIGN KEY([component_id])
REFERENCES [config].[Components] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ComponentConfigVariables_Components]') AND parent_object_id = OBJECT_ID(N'[config].[ComponentConfigVariables]'))
ALTER TABLE [config].[ComponentConfigVariables] CHECK CONSTRAINT [FK_ComponentConfigVariables_Components]
GO

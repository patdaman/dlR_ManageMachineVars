USE [DevOps]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationEnvironmentVariables_EnvironmentVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]'))
ALTER TABLE [config].[ApplicationEnvironmentVariables] DROP CONSTRAINT [FK_ApplicationEnvironmentVariables_EnvironmentVariables]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationEnvironmentVariables_Applications]') AND parent_object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]'))
ALTER TABLE [config].[ApplicationEnvironmentVariables] DROP CONSTRAINT [FK_ApplicationEnvironmentVariables_Applications]
GO
/****** Object:  Table [config].[ApplicationEnvironmentVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]') AND type in (N'U'))
DROP TABLE [config].[ApplicationEnvironmentVariables]
GO
/****** Object:  Table [config].[ApplicationEnvironmentVariables]    Script Date: 3/23/2017 5:01:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]') AND type in (N'U'))
BEGIN
CREATE TABLE [config].[ApplicationEnvironmentVariables](
	[application_id] [int] NOT NULL,
	[environmentvariable_id] [int] NOT NULL,
 CONSTRAINT [PK_ApplicationEnvironmentVariables] PRIMARY KEY CLUSTERED 
(
	[application_id] ASC,
	[environmentvariable_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationEnvironmentVariables_Applications]') AND parent_object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]'))
ALTER TABLE [config].[ApplicationEnvironmentVariables]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationEnvironmentVariables_Applications] FOREIGN KEY([application_id])
REFERENCES [config].[Applications] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationEnvironmentVariables_Applications]') AND parent_object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]'))
ALTER TABLE [config].[ApplicationEnvironmentVariables] CHECK CONSTRAINT [FK_ApplicationEnvironmentVariables_Applications]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationEnvironmentVariables_EnvironmentVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]'))
ALTER TABLE [config].[ApplicationEnvironmentVariables]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationEnvironmentVariables_EnvironmentVariables] FOREIGN KEY([environmentvariable_id])
REFERENCES [config].[EnvironmentVariables] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[config].[FK_ApplicationEnvironmentVariables_EnvironmentVariables]') AND parent_object_id = OBJECT_ID(N'[config].[ApplicationEnvironmentVariables]'))
ALTER TABLE [config].[ApplicationEnvironmentVariables] CHECK CONSTRAINT [FK_ApplicationEnvironmentVariables_EnvironmentVariables]
GO

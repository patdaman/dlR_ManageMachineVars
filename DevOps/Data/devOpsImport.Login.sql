USE [master]
GO
/****** Object:  Login [devOpsImport]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'devOpsImport')
DROP LOGIN [devOpsImport]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [devOpsImport]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'devOpsImport')
CREATE LOGIN [devOpsImport] WITH PASSWORD=N'7ugocMvObU8MZbdrKj2TvnYLggiQYUFsUCdezcqDRDI=', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [devOpsImport] DISABLE
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [devOpsImport]
GO

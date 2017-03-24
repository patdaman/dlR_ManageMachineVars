USE [master]
GO
/****** Object:  Login [DEV\root]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DEV\root')
DROP LOGIN [DEV\root]
GO
/****** Object:  Login [DEV\root]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DEV\root')
CREATE LOGIN [DEV\root] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO
ALTER SERVER ROLE [sysadmin] ADD MEMBER [DEV\root]
GO

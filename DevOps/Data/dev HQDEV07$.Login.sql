USE [master]
GO
/****** Object:  Login [dev\HQDEV07$]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'dev\HQDEV07$')
DROP LOGIN [dev\HQDEV07$]
GO
/****** Object:  Login [dev\HQDEV07$]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'dev\HQDEV07$')
CREATE LOGIN [dev\HQDEV07$] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english]
GO

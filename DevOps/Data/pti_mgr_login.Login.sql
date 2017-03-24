USE [master]
GO
/****** Object:  Login [pti_mgr_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_mgr_login')
DROP LOGIN [pti_mgr_login]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_mgr_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_mgr_login')
CREATE LOGIN [pti_mgr_login] WITH PASSWORD=N'TTUM2wUaSD38orHtzLJPsriZgruMoa8NNN/C9juAEiI=', DEFAULT_DATABASE=[pt], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_mgr_login] DISABLE
GO

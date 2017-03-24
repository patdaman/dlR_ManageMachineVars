USE [master]
GO
/****** Object:  Login [pti_app_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app_login')
DROP LOGIN [pti_app_login]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_app_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app_login')
CREATE LOGIN [pti_app_login] WITH PASSWORD=N'01taf9JDJWNpE9dL89of3BepM58t/GvSQwdP22OEt0s=', DEFAULT_DATABASE=[ImageServer], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_app_login] DISABLE
GO

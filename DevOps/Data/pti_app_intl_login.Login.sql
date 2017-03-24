USE [master]
GO
/****** Object:  Login [pti_app_intl_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app_intl_login')
DROP LOGIN [pti_app_intl_login]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_app_intl_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app_intl_login')
CREATE LOGIN [pti_app_intl_login] WITH PASSWORD=N'zzsPMfCkFYCH1zCmqLWhJRz56KyV1uRBEvwFYo90/rc=', DEFAULT_DATABASE=[pt_intl], DEFAULT_LANGUAGE=[British], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_app_intl_login] DISABLE
GO

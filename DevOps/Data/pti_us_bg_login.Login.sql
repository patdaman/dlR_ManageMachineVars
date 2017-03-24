USE [master]
GO
/****** Object:  Login [pti_us_bg_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_us_bg_login')
DROP LOGIN [pti_us_bg_login]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_us_bg_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_us_bg_login')
CREATE LOGIN [pti_us_bg_login] WITH PASSWORD=N'Uc0jOr9DmDLkcLBrVpc7bZwBR4EMCglsvVOdg0XXiEY=', DEFAULT_DATABASE=[bGateway], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_us_bg_login] DISABLE
GO

USE [master]
GO
/****** Object:  Login [pti_app]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app')
DROP LOGIN [pti_app]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_app]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app')
CREATE LOGIN [pti_app] WITH PASSWORD=N'3N881MaPN4nS+9ka0gLJWPI/8yQZ8MDfij7gpIXh1Lo=', DEFAULT_DATABASE=[pt], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_app] DISABLE
GO

USE [master]
GO
/****** Object:  Login [session14]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'session14')
DROP LOGIN [session14]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [session14]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'session14')
CREATE LOGIN [session14] WITH PASSWORD=N'KGvE3dc0HnG/d4aRSekWxpi3mQXDPSiSirDzCwyrXn4=', DEFAULT_DATABASE=[ASPStateInMemory], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [session14] DISABLE
GO

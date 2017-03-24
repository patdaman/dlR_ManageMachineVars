USE [master]
GO
/****** Object:  Login [session]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'session')
DROP LOGIN [session]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [session]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'session')
CREATE LOGIN [session] WITH PASSWORD=N'GlN9fAqIJ6oazHnXQtaIrjBlJovcPtotIqc2kdXLuD0=', DEFAULT_DATABASE=[ASPState], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [session] DISABLE
GO

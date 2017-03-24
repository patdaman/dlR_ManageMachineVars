USE [master]
GO
/****** Object:  Login [PRINTABLE]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'PRINTABLE')
DROP LOGIN [PRINTABLE]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [PRINTABLE]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'PRINTABLE')
CREATE LOGIN [PRINTABLE] WITH PASSWORD=N'hs1mb7+mXVTi3IQO4AjHyq4r33CWZo7tokndVxGvNC8=', DEFAULT_DATABASE=[ignite_printable], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [PRINTABLE] DISABLE
GO

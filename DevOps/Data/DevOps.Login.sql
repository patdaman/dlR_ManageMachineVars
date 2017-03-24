USE [master]
GO
/****** Object:  Login [DevOps]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DevOps')
DROP LOGIN [DevOps]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [DevOps]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'DevOps')
CREATE LOGIN [DevOps] WITH PASSWORD=N'MejT95M3h5Vf+z7gzJlwqulpn2m5ZNplWUTmDLyglZs=', DEFAULT_DATABASE=[DevOps], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [DevOps] DISABLE
GO

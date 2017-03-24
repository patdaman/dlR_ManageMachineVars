USE [master]
GO
/****** Object:  Login [ignite]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ignite')
DROP LOGIN [ignite]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [ignite]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ignite')
CREATE LOGIN [ignite] WITH PASSWORD=N'6rrsELDjPU2B9IPsZV3ca9nUyQlsq4rMU5tq7RKmkuM=', DEFAULT_DATABASE=[ignite_repository], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
ALTER LOGIN [ignite] DISABLE
GO

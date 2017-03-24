USE [master]
GO
/****** Object:  Login [ui_writer]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ui_writer')
DROP LOGIN [ui_writer]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [ui_writer]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'ui_writer')
CREATE LOGIN [ui_writer] WITH PASSWORD=N'6wKgvvCKXVFPTCeOeFkMK+VP5ezq0FjWtbFM5gJwsOg=', DEFAULT_DATABASE=[UIPublishing], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [ui_writer] DISABLE
GO

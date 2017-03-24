USE [master]
GO
/****** Object:  Login [vc_user]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'vc_user')
DROP LOGIN [vc_user]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [vc_user]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'vc_user')
CREATE LOGIN [vc_user] WITH PASSWORD=N'ZYr4cOglz9juhGfYSL4ZnMwOdK/sjbio5yxMDh7AVmA=', DEFAULT_DATABASE=[VersionControlDB], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [vc_user] DISABLE
GO

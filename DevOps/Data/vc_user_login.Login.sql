USE [master]
GO
/****** Object:  Login [vc_user_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'vc_user_login')
DROP LOGIN [vc_user_login]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [vc_user_login]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'vc_user_login')
CREATE LOGIN [vc_user_login] WITH PASSWORD=N'j7I8v5Pjl+PEA/R90EQszSqtXFqsSxSeHVzJKyT/RvI=', DEFAULT_DATABASE=[VersionControlDB], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [vc_user_login] DISABLE
GO

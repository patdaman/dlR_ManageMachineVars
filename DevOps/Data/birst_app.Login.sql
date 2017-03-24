USE [master]
GO
/****** Object:  Login [birst_app]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'birst_app')
DROP LOGIN [birst_app]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [birst_app]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'birst_app')
CREATE LOGIN [birst_app] WITH PASSWORD=N'5cZmVrJg3dDZykSp4chV5o1xtpzDRaA5U2LQvBw/wMY=', DEFAULT_DATABASE=[pt], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [birst_app] DISABLE
GO

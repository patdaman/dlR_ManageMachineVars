USE [master]
GO
/****** Object:  Login [pti_app_intl]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app_intl')
DROP LOGIN [pti_app_intl]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_app_intl]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_app_intl')
CREATE LOGIN [pti_app_intl] WITH PASSWORD=N'gbYY/Dh2SqEz7j8dfbMZu98s0oCvrqvTn7Cxmzb59no=', DEFAULT_DATABASE=[pt_intl], DEFAULT_LANGUAGE=[British], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_app_intl] DISABLE
GO

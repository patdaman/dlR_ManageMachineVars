USE [master]
GO
/****** Object:  Login [pti_mgr]    Script Date: 3/23/2017 5:01:26 PM ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_mgr')
DROP LOGIN [pti_mgr]
GO
/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [pti_mgr]    Script Date: 3/23/2017 5:01:26 PM ******/
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'pti_mgr')
CREATE LOGIN [pti_mgr] WITH PASSWORD=N'+lVNiFRBSIEJJ3Pouj6C4D9eH9siBNNiZLA/FfMly0c=', DEFAULT_DATABASE=[pt], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO
ALTER LOGIN [pti_mgr] DISABLE
GO

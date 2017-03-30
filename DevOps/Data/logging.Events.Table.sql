USE [DevOps]
GO
DELETE FROM [logging].[Events]
GO
SET IDENTITY_INSERT [logging].[Events] ON 

INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100000, 100000, N'hqdev04', CAST(N'2017-03-14' AS Date), CAST(N'16:55:25.1000000' AS Time), N'high', N'test log message')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100001, 100001, N'hqdev07', CAST(N'2017-03-14' AS Date), CAST(N'16:55:41.2230000' AS Time), N'normal', N'test log message')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100002, 100002, N'hqdev08', CAST(N'2017-03-14' AS Date), CAST(N'16:56:05.4770000' AS Time), N'normal', N'another test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100003, 100003, N'168LYR1', CAST(N'2017-03-14' AS Date), CAST(N'16:56:22.1930000' AS Time), N'high', N'test Local')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100004, 100008, N'bender', CAST(N'2016-03-14' AS Date), CAST(N'16:57:16.2730000' AS Time), N'low', N'back in time test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100005, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'16:57:25.3070000' AS Time), N'low', N'current test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100006, 100001, N'hqdev07', CAST(N'2016-12-04' AS Date), CAST(N'16:57:44.6400000' AS Time), N'high', N'old tester')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100007, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7300000' AS Time), N'high', N'0. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100008, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7330000' AS Time), N'high', N'1. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100009, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7400000' AS Time), N'high', N'2. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100010, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7500000' AS Time), N'high', N'3. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100011, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7570000' AS Time), N'high', N'4. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100012, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7700000' AS Time), N'high', N'5. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100013, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7800000' AS Time), N'high', N'6. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100014, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.7900000' AS Time), N'high', N'7. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100015, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8070000' AS Time), N'high', N'8. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100016, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8130000' AS Time), N'high', N'9. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100017, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8230000' AS Time), N'high', N'10. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100018, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8230000' AS Time), N'high', N'11. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100019, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8330000' AS Time), N'high', N'12. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100020, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8400000' AS Time), N'high', N'13. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100021, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8570000' AS Time), N'high', N'14. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100022, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8630000' AS Time), N'high', N'15. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100023, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8730000' AS Time), N'high', N'16. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100024, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8730000' AS Time), N'high', N'17. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100025, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8830000' AS Time), N'high', N'18. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100026, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.8970000' AS Time), N'high', N'19. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100027, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9000000' AS Time), N'high', N'20. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100028, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9070000' AS Time), N'high', N'21. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100029, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9170000' AS Time), N'high', N'22. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100030, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9170000' AS Time), N'high', N'23. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100031, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9270000' AS Time), N'high', N'24. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100032, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9300000' AS Time), N'high', N'25. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100033, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9370000' AS Time), N'high', N'26. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100034, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9430000' AS Time), N'high', N'27. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100035, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9570000' AS Time), N'high', N'28. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100036, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9770000' AS Time), N'high', N'29. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100037, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9870000' AS Time), N'high', N'30. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100038, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9870000' AS Time), N'high', N'31. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100039, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:16.9900000' AS Time), N'high', N'32. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100040, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17' AS Time), N'high', N'33. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100041, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17' AS Time), N'high', N'34. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100042, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0030000' AS Time), N'high', N'35. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100043, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0100000' AS Time), N'high', N'36. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100044, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0130000' AS Time), N'high', N'37. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100045, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0230000' AS Time), N'high', N'38. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100046, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0300000' AS Time), N'high', N'39. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100047, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0400000' AS Time), N'high', N'40. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100048, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0470000' AS Time), N'high', N'41. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100049, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0530000' AS Time), N'high', N'42. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100050, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0630000' AS Time), N'high', N'43. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100051, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0800000' AS Time), N'high', N'44. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100052, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0870000' AS Time), N'high', N'45. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100053, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.0970000' AS Time), N'high', N'46. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100054, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1030000' AS Time), N'high', N'47. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100055, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1130000' AS Time), N'high', N'48. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100056, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1200000' AS Time), N'high', N'49. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100057, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1270000' AS Time), N'high', N'50. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100058, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1330000' AS Time), N'high', N'51. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100059, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1430000' AS Time), N'high', N'52. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100060, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1500000' AS Time), N'high', N'53. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100061, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1600000' AS Time), N'high', N'54. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100062, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1670000' AS Time), N'high', N'55. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100063, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1730000' AS Time), N'high', N'56. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100064, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1800000' AS Time), N'high', N'57. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100065, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1830000' AS Time), N'high', N'58. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100066, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.1930000' AS Time), N'high', N'59. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100067, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2000000' AS Time), N'high', N'60. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100068, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2100000' AS Time), N'high', N'61. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100069, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2200000' AS Time), N'high', N'62. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100070, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2300000' AS Time), N'high', N'63. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100071, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2370000' AS Time), N'high', N'64. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100072, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2470000' AS Time), N'high', N'65. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100073, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2530000' AS Time), N'high', N'66. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100074, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2600000' AS Time), N'high', N'67. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100075, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2700000' AS Time), N'high', N'68. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100076, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2730000' AS Time), N'high', N'69. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100077, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2800000' AS Time), N'high', N'70. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100078, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.2900000' AS Time), N'high', N'71. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100079, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3000000' AS Time), N'high', N'72. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100080, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3070000' AS Time), N'high', N'73. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100081, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3170000' AS Time), N'high', N'74. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100082, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3230000' AS Time), N'high', N'75. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100083, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3300000' AS Time), N'high', N'76. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100084, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3430000' AS Time), N'high', N'77. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100085, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3500000' AS Time), N'high', N'78. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100086, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3600000' AS Time), N'high', N'79. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100087, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3630000' AS Time), N'high', N'80. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100088, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3700000' AS Time), N'high', N'81. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100089, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3730000' AS Time), N'high', N'82. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100090, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3800000' AS Time), N'high', N'83. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100091, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3900000' AS Time), N'high', N'84. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100092, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.3970000' AS Time), N'high', N'85. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100093, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4070000' AS Time), N'high', N'86. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100094, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4130000' AS Time), N'high', N'87. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100095, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4200000' AS Time), N'high', N'88. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100096, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4230000' AS Time), N'high', N'89. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100097, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4330000' AS Time), N'high', N'90. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100098, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4400000' AS Time), N'high', N'91. This is a recursive test')
GO
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100099, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4500000' AS Time), N'high', N'92. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100100, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4570000' AS Time), N'high', N'93. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100101, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4630000' AS Time), N'high', N'94. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100102, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4730000' AS Time), N'high', N'95. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100103, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4800000' AS Time), N'high', N'96. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100104, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4900000' AS Time), N'high', N'97. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100105, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4900000' AS Time), N'high', N'98. This is a recursive test')
INSERT [logging].[Events] ([id], [device_id], [Hostname], [Date], [Time], [Priority], [Message]) VALUES (100106, 100008, N'bender', CAST(N'2017-03-14' AS Date), CAST(N'17:03:17.4970000' AS Time), N'high', N'99. This is a recursive test')
SET IDENTITY_INSERT [logging].[Events] OFF

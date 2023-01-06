SET IDENTITY_INSERT [dbo].[Buyers] ON
INSERT INTO [dbo].[Buyers] ([Id], [familyName], [firstName], [patronymic], [phone], [email]) VALUES (1, N'n1        ', N'f1        ', N'p1        ', N'123       ', N'e1@mail.ru')
INSERT INTO [dbo].[Buyers] ([Id], [familyName], [firstName], [patronymic], [phone], [email]) VALUES (2, N'n2        ', N'f2        ', N'p2        ', N'258       ', N'e2@mail.ru')
SET IDENTITY_INSERT [dbo].[Buyers] OFF

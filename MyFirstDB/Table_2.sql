﻿CREATE TABLE [dbo].[Goods]
(
	[Id] INT PRIMARY KEY IDENTITY(1, 1) NOT NULL, 
    [goodName] NCHAR(10) NULL, 
    [weight] INT NULL, 
    [price] INT NULL 

)

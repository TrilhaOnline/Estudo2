USE [DCBASE]
GO

/****** Object:  StoredProcedure [dbo].[SP_DC_INSERT_USER]    Script Date: 01/03/2021 22:05:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_DC_INSERT_USER]
	@email as varchar(100),
	@guid as varchar(50),
	@password as varchar(50),
	@hashKeyUser as varchar(50),
	@userName as varchar(50),
	@proccessDate as datetime
AS
BEGIN
  INSERT INTO [dbo].[TB_DC_USER]
           ([email]
           ,[guid]
           ,[password]
           ,[hashKeyUser]
           ,[userName]
		   ,[proccessDate])
     VALUES
           (@email,
            @guid,
            @password,
            @hashKeyUser,
            @userName,
		    @proccessDate)
END





GO



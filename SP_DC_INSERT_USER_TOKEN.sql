USE [DCBASE]
GO

/****** Object:  StoredProcedure [dbo].[SP_DC_INSERT_USER_TOKEN]    Script Date: 01/03/2021 22:05:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SP_DC_INSERT_USER_TOKEN]
	@email as varchar(100),
	@token as varchar(1000),
	@processDate as datetime,
	@nameSystem as varchar(50),
	@expires as varchar(50)
AS
BEGIN
  INSERT INTO [dbo].[TB_DC_USER_TOKEN]
           ([email]
           ,[token]
           ,[processDate]
           ,[nameSystem]
           ,[expires])
     VALUES
           (@email,
            @token,
            @processDate,
            @nameSystem,
            @expires)
END






GO



USE [DCBASE]
GO

/****** Object:  StoredProcedure [dbo].[SP_DC_GET_USER_BY_EMAIL]    Script Date: 01/03/2021 22:06:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SP_DC_GET_USER_BY_EMAIL]
	@email varchar(100)
AS
BEGIN
  SELECT [idUser]
      ,[email]
      ,[guid]
      ,[password]
      ,[hashKeyUser]
      ,[userName]
      ,[proccessDate]
  FROM [DCBASE].[dbo].[TB_DC_USER]
  WHERE [email] = @email
END



GO



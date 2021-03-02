USE [DCBASE]
GO

/****** Object:  StoredProcedure [dbo].[SP_DC_GET_USER_TOKEN]    Script Date: 01/03/2021 22:06:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SP_DC_GET_USER_TOKEN]
	@nameSystem varchar(50),
	@email varchar(100)
AS
BEGIN
  SELECT [email]
      ,[token]
      ,[nameSystem]
  FROM [DCBASE].[dbo].[TB_DC_USER_TOKEN]
  where nameSystem = @nameSystem and email = @email
END




GO



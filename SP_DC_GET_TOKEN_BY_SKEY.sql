USE [DCBASE]
GO

/****** Object:  StoredProcedure [dbo].[SP_DC_GET_TOKEN_BY_SKEY]    Script Date: 01/03/2021 22:07:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[SP_DC_GET_TOKEN_BY_SKEY]
	@token varchar(1000),
	@nameSystem varchar(50)
AS
BEGIN
  SELECT [email]
  FROM [DCBASE].[dbo].[TB_DC_USER_TOKEN]
  WHERE [token] = @token and [nameSystem] = @nameSystem
END




GO



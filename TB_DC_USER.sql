USE [DCBASE]
GO

/****** Object:  Table [dbo].[TB_DC_USER]    Script Date: 01/03/2021 22:04:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_DC_USER](
	[idUser] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](100) NULL,
	[guid] [varchar](50) NULL,
	[password] [varchar](50) NULL,
	[hashKeyUser] [varchar](50) NULL,
	[userName] [varchar](50) NULL,
	[proccessDate] [datetime] NULL,
 CONSTRAINT [PK_TB_DC_USER] PRIMARY KEY CLUSTERED 
(
	[idUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



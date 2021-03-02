USE [DCBASE]
GO

/****** Object:  Table [dbo].[TB_DC_USER_TOKEN]    Script Date: 01/03/2021 22:05:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_DC_USER_TOKEN](
	[email] [varchar](100) NOT NULL,
	[token] [varchar](1000) NULL,
	[processDate] [datetime] NULL,
	[nameSystem] [varchar](50) NULL,
	[expires] [varchar](50) NULL,
 CONSTRAINT [PK_TB_DC_USER_TOKEN] PRIMARY KEY CLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



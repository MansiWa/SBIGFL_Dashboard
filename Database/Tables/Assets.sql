USE [sbigeneral]
GO

/****** Object:  Table [dbo].[tbl_BorroAndRFR]    Script Date: 11-06-2024 12:16:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
drop table tbl_ALUpload
CREATE TABLE [dbo].[tbl_ALUpload](
	[al_id] [uniqueidentifier] NOT NULL primary key ,
	[al_docid] [varchar](250) NULL,
	[al_date] [datetime] NULL,
	[al_filename] [varchar](250) NULL,
	[al_count] [varchar](50) NULL,
	[al_fileid] [varchar](250) NULL,
	[al_isactive] [char](1) NOT NULL,
	[al_createdby] [varchar](250) NULL,
	[al_createddate] [datetime] NOT NULL,
	[al_updateddate] [datetime] NOT NULL,
	[batchid] [varchar](100) NULL
)

CREATE TABLE [dbo].[tbl_AssetsLiabilities](
	[al_id] [uniqueidentifier] NOT NULL primary key,
	[al_particulars] [varchar](250) NULL,
	[al_date] [datetime] NULL,
	[al_col1] [varchar](250) NULL,
	[al_col2] [varchar](250) NULL,
	[al_col3] [varchar](250) NULL,
	[al_col4] [varchar](250) NULL,
	[al_isactive] [char](1) NOT NULL,
	[al_createdby] [varchar](250) NULL,
	[al_createddate] [datetime] NOT NULL,
	[al_updateddate] [datetime] NOT NULL,
	[batchid] [varchar](100) NULL
)


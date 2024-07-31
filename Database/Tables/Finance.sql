USE [sbigeneral]
GO

ALTER TABLE [dbo].[tbl_acc_highlights] DROP CONSTRAINT [DF__tbl_acc_h__h_cre__3E1D39E1]
GO

ALTER TABLE [dbo].[tbl_acc_highlights] DROP CONSTRAINT [DF__tbl_acc_h__h_isa__3D2915A8]
GO

ALTER TABLE [dbo].[tbl_acc_highlights] DROP CONSTRAINT [DF__tbl_acc_hi__h_id__3C34F16F]
GO

/****** Object:  Table [dbo].[tbl_acc_highlights]    Script Date: 31-07-2024 14:09:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_acc_highlights]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_acc_highlights]
GO

/****** Object:  Table [dbo].[tbl_acc_highlights]    Script Date: 31-07-2024 14:09:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_acc_highlights](
	[h_id] [uniqueidentifier] NULL,
	[h_date] [date] NULL,
	[h_particulars] [varchar](max) NULL,
	[h_col1] [varchar](max) NULL,
	[h_col2] [varchar](max) NULL,
	[h_col3] [varchar](max) NULL,
	[h_col4] [varchar](max) NULL,
	[h_col5] [varchar](max) NULL,
	[h_col6] [varchar](max) NULL,
	[h_col7] [varchar](max) NULL,
	[h_col8] [varchar](max) NULL,
	[h_col9] [varchar](max) NULL,
	[h_col10] [varchar](max) NULL,
	[h_col11] [varchar](max) NULL,
	[h_col12] [varchar](max) NULL,
	[h_col13] [varchar](max) NULL,
	[h_col14] [varchar](max) NULL,
	[h_creadtedby] [varchar](36) NULL,
	[h_isactive] [char](1) NULL,
	[h_creadteddate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_acc_highlights] ADD  DEFAULT (newid()) FOR [h_id]
GO

ALTER TABLE [dbo].[tbl_acc_highlights] ADD  DEFAULT ((1)) FOR [h_isactive]
GO

ALTER TABLE [dbo].[tbl_acc_highlights] ADD  DEFAULT (getdate()) FOR [h_creadteddate]
GO


USE [sbigeneral]
GO

ALTER TABLE [dbo].[tbl_mourevised] DROP CONSTRAINT [DF__tbl_moure__m_cre__42E1EEFE]
GO

ALTER TABLE [dbo].[tbl_mourevised] DROP CONSTRAINT [DF__tbl_moure__m_isa__41EDCAC5]
GO

ALTER TABLE [dbo].[tbl_mourevised] DROP CONSTRAINT [DF__tbl_mourev__m_id__40F9A68C]
GO

/****** Object:  Table [dbo].[tbl_mourevised]    Script Date: 31-07-2024 14:16:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_mourevised]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_mourevised]
GO

/****** Object:  Table [dbo].[tbl_mourevised]    Script Date: 31-07-2024 14:16:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_mourevised](
	[m_id] [uniqueidentifier] NULL,
	[m_date] [date] NULL,
	[m_particulars] [varchar](max) NULL,
	[m_col1] [varchar](max) NULL,
	[m_col2] [varchar](max) NULL,
	[m_col3] [varchar](max) NULL,
	[m_col4] [varchar](max) NULL,
	[m_col5] [varchar](max) NULL,
	[m_col6] [varchar](max) NULL,
	[m_col7] [varchar](max) NULL,
	[m_col8] [varchar](max) NULL,
	[m_col9] [varchar](max) NULL,
	[m_col10] [varchar](max) NULL,
	[m_col11] [varchar](max) NULL,
	[m_col12] [varchar](max) NULL,
	[m_col13] [varchar](max) NULL,
	[m_col14] [varchar](max) NULL,
	[m_creadtedby] [varchar](36) NULL,
	[m_isactive] [char](1) NULL,
	[m_creadteddate] [datetime] NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_mourevised] ADD  DEFAULT (newid()) FOR [m_id]
GO

ALTER TABLE [dbo].[tbl_mourevised] ADD  DEFAULT ((1)) FOR [m_isactive]
GO

ALTER TABLE [dbo].[tbl_mourevised] ADD  DEFAULT (getdate()) FOR [m_creadteddate]
GO


USE [sbigeneral]
GO

ALTER TABLE [dbo].[tbl_effratio] DROP CONSTRAINT [DF__tbl_effra__e_cre__4A8310C6]
GO

ALTER TABLE [dbo].[tbl_effratio] DROP CONSTRAINT [DF__tbl_effra__e_isa__498EEC8D]
GO

ALTER TABLE [dbo].[tbl_effratio] DROP CONSTRAINT [DF__tbl_effrat__e_id__489AC854]
GO

/****** Object:  Table [dbo].[tbl_effratio]    Script Date: 31-07-2024 14:14:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_effratio]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_effratio]
GO

/****** Object:  Table [dbo].[tbl_effratio]    Script Date: 31-07-2024 14:14:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_effratio](
	[e_id] [uniqueidentifier] NULL,
	[e_date] [date] NULL,
	[e_particulars] [varchar](max) NULL,
	[e_col1] [varchar](max) NULL,
	[e_col2] [varchar](max) NULL,
	[e_col3] [varchar](max) NULL,
	[e_col4] [varchar](max) NULL,
	[e_col5] [varchar](max) NULL,
	[e_col6] [varchar](max) NULL,
	[e_col7] [varchar](max) NULL,
	[e_col8] [varchar](max) NULL,
	[e_col9] [varchar](max) NULL,
	[e_col10] [varchar](max) NULL,
	[e_col11] [varchar](max) NULL,
	[e_col12] [varchar](max) NULL,
	[e_col13] [varchar](max) NULL,
	[e_col14] [varchar](max) NULL,
	[e_creadtedby] [varchar](36) NULL,
	[e_isactive] [char](1) NULL,
	[e_creadteddate] [datetime] NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_effratio] ADD  DEFAULT (newid()) FOR [e_id]
GO

ALTER TABLE [dbo].[tbl_effratio] ADD  DEFAULT ((1)) FOR [e_isactive]
GO

ALTER TABLE [dbo].[tbl_effratio] ADD  DEFAULT (getdate()) FOR [e_creadteddate]
GO


USE [sbigeneral]
GO



/****** Object:  Table [dbo].[tbl_acc_yeild]    Script Date: 31-07-2024 14:12:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_acc_yeild]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_acc_yeild]
GO

/****** Object:  Table [dbo].[tbl_acc_yeild]    Script Date: 31-07-2024 14:12:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_acc_yeild](
	[y_id] [uniqueidentifier] NULL,
	[y_date] [date] NULL,
	[y_particulars] [varchar](max) NULL,
	[y_col1] [varchar](max) NULL,
	[y_col2] [varchar](max) NULL,
	[y_col3] [varchar](max) NULL,
	[y_col4] [varchar](max) NULL,
	[y_col5] [varchar](max) NULL,
	[y_col6] [varchar](max) NULL,
	[y_col7] [varchar](max) NULL,
	[y_col8] [varchar](max) NULL,
	[y_col9] [varchar](max) NULL,
	[y_col10] [varchar](max) NULL,
	[y_col11] [varchar](max) NULL,
	[y_col12] [varchar](max) NULL,
	[y_col13] [varchar](max) NULL,
	[y_col14] [varchar](max) NULL,
	[y_creadtedby] [varchar](36) NULL,
	[y_isactive] [char](1) NULL,
	[y_creadteddate] [datetime] NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_acc_yeild] ADD  DEFAULT (newid()) FOR [y_id]
GO

ALTER TABLE [dbo].[tbl_acc_yeild] ADD  DEFAULT ((1)) FOR [y_isactive]
GO

ALTER TABLE [dbo].[tbl_acc_yeild] ADD  DEFAULT (getdate()) FOR [y_creadteddate]
GO


USE [sbigeneral]
GO

ALTER TABLE [dbo].[tbl_finhighlights] DROP CONSTRAINT [DF__tbl_finhi__f_cre__5224328E]
GO

ALTER TABLE [dbo].[tbl_finhighlights] DROP CONSTRAINT [DF__tbl_finhi__f_isa__51300E55]
GO

ALTER TABLE [dbo].[tbl_finhighlights] DROP CONSTRAINT [DF__tbl_finhig__f_id__503BEA1C]
GO

/****** Object:  Table [dbo].[tbl_finhighlights]    Script Date: 31-07-2024 14:10:46 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbl_finhighlights]') AND type in (N'U'))
DROP TABLE [dbo].[tbl_finhighlights]
GO

/****** Object:  Table [dbo].[tbl_finhighlights]    Script Date: 31-07-2024 14:10:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_finhighlights](
	[f_id] [uniqueidentifier] NULL,
	[f_date] [date] NULL,
	[f_particulars] [varchar](max) NULL,
	[f_col1] [varchar](max) NULL,
	[f_col2] [varchar](max) NULL,
	[f_col3] [varchar](max) NULL,
	[f_col4] [varchar](max) NULL,
	[f_col5] [varchar](max) NULL,
	[f_col6] [varchar](max) NULL,
	[f_col7] [varchar](max) NULL,
	[f_col8] [varchar](max) NULL,
	[f_col9] [varchar](max) NULL,
	[f_col10] [varchar](max) NULL,
	[f_col11] [varchar](max) NULL,
	[f_col12] [varchar](max) NULL,
	[f_col13] [varchar](max) NULL,
	[f_col14] [varchar](max) NULL,
	[f_creadtedby] [varchar](36) NULL,
	[f_isactive] [char](1) NULL,
	[f_creadteddate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbl_finhighlights] ADD  DEFAULT (newid()) FOR [f_id]
GO

ALTER TABLE [dbo].[tbl_finhighlights] ADD  DEFAULT ((1)) FOR [f_isactive]
GO

ALTER TABLE [dbo].[tbl_finhighlights] ADD  DEFAULT (getdate()) FOR [f_creadteddate]
GO



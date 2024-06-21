USE [sbigeneral]
GO
/****** Object:  Table [dbo].[tbl_BDBrQuRl]    Script Date: 21-05-2024 16:04:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BDBrQuRl](
	[bd_date] [date] NULL,
	[Branch] [varchar](200) NULL,
	[NameOfClient] [varchar](200) NULL,
	[NewExist] [varchar](200) NULL,
	[Status] [varchar](200) NULL,
	[Facility] [varchar](200) NULL,
	[FLS] [varchar](200) NULL,
	[Sub_date] [date] NULL,
	[CreditAnalyst] [varchar](200) NULL,
	[CreditPriority] [varchar](200) NULL,
	[ExpDis] [varchar](200) NULL,
	[Remarks] [varchar](200) NULL,
	[bd_isactive] [char](1) NULL,
	[bd_creadteddate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BDSanctioned]    Script Date: 21-05-2024 16:04:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BDSanctioned](
	[bd_date] [date] NULL,
	[Branch] [varchar](200) NULL,
	[NameOfClient] [varchar](200) NULL,
	[NewExist] [varchar](200) NULL,
	[Status] [varchar](200) NULL,
	[Facility] [varchar](200) NULL,
	[FLS] [varchar](200) NULL,
	[Sub_date] [date] NULL,
	[CreditAnalyst] [varchar](200) NULL,
	[CreditPriority] [varchar](200) NULL,
	[SanDate] [date] NULL,
	[ExpDis] [varchar](200) NULL,
	[Remarks] [varchar](200) NULL,
	[bd_isactive] [char](1) NULL,
	[bd_creadteddate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BDSubToCr]    Script Date: 21-05-2024 16:04:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BDSubToCr](
	[bd_date] [date] NULL,
	[NameOfClient] [varchar](200) NULL,
	[NewExist] [varchar](200) NULL,
	[Status] [varchar](200) NULL,
	[Facility] [varchar](200) NULL,
	[FLS] [varchar](200) NULL,
	[Sub_date] [date] NULL,
	[CreditAnalyst] [varchar](200) NULL,
	[CreditPriority] [varchar](200) NULL,
	[ExpDis] [varchar](200) NULL,
	[Remarks] [varchar](200) NULL,
	[bd_isactive] [char](1) NULL,
	[bd_creadteddate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_BDsummary]    Script Date: 21-05-2024 16:04:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_BDsummary](
	[bd_date] [date] NULL,
	[Branch] [varchar](200) NULL,
	[NewEPSCNo] [varchar](200) NULL,
	[NewEPSCLimit] [varchar](200) NULL,
	[NewSancNo] [varchar](200) NULL,
	[NewSancLimit] [varchar](200) NULL,
	[ActNo] [varchar](200) NULL,
	[ActLimit] [varchar](200) NULL,
	[SancActNo] [varchar](200) NULL,
	[SancActLimit] [varchar](200) NULL,
	[ProCrNo] [varchar](200) NULL,
	[ProCrLimit] [varchar](200) NULL,
	[ProBrNo] [varchar](200) NULL,
	[ProBrlimit] [varchar](200) NULL,
	[EstFIU] [varchar](200) NULL,
	[bd_isactive] [char](1) NULL,
	[bd_creadteddate] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_BDBrQuRl] ADD  DEFAULT ((1)) FOR [bd_isactive]
GO
ALTER TABLE [dbo].[tbl_BDBrQuRl] ADD  DEFAULT (getdate()) FOR [bd_creadteddate]
GO
ALTER TABLE [dbo].[tbl_BDSanctioned] ADD  DEFAULT ((1)) FOR [bd_isactive]
GO
ALTER TABLE [dbo].[tbl_BDSanctioned] ADD  DEFAULT (getdate()) FOR [bd_creadteddate]
GO
ALTER TABLE [dbo].[tbl_BDSubToCr] ADD  DEFAULT ((1)) FOR [bd_isactive]
GO
ALTER TABLE [dbo].[tbl_BDSubToCr] ADD  DEFAULT (getdate()) FOR [bd_creadteddate]
GO
ALTER TABLE [dbo].[tbl_BDsummary] ADD  DEFAULT ((1)) FOR [bd_isactive]
GO
ALTER TABLE [dbo].[tbl_BDsummary] ADD  DEFAULT (getdate()) FOR [bd_creadteddate]
GO

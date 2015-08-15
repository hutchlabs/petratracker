USE [Petra_tracker]
GO
/****** Object:  Table [dbo].[Emails]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Emails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sent_to] [nvarchar](max) NULL,
	[sent_to_id] [nvarchar](50) NULL,
	[email_text] [ntext] NULL,
	[email_type] [ntext] NULL,
	[job_type] [nchar](10) NULL,
	[job_id] [int] NULL,
	[modified_by] [int] NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Emails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_type] [nvarchar](50) NOT NULL,
	[job_description] [nvarchar](max) NOT NULL,
	[tier] [nvarchar](50) NULL,
	[status] [nvarchar](50) NOT NULL,
	[owner] [int] NULL,
	[approved_by] [int] NULL,
	[date_approved] [datetime2](7) NULL,
	[modified_by] [int] NULL,
	[created_at] [datetime2](7) NULL,
	[updated_at] [datetime2](7) NULL,
 CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[to_role_id] [int] NOT NULL,
	[from_user_id] [int] NOT NULL,
	[notification_type] [nvarchar](50) NOT NULL,
	[job_type] [nvarchar](50) NOT NULL,
	[job_id] [int] NOT NULL,
	[last_sent] [datetime2](7) NOT NULL CONSTRAINT [DF_Notifications_last_sent]  DEFAULT (getdate()),
	[times_sent] [int] NOT NULL CONSTRAINT [DF_Notifications_times_sent]  DEFAULT ((1)),
	[status] [nvarchar](50) NOT NULL,
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Notifications_created_at]  DEFAULT (getdate()),
	[updated_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Notifications_updated_at]  DEFAULT (getdate()),
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PDealDescriptions]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PDealDescriptions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[payment_id] [int] NOT NULL,
	[month] [int] NOT NULL,
	[year] [int] NOT NULL,
	[contribution_type_id] [int] NOT NULL,
	[contribution_type] [nvarchar](50) NOT NULL,
	[owner] [int] NULL,
	[modified_by] [int] NULL,
	[created_at] [datetime2](7) NULL,
	[updated_at] [datetime2](7) NULL,
 CONSTRAINT [PK_PDealDescriptions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PPayments]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PPayments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [int] NULL,
	[transaction_ref_no] [nvarchar](50) NOT NULL,
	[transaction_details] [text] NOT NULL,
	[transaction_date] [date] NOT NULL,
	[value_date] [date] NOT NULL,
	[transaction_amount] [money] NOT NULL,
	[tier] [nvarchar](50) NULL,
	[subscription_value_date] [datetime2](7) NULL,
	[subscription_amount] [money] NULL,
	[company_code] [nvarchar](50) NULL,
	[company_name] [nvarchar](200) NULL,
	[company_id] [int] NULL,
	[savings_booster] [bit] NULL,
	[savings_booster_client_code] [nvarchar](50) NULL,
	[deal_description] [nvarchar](max) NULL,
	[deal_description_period] [text] NULL,
	[status] [nvarchar](50) NOT NULL,
	[identified_by] [int] NULL,
	[date_identified] [datetime2](7) NULL,
	[approved_by] [int] NULL,
	[date_approved] [datetime2](7) NULL,
	[comments] [text] NULL,
	[owner] [int] NULL,
	[modified_by] [int] NULL,
	[created_at] [datetime2](7) NULL,
	[updated_at] [datetime2](7) NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role] [nvarchar](50) NOT NULL,
	[description] [nvarchar](100) NOT NULL,
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NULL CONSTRAINT [DF_Roles_created_at]  DEFAULT (getdate()),
	[updated_at] [datetime2](7) NULL CONSTRAINT [DF_Roles_updated_at]  DEFAULT (getdate()),
 CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[company_id] [nvarchar](50) NULL,
	[company] [nvarchar](max) NOT NULL,
	[company_email] [nvarchar](max) NULL,
	[tier] [nvarchar](50) NOT NULL,
	[amount] [money] NOT NULL CONSTRAINT [DF_Schedules_amount]  DEFAULT ((0.0)),
	[contributiontype] [nvarchar](50) NOT NULL CONSTRAINT [DF_Schedules_contributiontype]  DEFAULT (N'REGULAR'),
	[contributiontypeid] [int] NOT NULL CONSTRAINT [DF_Schedules_contributiontypeid]  DEFAULT ((1)),
	[month] [int] NOT NULL,
	[year] [int] NOT NULL,
	[validated] [bit] NOT NULL CONSTRAINT [DF_Schedules_validated]  DEFAULT ((0)),
	[validation_status] [nvarchar](50) NOT NULL CONSTRAINT [DF_Schedules_validation_status]  DEFAULT (N'Not Validated'),
	[validation_valuetime] [datetime2](7) NULL,
	[resolution_reminder1_date] [datetime2](7) NULL,
	[resolution_reminder2_date] [datetime2](7) NULL,
	[resolution_type] [nvarchar](50) NULL,
	[resolution_info] [nvarchar](max) NULL,
	[resolution_date] [datetime2](7) NULL,
	[payment_id] [int] NULL,
	[receipt_sent] [bit] NOT NULL CONSTRAINT [DF_Schedules_receipt_sent]  DEFAULT ((0)),
	[receipt_sent_date] [datetime2](7) NULL,
	[file_downloaded] [bit] NOT NULL CONSTRAINT [DF_Schedules_file_downloaded]  DEFAULT ((0)),
	[file_downloaded_date] [datetime2](7) NULL,
	[file_uploaded] [bit] NOT NULL CONSTRAINT [DF_Schedules_file_uploaded]  DEFAULT ((0)),
	[file_uploaded_date] [datetime2](7) NULL,
	[email_last_sent] [datetime2](7) NULL CONSTRAINT [DF_Schedules_email_last_sent]  DEFAULT (getdate()),
	[emails_sent] [int] NULL CONSTRAINT [DF_Schedules_emails_sent]  DEFAULT ((0)),
	[processing] [bit] NOT NULL CONSTRAINT [DF_Schedules_processing]  DEFAULT ((0)),
	[workflow_status] [nvarchar](max) NULL,
	[workflow_summary] [nvarchar](max) NULL,
	[parent_id] [int] NULL CONSTRAINT [DF_Schedules_parent_id]  DEFAULT ((0)),
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NOT NULL,
	[ptas_fund_deal_id] [int] NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[setting] [nvarchar](50) NOT NULL,
	[value] [text] NOT NULL,
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Settings_created_at]  DEFAULT (getdate()),
	[updated_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Settings_updated_at]  DEFAULT (getdate()),
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/15/2015 3:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_id] [int] NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [char](60) NOT NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[middle_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NOT NULL,
	[status] [bit] NOT NULL CONSTRAINT [DF_Users_status]  DEFAULT ((1)),
	[theme] [nvarchar](50) NOT NULL CONSTRAINT [DF_Users_theme]  DEFAULT (N'BaseLight'),
	[accent] [nvarchar](50) NOT NULL CONSTRAINT [DF_Users_accent]  DEFAULT (N'Blue'),
	[first_login] [bit] NOT NULL CONSTRAINT [DF_Users_first_login]  DEFAULT ((1)),
	[last_login] [datetime2](7) NOT NULL CONSTRAINT [DF_Users_last_login]  DEFAULT (getdate()),
	[logged_in] [bit] NOT NULL CONSTRAINT [DF_Users_logged_in]  DEFAULT ((0)),
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Users_created_at]  DEFAULT (getdate()),
	[updated_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Users_updated_at]  DEFAULT (getdate()),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [Username_Users] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Emails] ADD  CONSTRAINT [DF_Emails_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Emails] ADD  CONSTRAINT [DF_Emails_updated_at]  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[Emails]  WITH CHECK ADD  CONSTRAINT [FK_Emails_Users] FOREIGN KEY([modified_by])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Emails] CHECK CONSTRAINT [FK_Emails_Users]
GO
ALTER TABLE [dbo].[Jobs]  WITH CHECK ADD  CONSTRAINT [FK_Jobs_Users] FOREIGN KEY([owner])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Jobs] CHECK CONSTRAINT [FK_Jobs_Users]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Roles] FOREIGN KEY([to_role_id])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Roles]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Users] FOREIGN KEY([from_user_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Users]
GO
ALTER TABLE [dbo].[PDealDescriptions]  WITH CHECK ADD  CONSTRAINT [FK_PDealDescriptions_PPayments] FOREIGN KEY([payment_id])
REFERENCES [dbo].[PPayments] ([id])
GO
ALTER TABLE [dbo].[PDealDescriptions] CHECK CONSTRAINT [FK_PDealDescriptions_PPayments]
GO
ALTER TABLE [dbo].[PDealDescriptions]  WITH CHECK ADD  CONSTRAINT [FK_PDealDescriptions_Users] FOREIGN KEY([owner])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[PDealDescriptions] CHECK CONSTRAINT [FK_PDealDescriptions_Users]
GO
ALTER TABLE [dbo].[PPayments]  WITH NOCHECK ADD  CONSTRAINT [FK_Payments_Users_ApprovedBy] FOREIGN KEY([approved_by])
REFERENCES [dbo].[Users] ([id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PPayments] NOCHECK CONSTRAINT [FK_Payments_Users_ApprovedBy]
GO
ALTER TABLE [dbo].[PPayments]  WITH NOCHECK ADD  CONSTRAINT [FK_Payments_Users_Owner] FOREIGN KEY([owner])
REFERENCES [dbo].[Users] ([id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[PPayments] NOCHECK CONSTRAINT [FK_Payments_Users_Owner]
GO
ALTER TABLE [dbo].[Schedules]  WITH NOCHECK ADD  CONSTRAINT [FK_Schedules_Payments] FOREIGN KEY([payment_id])
REFERENCES [dbo].[PPayments] ([id])
GO
ALTER TABLE [dbo].[Schedules] NOCHECK CONSTRAINT [FK_Schedules_Payments]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Users] FOREIGN KEY([modified_by])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Users]
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_Modifier] FOREIGN KEY([modified_by])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_Modifier]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([role_id])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tracks who last modified a setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Settings', @level2type=N'CONSTRAINT',@level2name=N'FK_Settings_Modifier'
GO

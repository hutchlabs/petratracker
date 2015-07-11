USE [master]
GO
/****** Object:  Database [Petra_tracker]    Script Date: 7/11/2015 12:56:02 PM ******/
CREATE DATABASE [Petra_tracker]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Petra_tracker', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Petra_tracker.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Petra_tracker_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Petra_tracker_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Petra_tracker] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Petra_tracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Petra_tracker] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Petra_tracker] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Petra_tracker] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Petra_tracker] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Petra_tracker] SET ARITHABORT OFF 
GO
ALTER DATABASE [Petra_tracker] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Petra_tracker] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Petra_tracker] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Petra_tracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Petra_tracker] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Petra_tracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Petra_tracker] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Petra_tracker] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Petra_tracker] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Petra_tracker] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Petra_tracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Petra_tracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Petra_tracker] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Petra_tracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Petra_tracker] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Petra_tracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Petra_tracker] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Petra_tracker] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Petra_tracker] SET  MULTI_USER 
GO
ALTER DATABASE [Petra_tracker] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Petra_tracker] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Petra_tracker] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Petra_tracker] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Petra_tracker]
GO
/****** Object:  Table [dbo].[Emails]    Script Date: 7/11/2015 12:56:02 PM ******/
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
/****** Object:  Table [dbo].[Jobs]    Script Date: 7/11/2015 12:56:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_type] [nvarchar](50) NOT NULL,
	[job_description] [nvarchar](max) NOT NULL,
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
/****** Object:  Table [dbo].[Notifications]    Script Date: 7/11/2015 12:56:02 PM ******/
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
	[last_sent] [datetime2](7) NOT NULL,
	[times_sent] [int] NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Payments]    Script Date: 7/11/2015 12:56:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[job_id] [int] NULL,
	[transaction_ref_no] [nvarchar](50) NOT NULL,
	[transaction_details] [text] NOT NULL,
	[transaction_date] [date] NOT NULL,
	[value_date] [date] NOT NULL,
	[transaction_amount] [money] NOT NULL,
	[subscription_value_date] [datetime2](7) NULL,
	[subscription_amount] [money] NULL,
	[company_code] [nvarchar](50) NULL,
	[savings_booster] [bit] NULL,
	[savings_booster_client_code] [nvarchar](50) NULL,
	[deal_description] [nvarchar](max) NULL,
	[deal_description_period] [nvarchar](50) NULL,
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
/****** Object:  Table [dbo].[Roles]    Script Date: 7/11/2015 12:56:02 PM ******/
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
/****** Object:  Table [dbo].[Schedules]    Script Date: 7/11/2015 12:56:02 PM ******/
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
	[contributiontype] [nvarchar](50) NOT NULL,
	[contributiontypeid] [int] NOT NULL,
	[month] [int] NOT NULL,
	[year] [int] NOT NULL,
	[validated] [bit] NOT NULL,
	[validation_status] [nvarchar](50) NOT NULL,
	[validation_valuetime] [datetime2](7) NULL,
	[resolution_reminder1_date] [datetime2](7) NULL,
	[resolution_reminder2_date] [datetime2](7) NULL,
	[resolution_type] [nvarchar](50) NULL,
	[resolution_info] [nvarchar](max) NULL,
	[resolution_date] [datetime2](7) NULL,
	[payment_id] [int] NULL,
	[receipt_sent] [bit] NOT NULL,
	[receipt_sent_date] [datetime2](7) NULL,
	[file_downloaded] [bit] NOT NULL,
	[file_downloaded_date] [datetime2](7) NULL,
	[file_uploaded] [bit] NOT NULL,
	[file_uploaded_date] [datetime2](7) NULL,
	[email_last_sent] [datetime2](7) NULL,
	[emails_sent] [int] NULL,
	[processing] [bit] NOT NULL,
	[workflow_status] [nvarchar](max) NULL,
	[workflow_summary] [nvarchar](max) NULL,
	[parent_id] [int] NULL,
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 7/11/2015 12:56:02 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 7/11/2015 12:56:02 PM ******/
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
	[theme] [nchar](10) NOT NULL CONSTRAINT [DF_Users_theme]  DEFAULT (N'BaseLight'),
	[accent] [nchar](10) NOT NULL CONSTRAINT [DF_Users_accent]  DEFAULT (N'Blue'),
	[first_login] [bit] NOT NULL CONSTRAINT [DF_Users_first_login]  DEFAULT ((1)),
	[last_login] [datetime2](7) NOT NULL CONSTRAINT [DF_Users_last_login]  DEFAULT (getdate()),
	[logged_in] [bit] NOT NULL CONSTRAINT [DF_Users_logged_in]  DEFAULT ((0)),
	[modified_by] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Users_created_at]  DEFAULT (getdate()),
	[updated_at] [datetime2](7) NOT NULL CONSTRAINT [DF_Users_updated_at]  DEFAULT (getdate()),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

GO
INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (1, N'Super User', N'Full access all features', 1, CAST(N'2015-06-20 15:04:53.2000000' AS DateTime2), CAST(N'2015-06-20 15:04:53.2000000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (2, N'Administrator', N'Create and edit users', 1, CAST(N'2015-06-20 15:05:10.2330000' AS DateTime2), CAST(N'2015-06-20 15:05:10.2330000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (3, N'Super Ops User', N'Access to parsing features and approval authority', 1, CAST(N'2015-06-20 15:05:28.0230000' AS DateTime2), CAST(N'2015-06-20 15:05:28.0230000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (4, N'Ops User', N'Access to parsing features', 1, CAST(N'2015-06-20 15:06:02.0070000' AS DateTime2), CAST(N'2015-06-20 15:06:02.0070000' AS DateTime2))
GO
INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (8, N'Reporter', N'Access to reports', 1, CAST(N'2015-06-22 09:55:04.8100000' AS DateTime2), CAST(N'2015-06-22 09:55:04.8100000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Settings] ON 

GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (1, N'email_smtp_host', N'smtp.gmail.com', 1, CAST(N'2015-06-22 11:07:20.4000000' AS DateTime2), CAST(N'2015-06-22 11:07:20.4000000' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (3, N'email_from', N'no-reply@petratrust.com', 1, CAST(N'2015-06-22 11:07:42.5170000' AS DateTime2), CAST(N'2015-07-11 11:36:39.3300132' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (4, N'permission_approveown', N'True', 1, CAST(N'2015-06-22 11:12:28.9730000' AS DateTime2), CAST(N'2015-07-11 12:44:57.0140056' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (5, N'tmpl_newuser_email', N'Hello <name>,\n\n
Welcome to the Petra Tracker. Your user credentials are below.\n
You will be asked to reset your password on first login.\n\n
Username: <username>\n
Passwrod: <password>\n\n
Sincerely,\n
Petra Admin Team', 1, CAST(N'2015-06-22 11:19:02.8600000' AS DateTime2), CAST(N'2015-06-22 11:19:02.8600000' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (6, N'tmpl_resetpass_email', N'Hello Admin,\n\n
<username> has requested a password reset.\n\n 
Sincerely,\n
Tracker System', 1, CAST(N'2015-06-22 11:21:34.6430000' AS DateTime2), CAST(N'2015-06-22 11:21:34.6430000' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (7, N'time_update_schedules', N'5', 1, CAST(N'2015-07-06 14:14:55.1770000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8191699' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (9, N'time_retry_validationrequest', N'24', 1, CAST(N'2015-07-06 14:32:19.2800000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8245851' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (10, N'time_retry_errorfix3rd', N'120', 1, CAST(N'2015-06-22 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8427153' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (11, N'time_retry_errorfix2nd', N'72', 1, CAST(N'2015-07-06 15:03:23.0900000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8369043' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (17, N'time_retry_errorfix1st', N'48', 1, CAST(N'2015-06-10 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8314632' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (18, N'time_retry_receiptsendrequest', N'24', 1, CAST(N'2015-07-06 17:29:28.4600000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8697476' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (19, N'time_retry_filedownloadrequest', N'24', 1, CAST(N'2015-07-06 18:07:58.5300000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8767591' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (20, N'time_retry_fileuploadrequest', N'24', 1, CAST(N'2015-07-06 18:14:31.5300000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8819922' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (21, N'time_window_fileupload', N'36', 1, CAST(N'2015-07-06 18:15:44.2300000' AS DateTime2), CAST(N'2015-07-11 12:40:44.8875667' AS DateTime2))
GO
INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (26, N'time_update_notifications', N'1', 1, CAST(N'2015-07-06 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-11 12:40:44.7957955' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Settings] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (1, 1, N'admin@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvgoPYyIV06kQmlr2W.', N'Super', NULL, N'Admin', 1, N'BaseLight ', N'Olive     ', 0, CAST(N'2015-07-11 12:45:25.9998752' AS DateTime2), 0, 1, CAST(N'2015-06-20 15:15:14.4370000' AS DateTime2), CAST(N'2015-07-11 12:45:26.0178875' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (3, 1, N'marlene@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvgoPYyIV06kQmlr2W.', N'Emefa', N'', N'Baeta', 0, N'BaseLight ', N'Olive     ', 0, CAST(N'2015-07-10 13:32:26.3535159' AS DateTime2), 0, 3, CAST(N'0001-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-10 13:32:26.3695308' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (4, 3, N'superops@petratrust.com', N'$2a$10$19sMpgXX1wgrkhtWGtBUd.LeCHtgM8RKgmMtUahHyXhLAA9I8jl/W', N'Super', N'Ops', N'User', 0, N'BaseLight ', N'Olive     ', 0, CAST(N'2015-07-10 13:48:17.8035203' AS DateTime2), 0, 4, CAST(N'0001-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-10 13:48:17.8145279' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (5, 4, N'opsuser@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvgoPYyIV06kQmlr2W.', N'Ops', N'', N'User', 0, N'BaseLight ', N'Olive     ', 0, CAST(N'2015-07-10 17:16:51.1939104' AS DateTime2), 0, 5, CAST(N'0001-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-10 17:16:51.2149500' AS DateTime2))
GO
INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (8, 8, N'reporter@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvgoPYyIV06kQmlr2W.', N'Reporter', N'', N'', 1, N'BaseDark  ', N'Green     ', 0, CAST(N'2015-07-03 19:13:34.1201367' AS DateTime2), 0, 8, CAST(N'2015-07-02 22:58:59.1734318' AS DateTime2), CAST(N'2015-07-03 19:13:34.1211372' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Username_Users]    Script Date: 7/11/2015 12:56:02 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [Username_Users] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Emails] ADD  CONSTRAINT [DF_Emails_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Emails] ADD  CONSTRAINT [DF_Emails_updated_at]  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_last_sent]  DEFAULT (getdate()) FOR [last_sent]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_times_sent]  DEFAULT ((1)) FOR [times_sent]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Notifications] ADD  CONSTRAINT [DF_Notifications_updated_at]  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_contributiontype]  DEFAULT (N'REGULAR') FOR [contributiontype]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_contributiontypeid]  DEFAULT ((1)) FOR [contributiontypeid]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_validated]  DEFAULT ((0)) FOR [validated]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_validation_status]  DEFAULT (N'Not Validated') FOR [validation_status]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_receipt_sent]  DEFAULT ((0)) FOR [receipt_sent]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_file_downloaded]  DEFAULT ((0)) FOR [file_downloaded]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_file_uploaded]  DEFAULT ((0)) FOR [file_uploaded]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_email_last_sent]  DEFAULT (getdate()) FOR [email_last_sent]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_emails_sent]  DEFAULT ((0)) FOR [emails_sent]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_processing]  DEFAULT ((0)) FOR [processing]
GO
ALTER TABLE [dbo].[Schedules] ADD  CONSTRAINT [DF_Schedules_parent_id]  DEFAULT ((0)) FOR [parent_id]
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
ALTER TABLE [dbo].[Payments]  WITH NOCHECK ADD  CONSTRAINT [FK_Payments_Users_ApprovedBy] FOREIGN KEY([approved_by])
REFERENCES [dbo].[Users] ([id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Payments] NOCHECK CONSTRAINT [FK_Payments_Users_ApprovedBy]
GO
ALTER TABLE [dbo].[Payments]  WITH NOCHECK ADD  CONSTRAINT [FK_Payments_Users_Owner] FOREIGN KEY([owner])
REFERENCES [dbo].[Users] ([id])
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[Payments] NOCHECK CONSTRAINT [FK_Payments_Users_Owner]
GO
ALTER TABLE [dbo].[Schedules]  WITH NOCHECK ADD  CONSTRAINT [FK_Schedules_Payments] FOREIGN KEY([payment_id])
REFERENCES [dbo].[Payments] ([id])
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
USE [master]
GO
ALTER DATABASE [Petra_tracker] SET  READ_WRITE 
GO

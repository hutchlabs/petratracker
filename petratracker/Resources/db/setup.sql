USE [Petra_tracker]

SET ANSI_PADDING OFF

SET IDENTITY_INSERT [dbo].[Roles] ON 


INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (1, N'Super User', N'Full access all features', 1, CAST(N'2015-06-20 15:04:53.2000000' AS DateTime2), CAST(N'2015-06-20 15:04:53.2000000' AS DateTime2))

INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (2, N'Administrator', N'Create and edit users', 1, CAST(N'2015-06-20 15:05:10.2330000' AS DateTime2), CAST(N'2015-06-20 15:05:10.2330000' AS DateTime2))

INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (3, N'Super Ops User', N'Access to parsing features and approval authority', 1, CAST(N'2015-06-20 15:05:28.0230000' AS DateTime2), CAST(N'2015-06-20 15:05:28.0230000' AS DateTime2))

INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (4, N'Ops User', N'Access to parsing features', 1, CAST(N'2015-06-20 15:06:02.0070000' AS DateTime2), CAST(N'2015-06-20 15:06:02.0070000' AS DateTime2))

INSERT [dbo].[Roles] ([id], [role], [description], [modified_by], [created_at], [updated_at]) VALUES (8, N'Reporter', N'Access to reports', 1, CAST(N'2015-06-22 09:55:04.8100000' AS DateTime2), CAST(N'2015-06-22 09:55:04.8100000' AS DateTime2))

SET IDENTITY_INSERT [dbo].[Roles] OFF



SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (1, 1, N'admin@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvPYyIV06kQmlr2W.', N'Super', N'Uber', N'Admin', 1, N'BaseLight', N'Olive', 1, CAST(N'2015-08-01 10:04:17.6296352' AS DateTime2), 0, 1, CAST(N'2015-06-20 15:15:14.4370000' AS DateTime2), CAST(N'2015-08-01 10:04:17.6416624' AS DateTime2))

INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (3, 1, N'marlene@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvPYyIV06kQmlr2W.', N'Emefa', N'', N'Baeta', 1, N'BaseLight', N'Olive', 1, CAST(N'2015-07-10 13:32:26.3535159' AS DateTime2), 0, 3, CAST(N'0001-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-10 13:32:26.3695308' AS DateTime2))

INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (4, 3, N'superops@petratrust.com', N'$2a$10$19sMpgXX1wgrkhtWGtBUd.LeCHtgM8RKgmMtUahHyXhLAA9I8jl/W', N'Super', N'Ops', N'User', 1, N'BaseLight', N'Olive', 1, CAST(N'2015-08-01 09:24:28.7965263' AS DateTime2), 0, 4, CAST(N'0001-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-08-01 09:24:28.8075330' AS DateTime2))

INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (5, 4, N'opsuser@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvPYyIV06kQmlr2W.', N'Ops', N'', N'User', 1, N'BaseLight', N'Olive', 1, CAST(N'2015-07-31 23:21:26.6943306' AS DateTime2), 0, 5, CAST(N'0001-01-01 00:00:00.0000000' AS DateTime2), CAST(N'2015-07-31 23:21:26.7023371' AS DateTime2))

INSERT [dbo].[Users] ([id], [role_id], [username], [password], [first_name], [middle_name], [last_name], [status], [theme], [accent], [first_login], [last_login], [logged_in], [modified_by], [created_at], [updated_at]) VALUES (8, 8, N'reporter@petratrust.com', N'$2a$10$0Eg5UkxJ9JwYLQoqXJVARepn/3WAB8iPyXYvPYyIV06kQmlr2W.', N'Reporter', N'', N'', 1, N'BaseDark', N'Green', 1, CAST(N'2015-07-03 19:13:34.1201367' AS DateTime2), 0, 8, CAST(N'2015-07-02 22:58:59.1734318' AS DateTime2), CAST(N'2015-07-03 19:13:34.1211372' AS DateTime2))

SET IDENTITY_INSERT [dbo].[Users] OFF

SET IDENTITY_INSERT [dbo].[Settings] ON 

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (1, N'app_version', N'1.0.0.0', 1, CAST(N'2015-08-01 10:47:08.1870000' AS DateTime2), CAST(N'2015-08-01 10:47:08.1870000' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (2, N'email_smtp_host', N'smtp.gmail.com', 1, CAST(N'2015-06-22 11:07:20.4000000' AS DateTime2), CAST(N'2015-06-22 11:07:20.4000000' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (3, N'email_from', N'no-reply@petratrust.com', 1, CAST(N'2015-06-22 11:07:42.5170000' AS DateTime2), CAST(N'2015-07-11 11:36:39.3300132' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (4, N'permission_approveown', N'True', 1, CAST(N'2015-06-22 11:12:28.9730000' AS DateTime2), CAST(N'2015-07-12 14:44:21.3560016' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (5, N'tmpl_newuser_email', N'Hello <name>,\n\n
Welcome to the Petra Tracker. Your user credentials are below.\n
You will be asked to reset your password on first login.\n\n
Username: <username>\n
Passwrod: <password>\n\n
Sincerely,\n
Petra Admin Team', 1, CAST(N'2015-06-22 11:19:02.8600000' AS DateTime2), CAST(N'2015-06-22 11:19:02.8600000' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (6, N'tmpl_resetpass_email', N'Hello Admin,\n\n
<username> has requested a password reset.\n\n 
Sincerely,\n
Tracker System', 1, CAST(N'2015-06-22 11:21:34.6430000' AS DateTime2), CAST(N'2015-06-22 11:21:34.6430000' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (7, N'time_update_schedules', N'5', 1, CAST(N'2015-07-06 14:14:55.1770000' AS DateTime2), CAST(N'2015-08-01 10:04:05.7820384' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (8, N'time_retry_validationrequest', N'24', 1, CAST(N'2015-07-06 14:32:19.2800000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8192411' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (9, N'time_retry_errorfix3rd', N'120', 1, CAST(N'2015-06-22 00:00:00.0000000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8482838' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (10, N'time_retry_errorfix2nd', N'72', 1, CAST(N'2015-07-06 15:03:23.0900000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8392553' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (11, N'time_retry_errorfix1st', N'48', 1, CAST(N'2015-06-10 00:00:00.0000000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8292696' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (12, N'time_retry_receiptsendrequest', N'24', 1, CAST(N'2015-07-06 17:29:28.4600000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8552782' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (13, N'time_retry_filedownloadrequest', N'24', 1, CAST(N'2015-07-06 18:07:58.5300000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8612825' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (14, N'time_retry_fileuploadrequest', N'24', 1, CAST(N'2015-07-06 18:14:31.5300000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8672867' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (15, N'time_window_fileupload', N'36', 1, CAST(N'2015-07-06 18:15:44.2300000' AS DateTime2), CAST(N'2015-08-01 10:04:05.8722713' AS DateTime2))

INSERT [dbo].[Settings] ([id], [setting], [value], [modified_by], [created_at], [updated_at]) VALUES (16, N'time_update_notifications', N'1', 1, CAST(N'2015-07-06 00:00:00.0000000' AS DateTime2), CAST(N'2015-08-01 10:04:05.7640417' AS DateTime2))

SET IDENTITY_INSERT [dbo].[Settings] OFF

/*
后期部署脚本模板							
--------------------------------------------------------------------------------------
 此文件包含将附加到生成脚本中的 SQL 语句。		
 使用 SQLCMD 语法将文件包含到后期部署脚本中。			
 示例:      :r .\myfile.sql								
 使用 SQLCMD 语法引用后期部署脚本中的变量。		
 示例:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SET IDENTITY_INSERT [dbo].[UserProfile] ON
INSERT INTO [dbo].[UserProfile] ([UserId], [UserName], [Email], [PhoneNumber]) VALUES (1, N'admin', N'', N'')
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO

INSERT INTO [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate])
    VALUES (1, GETDATE(), N'9f1cc839-43ef-4929-b8df-b663574d8226', 1, GETDATE(), 0, N'8D8C97133C046153F0CEF2199ED3F8C1', GETDATE(), N'4b50d3f4-e5bb-4672-9433-bc84652cf004', NULL, NULL)
GO

SET IDENTITY_INSERT [dbo].[webpages_Roles] ON
INSERT INTO [dbo].[webpages_Roles] ([RoleId], [RoleName], [Description]) VALUES (1, N'admin', NULL)
INSERT INTO [dbo].[webpages_Roles] ([RoleId], [RoleName], [Description]) VALUES (2, N'users', NULL)
SET IDENTITY_INSERT [dbo].[webpages_Roles] OFF
GO

INSERT INTO [dbo].[webpages_UsersInRoles] ([UserId], [RoleId]) VALUES (1, 1)
GO

SET IDENTITY_INSERT [dbo].[Dep_Department] ON
INSERT INTO [dbo].[Dep_Department] ([Id], [ParentId], [DepHID], [Ord], [Name], [Remark], [DepType], [ExamineType], [IsActive], [IsDisabled], [IsDeleted], [CreateDatetime], [ModifiedDateTime], [OrgNode])
    VALUES (1, 0, NULL, 1, N'侏罗纪软件股份有限公司', NULL, NULL, NULL, 1, 0, 0, GETDATE(), GETDATE(), NULL)
SET IDENTITY_INSERT [dbo].[Dep_Department] OFF
GO

SET IDENTITY_INSERT [dbo].[Dep_DepUser] ON
INSERT INTO [dbo].[Dep_DepUser] ([Id], [DepId], [UserId], [UserName], [ExamineType], [ContractType], [ContractLenght], [JoinDateTime], [OutDateTime], [CreateDatetime], [IsSuspension], [IsDeleted], [PostId])
    VALUES (1, 1, 1, N'admin', N'1', N'1', 3, GETDATE(), NULL, GETDATE(), 0, 0, NULL)
SET IDENTITY_INSERT [dbo].[Dep_DepUser] OFF

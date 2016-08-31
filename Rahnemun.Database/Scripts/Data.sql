INSERT [dbo].[Framework_Roles]([Name], [DisplayName])
VALUES (N'Consultant',N'مشاور')
GO

-- I got the data from http://moshavere.tebyan.net/
SET IDENTITY_INSERT [dbo].[Rahnemun_CategoryGroups] ON 
GO
INSERT [dbo].[Rahnemun_CategoryGroups] ([Id], [Caption])
SELECT 1, N'روانشناسی'
UNION ALL
SELECT 2, N'سلامتی'
UNION ALL
SELECT 3, N'مشاوره تحصیلی'
UNION ALL
SELECT 4, N'حقوقی'
GO
SET IDENTITY_INSERT [dbo].[Rahnemun_CategoryGroups] OFF
GO

SET IDENTITY_INSERT [dbo].[Rahnemun_Categories] ON 
GO
INSERT [dbo].[Rahnemun_Categories] ([Id], [Caption], [CategoryGroupId])
SELECT 1, N'انتخاب همسر', 1
UNION ALL
SELECT 2, N'مشاوره پس از ازدواج', 1
UNION ALL
SELECT 3, N'اضطراب', 1
UNION ALL
SELECT 4, N'پزشکی', 2
UNION ALL
SELECT 5, N'تغذیه', 2
UNION ALL
SELECT 6, N'کنکور کارشناسی', 3
UNION ALL
SELECT 7, N'کنکور ارشد', 3
UNION ALL
SELECT 8, N'حقوقی', 4
GO
SET IDENTITY_INSERT [dbo].[Rahnemun_Categories] OFF
GO

UPDATE [dbo].[Rahnemun_Categories]
   SET [Description] = N'در خصوص ' + [Caption] + N' می باشد'
GO

INSERT INTO [dbo].[Framework_Users]
		   ([Id]
		   ,[Username]
		   ,[Email]
		   ,[Password]
		   ,[PasswordSalt]
		   ,[HashAlgorithm]
		   ,[PasswordFormat]
		   ,[Approved]
		   ,[EmailConfirmed]
		   ,[Disabled]
		   ,[UserData])
	 VALUES
		   (2
		   ,N'mhmohammadi'
		   ,N'mhm@rahnemun.ir'
		   ,N'7YNP9wO299s3vfG71utHHcqDSAM='
		   ,N'TUhhs2qU1zt4ykEd/DOIgQ=='
		   ,N'SHA1'
		   ,1
		   ,'True'
		   ,'True'
		   ,'False'
		   ,'###################################')
		   ,
		   (3
		   ,N'dlotfi'
		   ,N'dlotfi@rahnemun.ir'
		   ,N'7YNP9wO299s3vfG71utHHcqDSAM='
		   ,N'TUhhs2qU1zt4ykEd/DOIgQ=='
		   ,N'SHA1'
		   ,1
		   ,'True'
		   ,'True'
		   ,'False'
		   ,'###################################')
		   ,
		   (4
		   ,N'mmoghaddam'
		   ,N'mmoghaddam@rahnemun.ir'
		   ,N'7YNP9wO299s3vfG71utHHcqDSAM='
		   ,N'TUhhs2qU1zt4ykEd/DOIgQ=='
		   ,N'SHA1'
		   ,1
		   ,'True'
		   ,'True'
		   ,'False'
		   ,'###################################')
		   ,
		   (5
		   ,N'disapproveduser'
		   ,N'disapproveduser@rahnemun.ir'
		   ,N'7YNP9wO299s3vfG71utHHcqDSAM='
		   ,N'TUhhs2qU1zt4ykEd/DOIgQ=='
		   ,N'SHA1'
		   ,1
		   ,'False'
		   ,'False'
		   ,'True'
		   ,'###################################')
		   ,
		   (6
		   ,N'disableduser'
		   ,N'disableduser@rahnemun.ir'
		   ,N'7YNP9wO299s3vfG71utHHcqDSAM='
		   ,N'TUhhs2qU1zt4ykEd/DOIgQ=='
		   ,N'SHA1'
		   ,1
		   ,'True'
		   ,'True'
		   ,'True'
		   ,'###################################')
GO

INSERT INTO [dbo].[Rahnemun_Users]
		   ([Id]
		   ,[FirstName]
		   ,[LastName]
		   ,[Gender]
		   ,[EducationLevel]
		   ,[MaritalStatus]
		   ,[BirthDate]
		   ,[NationalId]
		   ,[CellphoneNo]
		   ,[RegisterDate])
	 VALUES
		   (2
		   ,N'محمد حسین'
		   ,N'محمدی'
		   ,0
		   ,6
		   ,1
		   ,'1983-01-01'
		   ,'1111111111'
		   ,'09126627116'
		   ,'2014-11-23')
		   ,
		   (3
		   ,N'داریوش'
		   ,N'لطفی'
		   ,0
		   ,6
		   ,0
		   ,'1985-01-01'
		   ,'2222222222'
		   ,'09102106238'
		   ,'2014-11-23')
		   ,
		   (4
		   ,N'مصطفی'
		   ,N'مقدم'
		   ,0
		   ,6
		   ,0
		   ,'1980-01-01'
		   ,'3333333333'
		   ,'09127397162'
		   ,'2014-11-23')
		   ,
		   (5
		   ,N'کاربر فاقد ایمیل'
		   ,N'تایید نشده'
		   ,1
		   ,0
		   ,1
		   ,'1990-01-01'
		   ,'4444444444'
		   ,'09370000000'
		   ,'2014-11-23')
		   ,
		   (6
		   ,N'کاربر غیر فعال'
		   ,N'فعال نشده'
		   ,1
		   ,0
		   ,0
		   ,'1991-01-01'
		   ,'5555555555'
		   ,'09350000000'
		   ,'2014-11-23')
		   
GO

INSERT INTO [dbo].[Rahnemun_Consultants]
		   ([Id]
		   ,[Specialty]
		   ,[CategoryId]
		   ,[Education]
		   ,[ProfessionalExperience]
		   ,[WorkAddress]
		   ,[WorkPhoneNo]
		   ,[Capacity]
		   ,[BankCardNo]
		   ,[Approved])
	 VALUES
		   (2
		   ,N'مشاوره تحصیلی در تمامی رشته های فنی و مهندسی'
		   ,6
		   ,N'لیسانس مهندسی کامپیوتر دانشگاه آزاد شهر قدس و فوق لیسانس مکاترونیک دانشگاه آزاد قزوین'
		   ,N'برنامه نویسی در حوزه بانک اطلاعانی'
		   ,N'تهران شهرک گلستان بلوار کاج'
		   ,'09126627116'
		   ,10
		   ,'1234123412341234'
		   ,'True')
		   ,
		   (3
		   ,N'مشاوره تحصیلی در تمامی رشته های فنی و مهندسی'
		   ,7
		   ,N'لیسانس مهندسی کامپیوتر دانشگاه آزاد شهر قدس و فوق لیسانس مهندسی نرم افزار دانشگاه علوم تحقیقات'
		   ,N'برنامه نویسی در حوزه بانک اطلاعانی و ایجاد فریم ورک های خیلی خفن'
		   ,N'تهران شهرک گلستان بلوار کاج'
		   ,'09102106238'
		   ,5
		   ,'2345234523452345'
		   ,'True')
		   ,
		   (4
		   ,N'مشاوره طراحی و ساخت اینترفیس های حرفه ای'
		   ,1
		   ,N'لیسانس گرافیک دانشگاه آزاد شمال'
		   ,N'طراحی اینترفیس و ست اداری'
		   ,N'تهران شهرک اندیشه خ پنجم غربی'
		   ,'09127397162'
		   ,8
		   ,'3456345634563456'
		   ,'True')
		   ,
		   (5
		   ,N'مشاوره اضطراب و پریشان درمانی'
		   ,3
		   ,N'دکتری تنبلی دانشگاه آزاد جنوب'
		   ,N'پریشان درمانی'
		   ,N'تنبل خانه شاه عباسی'
		   ,'09121234567'
		   ,2
		   ,'3456345634563456'
		   ,'False')
		   ,
		   (6
		   ,N'مشاوره حقوقی'
		   ,1
		   ,N'لیسانس حقوق دانشگاه آزاد تهران مرکز'
		   ,N'وکیل پایه یک دادگستری شهر تهران'
		   ,N'تهران سعادت آباد'
		   ,'09350000000'
		   ,0
		   ,'4567456745674567'
		   ,'True')
GO
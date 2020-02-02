CREATE TABLE Users(
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Email] nvarchar(128) NOT NULL,
	[Guid] varchar(255) NOT NULL
)

CREATE TABLE Courses(
	[CourseId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] nvarchar(128) NOT NULL,
	[Description] varchar(255) NOT NULL,
	[PathMp3] varchar(255) NULL,
	[PathMp4] varchar(255) NULL
)

CREATE TABLE UserCourses(
	[UserId] [int] REFERENCES Users(UserId),
	[CourseId] [int] REFERENCES Courses(CourseId),
	[IsPurchased] [bit] NOT NULL DEFAULT(0)
)

CREATE TABLE Modules(
	[ModuleId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[CourseId] [int] REFERENCES Courses(CourseId),
	[Name] nvarchar(128) NOT NULL,
	[Description] varchar(255) NOT NULL
)

CREATE TABLE Lessons(
	[LessonId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] REFERENCES Modules(ModuleId),
	[Name] nvarchar(128) NOT NULL,
	[ContentUrl] nvarchar(128) NOT NULL,
	[ContentRawUrl] nvarchar(128) NULL,
	[Duration] nvarchar(20) NOT NULL
)
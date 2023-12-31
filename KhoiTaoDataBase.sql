USE [master]
GO
/****** Object:  Database [eStore]    Script Date: 10/12/2023 9:11:13 AM ******/
CREATE DATABASE [eStore]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'eStore', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\eStore.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'eStore_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\eStore_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [eStore] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [eStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [eStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [eStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [eStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [eStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [eStore] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [eStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [eStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [eStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [eStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [eStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [eStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [eStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [eStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [eStore] SET  ENABLE_BROKER 
GO
ALTER DATABASE [eStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [eStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [eStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [eStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [eStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [eStore] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [eStore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [eStore] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [eStore] SET  MULTI_USER 
GO
ALTER DATABASE [eStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [eStore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [eStore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [eStore] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [eStore] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [eStore] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'eStore', N'ON'
GO
ALTER DATABASE [eStore] SET QUERY_STORE = OFF
GO
USE [eStore]
GO
/****** Object:  User [new]    Script Date: 10/12/2023 9:11:13 AM ******/
CREATE USER [new] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/12/2023 9:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/12/2023 9:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 10/12/2023 9:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[MemberId] [int] IDENTITY(1,1) NOT NULL,
	[EMail] [nvarchar](50) NOT NULL,
	[CompanyName] [nvarchar](50) NULL,
	[City] [nvarchar](20) NULL,
	[Country] [nvarchar](30) NULL,
	[Password] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 10/12/2023 9:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
	[UnitPrice] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10/12/2023 9:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[OrderDate] [date] NOT NULL,
	[RequiredDate] [date] NULL,
	[ShippedDate] [date] NOT NULL,
	[Freight] [int] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/12/2023 9:11:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Weight] [real] NULL,
	[UnitPrice] [int] NOT NULL,
	[UnitInStock] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230725073544_MyFirstMigration', N'6.0.20')
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (1, N'Grains')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (2, N'Meat')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (3, N'Vegetable')
INSERT [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (4, N'Fruit')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Members] ON 

INSERT [dbo].[Members] ([MemberId], [EMail], [CompanyName], [City], [Country], [Password]) VALUES (1, N'huong@fpt', N'FPT', N'HN', N'VN', N'12345')
INSERT [dbo].[Members] ([MemberId], [EMail], [CompanyName], [City], [Country], [Password]) VALUES (2, N'string', N'string', N'HN', N'VN', N'123')
INSERT [dbo].[Members] ([MemberId], [EMail], [CompanyName], [City], [Country], [Password]) VALUES (3, N'Lan@fpt', NULL, NULL, NULL, N'123')
INSERT [dbo].[Members] ([MemberId], [EMail], [CompanyName], [City], [Country], [Password]) VALUES (4, N'Tuan@fpt', NULL, NULL, NULL, N'12345')
INSERT [dbo].[Members] ([MemberId], [EMail], [CompanyName], [City], [Country], [Password]) VALUES (5, N'Minh@fpt', NULL, NULL, NULL, N'12345')
INSERT [dbo].[Members] ([MemberId], [EMail], [CompanyName], [City], [Country], [Password]) VALUES (6, N'Lan@fpt', NULL, NULL, NULL, N'12345')
SET IDENTITY_INSERT [dbo].[Members] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (1, 1, 1, 10, 1, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (2, 4, 1, 2, 2, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (3, 7, 1, 10, 3, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (4, 2, 2, 9, 3, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (5, 5, 2, 9, 2, 10)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (6, 8, 2, 9, 1, 90)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (7, 10, 3, 9, 2, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (8, 11, 3, 5, 3, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (9, 2, 4, 9, 1, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (10, 6, 4, 5, 2, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (11, 7, 4, 10, 3, 8)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (12, 11, 4, 5, 1, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (13, 8, 5, 9, 1, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (14, 1, 6, 10, 2, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (15, 2, 6, 9, 3, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (16, 3, 6, 5, 2, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (17, 4, 7, 2, 2, 10)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (18, 5, 7, 9, 1, 0)
INSERT [dbo].[OrderDetails] ([OrderDetailId], [ProductId], [OrderId], [UnitPrice], [Quantity], [Discount]) VALUES (19, 6, 8, 5, 3, 0)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (1, 1, CAST(N'2023-09-10' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-12' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (2, 3, CAST(N'2023-09-11' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (3, 1, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (4, 2, CAST(N'2023-09-12' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (5, 4, CAST(N'2023-09-14' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (6, 4, CAST(N'2023-09-15' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (7, 2, CAST(N'2023-09-16' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (8, 2, CAST(N'2023-09-18' AS Date), CAST(N'2023-09-20' AS Date), CAST(N'2023-09-19' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (1, 1, N'Almond', NULL, 10, 20)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (2, 1, N'Walnut', NULL, 9, 20)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (3, 1, N'Chia seed', NULL, 5, 20)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (4, 2, N'Chicken egg', NULL, 2, 200)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (5, 2, N'Fish', NULL, 9, 10)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (6, 2, N'Beef', NULL, 5, 3)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (7, 3, N'Broccoli', NULL, 10, 6)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (8, 3, N'Carrot', NULL, 9, 10)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (9, 4, N'Pineapple', NULL, 10, 10)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (10, 4, N'Avocado', NULL, 9, 2)
INSERT [dbo].[Products] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitInStock]) VALUES (11, 4, N'Coconut', NULL, 5, 10)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
/****** Object:  Index [IX_OrderDetails_OrderId]    Script Date: 10/12/2023 9:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_OrderId] ON [dbo].[OrderDetails]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_ProductId]    Script Date: 10/12/2023 9:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_ProductId] ON [dbo].[OrderDetails]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_MemberId]    Script Date: 10/12/2023 9:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_MemberId] ON [dbo].[Orders]
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_CategoryId]    Script Date: 10/12/2023 9:11:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_Products_CategoryId] ON [dbo].[Products]
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders_OrderId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Products_ProductId]
GO
USE [master]
GO
ALTER DATABASE [eStore] SET  READ_WRITE 
GO

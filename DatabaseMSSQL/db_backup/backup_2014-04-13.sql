USE [master]
GO
/****** Object:  Database [bartoszbartniczak_scrum4u_production]    Script Date: 2014-04-13 10:30:50 ******/
CREATE DATABASE [bartoszbartniczak_scrum4u_production]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bartoszbartniczak_scrum4u_production_data', FILENAME = N'D:\MSSQL\bartoszbartniczak\bartoszbartniczak_scrum4u_production_data.mdf' , SIZE = 4160KB , MAXSIZE = 128000KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'bartoszbartniczak_scrum4u_production_log', FILENAME = N'D:\MSSQL\bartoszbartniczak\bartoszbartniczak_scrum4u_production_log.ldf' , SIZE = 2304KB , MAXSIZE = 512000KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bartoszbartniczak_scrum4u_production].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET ARITHABORT OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET  ENABLE_BROKER 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET RECOVERY FULL 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET  MULTI_USER 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET DB_CHAINING OFF 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [bartoszbartniczak_scrum4u_production]
GO
/****** Object:  User [bartoszbartniczak_dbadmin]    Script Date: 2014-04-13 10:30:51 ******/
CREATE USER [bartoszbartniczak_dbadmin] FOR LOGIN [bartoszbartniczak_dbadmin] WITH DEFAULT_SCHEMA=[bartoszbartniczak_dbadmin]
GO
/****** Object:  User [bartoszbartniczak_aplikacja]    Script Date: 2014-04-13 10:30:51 ******/
CREATE USER [bartoszbartniczak_aplikacja] FOR LOGIN [bartoszbartniczak_aplikacja] WITH DEFAULT_SCHEMA=[bartoszbartniczak_aplikacja]
GO
ALTER ROLE [db_owner] ADD MEMBER [bartoszbartniczak_dbadmin]
GO
ALTER ROLE [db_owner] ADD MEMBER [bartoszbartniczak_aplikacja]
GO
/****** Object:  Schema [bartoszbartniczak_aplikacja]    Script Date: 2014-04-13 10:30:51 ******/
CREATE SCHEMA [bartoszbartniczak_aplikacja]
GO
/****** Object:  Schema [bartoszbartniczak_dbadmin]    Script Date: 2014-04-13 10:30:51 ******/
CREATE SCHEMA [bartoszbartniczak_dbadmin]
GO
/****** Object:  UserDefinedFunction [dbo].[funcCzyKontoAktywne]    Script Date: 2014-04-13 10:30:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[funcCzyKontoAktywne]( @email_uzytkownika NVARCHAR(50))
RETURNS BIT
BEGIN
DECLARE @wynik BIT;
SET @wynik = (SELECT is_konto_aktywne FROM bartoszbartniczak_dbadmin.Uzytkownicy WHERE uzytkownicy_email = @email_uzytkownika);
RETURN @wynik;
END;
GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[DziennikWydarzen]    Script Date: 2014-04-13 10:30:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[DziennikWydarzen](
	[id_dziennik_wydarzen] [int] IDENTITY(1,1) NOT NULL,
	[row_date] [datetime2](7) NOT NULL,
	[zrodlo] [nvarchar](200) NULL,
	[opis] [ntext] NULL,
	[stack_trace] [ntext] NULL,
 CONSTRAINT [PK_DziennikWydarzen] PRIMARY KEY CLUSTERED 
(
	[id_dziennik_wydarzen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[GrupyRobocze]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[GrupyRobocze](
	[id_grupy_robocze] [int] IDENTITY(1,1) NOT NULL,
	[uzytkownicy_email] [nvarchar](50) NOT NULL,
	[nazwa] [nvarchar](50) NOT NULL,
	[row_date] [datetime2](7) NOT NULL CONSTRAINT [DF_GrupyRobocze_row_date]  DEFAULT (getdate()),
	[is_aktywna] [bit] NOT NULL CONSTRAINT [DF_GrupyRobocze_is_aktywna]  DEFAULT ((1)),
 CONSTRAINT [PK_GrupyRobocze] PRIMARY KEY CLUSTERED 
(
	[id_grupy_robocze] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[Kolejka_Emaili]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[Kolejka_Emaili](
	[id_kolejka_emaili] [int] IDENTITY(1,1) NOT NULL,
	[od] [nvarchar](50) NOT NULL,
	[do] [nvarchar](50) NOT NULL,
	[temat] [nvarchar](150) NOT NULL,
	[tresc] [ntext] NOT NULL,
	[wersja] [nvarchar](10) NOT NULL,
	[wyslany] [bit] NOT NULL,
	[data_kolejki] [datetime2](7) NOT NULL,
	[data_wyslania] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow](
	[id_grupy_uzytkownikow] [int] IDENTITY(1,1) NOT NULL,
	[id_projekty] [int] NOT NULL,
	[nazwa_grupy] [nvarchar](50) NOT NULL,
	[is_usunieta] [bit] NOT NULL,
 CONSTRAINT [PK_ProjektGrupyUzytkownikow] PRIMARY KEY CLUSTERED 
(
	[id_grupy_uzytkownikow] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_ProjektGrupyUzytkownikow] UNIQUE NONCLUSTERED 
(
	[id_projekty] ASC,
	[nazwa_grupy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[ProjektUzytkownicyGrupy]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[ProjektUzytkownicyGrupy](
	[id_grupy_uzytkownikow] [int] NOT NULL,
	[email_uzytkownika] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProjektUzytkownicyGrupy] PRIMARY KEY CLUSTERED 
(
	[id_grupy_uzytkownikow] ASC,
	[email_uzytkownika] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[Projekty]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[Projekty](
	[id_projekty] [int] IDENTITY(1,1) NOT NULL,
	[id_menager_projektu] [nvarchar](50) NOT NULL,
	[id_grupy_robocze] [int] NOT NULL,
	[nazwa_projektu] [nvarchar](50) NOT NULL,
	[id_scrum_master] [nvarchar](50) NOT NULL,
	[is_aktywny] [nchar](10) NOT NULL CONSTRAINT [DF_Projekty_is_aktywny]  DEFAULT ((1)),
	[row_date] [datetime2](7) NOT NULL CONSTRAINT [DF_Projekty_row_date]  DEFAULT (getdate()),
 CONSTRAINT [PK_Projekty] PRIMARY KEY CLUSTERED 
(
	[id_projekty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[ProjektyUzytkownicy]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[ProjektyUzytkownicy](
	[email_uzytkownicy] [nvarchar](50) NOT NULL,
	[id_projekty] [int] NOT NULL,
 CONSTRAINT [PK_ProjektyUzytkownicy] PRIMARY KEY CLUSTERED 
(
	[email_uzytkownicy] ASC,
	[id_projekty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[ProjektZaproszenia]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia](
	[id_projekty] [int] NOT NULL,
	[id_zapraszajacego] [nvarchar](50) NOT NULL,
	[id_zapraszanego] [nvarchar](50) NOT NULL,
	[token] [nchar](32) NOT NULL,
	[is_aktywny] [bit] NOT NULL CONSTRAINT [DF_ProjektyZaproszenia_is_aktywny]  DEFAULT ((1)),
	[row_date] [datetime2](7) NOT NULL CONSTRAINT [DF_ProjektyZaproszenia_row_date]  DEFAULT (getdate()),
 CONSTRAINT [PK_ProjektyZaproszenia] PRIMARY KEY CLUSTERED 
(
	[id_projekty] ASC,
	[id_zapraszanego] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[Sprinty]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[Sprinty](
	[id_sprinty] [int] IDENTITY(1,1) NOT NULL,
	[id_projekty] [int] NOT NULL,
	[data_rozpoczecia] [datetime2](7) NULL,
	[data_zakonczenia] [datetime2](7) NULL,
	[data_deadline] [datetime2](7) NULL,
	[row_date] [datetime2](7) NOT NULL,
	[is_usuniety] [bit] NOT NULL,
 CONSTRAINT [PK_Sprinty] PRIMARY KEY CLUSTERED 
(
	[id_sprinty] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[TokenyRejestracyjne]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[TokenyRejestracyjne](
	[uzytkownicy_email] [nvarchar](50) NOT NULL,
	[token] [nchar](32) NOT NULL,
	[row_date] [datetime2](7) NOT NULL CONSTRAINT [DF_TokenyRejestracyjne_row_date]  DEFAULT (getdate()),
	[is_aktywny] [bit] NOT NULL CONSTRAINT [DF_TokenyRejestracyjne_czy_aktywny]  DEFAULT ((1))
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[Uzytkownicy]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[Uzytkownicy](
	[uzytkownicy_email] [nvarchar](50) NOT NULL,
	[imie] [nvarchar](50) NOT NULL,
	[nazwisko] [nvarchar](50) NOT NULL,
	[haslo] [nchar](256) NOT NULL,
	[is_konto_aktywne] [bit] NOT NULL CONSTRAINT [DF_Uzytkownicy_is_konto_aktywne]  DEFAULT ((0)),
	[row_date] [datetime2](7) NOT NULL CONSTRAINT [DF_Uzytkownicy_row_date]  DEFAULT (getdate()),
 CONSTRAINT [PK_Uzytkownicy] PRIMARY KEY CLUSTERED 
(
	[uzytkownicy_email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[Zadania]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[Zadania](
	[id_zadania] [int] IDENTITY(1,1) NOT NULL,
	[id_sprinty] [int] NOT NULL,
	[id_zadania_typy] [nvarchar](10) NOT NULL,
	[tytul] [varchar](100) NOT NULL,
	[opis] [ntext] NOT NULL,
	[is_usuniety] [bit] NOT NULL,
	[email_dodajacego] [nvarchar](50) NOT NULL,
	[row_date] [datetime2](7) NOT NULL,
	[priorytet] [int] NOT NULL,
	[email_przydzielony_uzytkownik] [nvarchar](50) NULL,
	[id_przydzielonej_grupy] [int] NULL,
	[zadanie_nadrzedne] [int] NULL,
	[data_zakonczenia] [datetime2](7) NULL,
 CONSTRAINT [PK_Zadania] PRIMARY KEY CLUSTERED 
(
	[id_zadania] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [bartoszbartniczak_dbadmin].[ZadaniaTypy]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bartoszbartniczak_dbadmin].[ZadaniaTypy](
	[id_zadania_typy] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_ZadaniaTypy] PRIMARY KEY CLUSTERED 
(
	[id_zadania_typy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TokenyRejestracyjne]    Script Date: 2014-04-13 10:30:52 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TokenyRejestracyjne] ON [bartoszbartniczak_dbadmin].[TokenyRejestracyjne]
(
	[uzytkownicy_email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[DziennikWydarzen] ADD  CONSTRAINT [DF_DziennikWydarzen_dziennik_data]  DEFAULT (getdate()) FOR [row_date]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Kolejka_Emaili] ADD  CONSTRAINT [DF_Kolejka_Emaili_wersja]  DEFAULT (N'html') FOR [wersja]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Kolejka_Emaili] ADD  CONSTRAINT [DF_Kolejka_Emaili_wyslany]  DEFAULT ((0)) FOR [wyslany]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Kolejka_Emaili] ADD  CONSTRAINT [DF_Kolejka_Emaili_data_kolejki]  DEFAULT (getdate()) FOR [data_kolejki]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow] ADD  CONSTRAINT [DF_ProjektGrupyUzytkownikow_is_usunieta]  DEFAULT ((0)) FOR [is_usunieta]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Sprinty] ADD  CONSTRAINT [DF_Sprinty_row_date]  DEFAULT (getdate()) FOR [row_date]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Sprinty] ADD  CONSTRAINT [DF_Sprinty_status]  DEFAULT ((0)) FOR [is_usuniety]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] ADD  CONSTRAINT [DF_Zadania_is_usuniety]  DEFAULT ((0)) FOR [is_usuniety]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] ADD  CONSTRAINT [DF_Zadania_row_date]  DEFAULT (getdate()) FOR [row_date]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] ADD  CONSTRAINT [DF_Zadania_priorytet]  DEFAULT ((0)) FOR [priorytet]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[GrupyRobocze]  WITH CHECK ADD  CONSTRAINT [FK_GrupyRobocze_Uzytkownicy] FOREIGN KEY([uzytkownicy_email])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[GrupyRobocze] CHECK CONSTRAINT [FK_GrupyRobocze_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow]  WITH CHECK ADD  CONSTRAINT [FK_ProjektGrupyUzytkownikow_Projekty] FOREIGN KEY([id_projekty])
REFERENCES [bartoszbartniczak_dbadmin].[Projekty] ([id_projekty])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow] CHECK CONSTRAINT [FK_ProjektGrupyUzytkownikow_Projekty]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektUzytkownicyGrupy]  WITH CHECK ADD  CONSTRAINT [FK_ProjektUzytkownicyGrupy_ProjektGrupyUzytkownikow] FOREIGN KEY([id_grupy_uzytkownikow])
REFERENCES [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow] ([id_grupy_uzytkownikow])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektUzytkownicyGrupy] CHECK CONSTRAINT [FK_ProjektUzytkownicyGrupy_ProjektGrupyUzytkownikow]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektUzytkownicyGrupy]  WITH CHECK ADD  CONSTRAINT [FK_ProjektUzytkownicyGrupy_Uzytkownicy] FOREIGN KEY([email_uzytkownika])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektUzytkownicyGrupy] CHECK CONSTRAINT [FK_ProjektUzytkownicyGrupy_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Projekty]  WITH CHECK ADD  CONSTRAINT [FK_Projekty_GrupyRobocze] FOREIGN KEY([id_grupy_robocze])
REFERENCES [bartoszbartniczak_dbadmin].[GrupyRobocze] ([id_grupy_robocze])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Projekty] CHECK CONSTRAINT [FK_Projekty_GrupyRobocze]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Projekty]  WITH CHECK ADD  CONSTRAINT [FK_Projekty_Uzytkownicy] FOREIGN KEY([id_menager_projektu])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Projekty] CHECK CONSTRAINT [FK_Projekty_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Projekty]  WITH CHECK ADD  CONSTRAINT [FK_Projekty_Uzytkownicy1] FOREIGN KEY([id_scrum_master])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Projekty] CHECK CONSTRAINT [FK_Projekty_Uzytkownicy1]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektyUzytkownicy]  WITH CHECK ADD  CONSTRAINT [FK_ProjektyUzytkownicy_Projekty] FOREIGN KEY([id_projekty])
REFERENCES [bartoszbartniczak_dbadmin].[Projekty] ([id_projekty])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektyUzytkownicy] CHECK CONSTRAINT [FK_ProjektyUzytkownicy_Projekty]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektyUzytkownicy]  WITH CHECK ADD  CONSTRAINT [FK_ProjektyUzytkownicy_Uzytkownicy] FOREIGN KEY([email_uzytkownicy])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektyUzytkownicy] CHECK CONSTRAINT [FK_ProjektyUzytkownicy_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia]  WITH CHECK ADD  CONSTRAINT [FK_ProjektyZaproszenia_Projekty] FOREIGN KEY([id_projekty])
REFERENCES [bartoszbartniczak_dbadmin].[Projekty] ([id_projekty])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia] CHECK CONSTRAINT [FK_ProjektyZaproszenia_Projekty]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia]  WITH CHECK ADD  CONSTRAINT [FK_ProjektyZaproszenia_Uzytkownicy] FOREIGN KEY([id_zapraszajacego])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia] CHECK CONSTRAINT [FK_ProjektyZaproszenia_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia]  WITH CHECK ADD  CONSTRAINT [FK_ProjektyZaproszenia_Uzytkownicy1] FOREIGN KEY([id_zapraszanego])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[ProjektZaproszenia] CHECK CONSTRAINT [FK_ProjektyZaproszenia_Uzytkownicy1]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Sprinty]  WITH CHECK ADD  CONSTRAINT [FK_Sprinty_Projekty] FOREIGN KEY([id_projekty])
REFERENCES [bartoszbartniczak_dbadmin].[Projekty] ([id_projekty])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Sprinty] CHECK CONSTRAINT [FK_Sprinty_Projekty]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[TokenyRejestracyjne]  WITH CHECK ADD  CONSTRAINT [FK_TokenyRejestracyjne_Uzytkownicy] FOREIGN KEY([uzytkownicy_email])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[TokenyRejestracyjne] CHECK CONSTRAINT [FK_TokenyRejestracyjne_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania]  WITH CHECK ADD  CONSTRAINT [FK_Zadania_ProjektGrupyUzytkownikow] FOREIGN KEY([id_przydzielonej_grupy])
REFERENCES [bartoszbartniczak_dbadmin].[ProjektGrupyUzytkownikow] ([id_grupy_uzytkownikow])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] CHECK CONSTRAINT [FK_Zadania_ProjektGrupyUzytkownikow]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania]  WITH CHECK ADD  CONSTRAINT [FK_Zadania_Sprinty] FOREIGN KEY([id_sprinty])
REFERENCES [bartoszbartniczak_dbadmin].[Sprinty] ([id_sprinty])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] CHECK CONSTRAINT [FK_Zadania_Sprinty]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania]  WITH CHECK ADD  CONSTRAINT [FK_Zadania_Uzytkownicy] FOREIGN KEY([email_dodajacego])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] CHECK CONSTRAINT [FK_Zadania_Uzytkownicy]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania]  WITH CHECK ADD  CONSTRAINT [FK_Zadania_Uzytkownicy1] FOREIGN KEY([email_przydzielony_uzytkownik])
REFERENCES [bartoszbartniczak_dbadmin].[Uzytkownicy] ([uzytkownicy_email])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] CHECK CONSTRAINT [FK_Zadania_Uzytkownicy1]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania]  WITH CHECK ADD  CONSTRAINT [FK_Zadania_Zadania] FOREIGN KEY([zadanie_nadrzedne])
REFERENCES [bartoszbartniczak_dbadmin].[Zadania] ([id_zadania])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] CHECK CONSTRAINT [FK_Zadania_Zadania]
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania]  WITH CHECK ADD  CONSTRAINT [FK_Zadania_ZadaniaTypy] FOREIGN KEY([id_zadania_typy])
REFERENCES [bartoszbartniczak_dbadmin].[ZadaniaTypy] ([id_zadania_typy])
GO
ALTER TABLE [bartoszbartniczak_dbadmin].[Zadania] CHECK CONSTRAINT [FK_Zadania_ZadaniaTypy]
GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[aktywacjaKonta]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[aktywacjaKonta](
@email_uzytkownika nvarchar(50),
@token nchar(40)
) AS
BEGIN

DECLARE @wynik_funkcji AS BIT;

BEGIN TRANSACTION T1;
UPDATE Uzytkownicy SET is_konto_aktywne = 1
WHERE uzytkownicy_email in 
	(SELECT uzytkownicy_email 
	FROM TokenyRejestracyjne 
	WHERE uzytkownicy_email = @email_uzytkownika 
	AND token = @token 
	AND is_aktywny = 1);

SET @wynik_funkcji = dbo.funcCzyKontoAktywne( @email_uzytkownika );
IF  @wynik_funkcji = 1
	BEGIN
		COMMIT TRANSACTION T1;
		UPDATE TokenyRejestracyjne SET is_aktywny = 0 where uzytkownicy_email = @email_uzytkownika;
		EXEC czyKontoAktywne @email_uzytkownika = @email_uzytkownika;
	END
ELSE
	BEGIN
		ROLLBACK TRANSACTION T1;
		EXEC czyKontoAktywne @email_uzytkownika = @email_uzytkownika;
	END

END;

GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[czyKontoAktywne]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[czyKontoAktywne](
@email_uzytkownika nvarchar(50)
) AS
BEGIN
SELECT is_konto_aktywne AS konto_aktywne FROM Uzytkownicy WHERE uzytkownicy_email = @email_uzytkownika;
END;
GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[listaGrupRoboczych]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[listaGrupRoboczych](
@email_uzytkownika nvarchar(50)
)AS
BEGIN

SELECT id_grupy_robocze, nazwa
FROM GrupyRobocze
WHERE uzytkownicy_email = @email_uzytkownika
AND is_aktywna = 1;

END;
GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[listaProjektow]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[listaProjektow](
@email_uzytkownika NVARCHAR(50)
) AS
BEGIN
	SELECT * FROM Projekty WHERE id_menager_projektu = @email_uzytkownika AND is_aktywny = 1;
END;
GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[logowanie]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[logowanie](
@email_uzytkownika nvarchar(50),
@haslo nchar(256)
) AS
BEGIN

SET @email_uzytkownika = LTRIM(RTRIM(@email_uzytkownika));

SELECT COUNT(*) as status_logowania FROM Uzytkownicy
WHERE uzytkownicy_email = @email_uzytkownika and haslo = @haslo and is_konto_aktywne = 1;

END;
GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[profilUzytkownika]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[profilUzytkownika](
@email_uzytkownika nvarchar(50)
) AS
BEGIN
SELECT uzytkownicy_email, imie, nazwisko
FROM Uzytkownicy
WHERE uzytkownicy_email = @email_uzytkownika;
END;

GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[rejestracjaUzytkownika]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[rejestracjaUzytkownika](
@email_uzytkownika nvarchar(50),
@imie nvarchar(50),
@nazwisko nvarchar(50),
@haslo nchar(256)
)
AS
BEGIN

SET @email_uzytkownika = LTRIM(RTRIM( @email_uzytkownika ) );
SET @imie = LTRIM(RTRIM(@imie));
SET @nazwisko = LTRIM(RTRIM(@nazwisko));

INSERT INTO Uzytkownicy (uzytkownicy_email, imie, nazwisko, haslo)
VALUES ( @email_uzytkownika, @imie, @nazwisko, @haslo );

exec utworzTokenRejestracyjny @email_uzytkownika = @email_uzytkownika;

END;

GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[utworzGrupeRobocza]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[utworzGrupeRobocza](
@email_uzytkownika nvarchar(50),
@nazwa_grupy nvarchar(50)
) AS
BEGIN

SET @email_uzytkownika = LTRIM(RTRIM(@email_uzytkownika));
SET @nazwa_grupy = LTRIM(RTRIM(@nazwa_grupy));

INSERT INTO GrupyRobocze (uzytkownicy_email, nazwa)
VALUES (@email_uzytkownika, @nazwa_grupy);

SELECT SCOPE_IDENTITY() as id_grupy_robocze;

END;

GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[utworzProjekt]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[utworzProjekt](
@email_uzytkownika NVARCHAR(50),
@id_grupy_robocze INT,
@nazwa_projektu NVARCHAR(50),
@scrum_master_id NVARCHAR(50)
) AS
BEGIN

DECLARE @id_projektu INT;
--DECLARE @tokenZaproszenieScrumMaster NCHAR(32);

INSERT INTO Projekty ( id_menager_projektu, id_grupy_robocze, nazwa_projektu, id_scrum_master )
VALUES ( @email_uzytkownika, @id_grupy_robocze, @nazwa_projektu, @scrum_master_id );

SET @id_projektu = (SELECT SCOPE_IDENTITY() as id_projektu);

exec zaproszenieDoProjektu @email_uzytkownika=@email_uzytkownika, @email_zapraszanego=@scrum_master_id, @id_projektu=@id_projektu;

SELECT @id_projektu as id_projektu;

END

GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[utworzTokenRejestracyjny]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[utworzTokenRejestracyjny](
@email_uzytkownika nvarchar(50)
)
AS
BEGIN
INSERT INTO TokenyRejestracyjne (uzytkownicy_email, token)
VALUES ( @email_uzytkownika, CONVERT(VARCHAR(32), HashBytes('MD5', cast(rand() as char(10))), 2));

SELECT token FROM TokenyRejestracyjne
WHERE uzytkownicy_email = @email_uzytkownika and is_aktywny = 1;

END;
GO
/****** Object:  StoredProcedure [bartoszbartniczak_dbadmin].[zaproszenieDoProjektu]    Script Date: 2014-04-13 10:30:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [bartoszbartniczak_dbadmin].[zaproszenieDoProjektu](
@email_uzytkownika NVARCHAR(50),
@email_zapraszanego NVARCHAR(50),
@id_projektu INT
)AS
BEGIN

INSERT INTO ProjektyZaproszenia (id_projekty, id_zapraszajacego, id_zapraszanego, token )
VALUES (@id_projektu, @email_uzytkownika, @email_zapraszanego, CONVERT(VARCHAR(32), HashBytes('MD5', cast(rand() as char(10))), 2));

SELECT token FROM ProjektyZaproszenia
WHERE id_projekty = @id_projektu
AND id_zapraszajacego = @email_uzytkownika
AND id_zapraszanego = @email_zapraszanego;

END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela zawiera grupy użytkowników wewnątrz projektu np. Database Administrator, Developer, Front-end itp.' , @level0type=N'SCHEMA',@level0name=N'bartoszbartniczak_dbadmin', @level1type=N'TABLE',@level1name=N'ProjektGrupyUzytkownikow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unikalna Nazwa w ramach projektu.' , @level0type=N'SCHEMA',@level0name=N'bartoszbartniczak_dbadmin', @level1type=N'TABLE',@level1name=N'ProjektGrupyUzytkownikow', @level2type=N'CONSTRAINT',@level2name=N'IX_ProjektGrupyUzytkownikow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela zawiera użytkowników grupy wewnątrz projektu.' , @level0type=N'SCHEMA',@level0name=N'bartoszbartniczak_dbadmin', @level1type=N'TABLE',@level1name=N'ProjektUzytkownicyGrupy'
GO
USE [master]
GO
ALTER DATABASE [bartoszbartniczak_scrum4u_production] SET  READ_WRITE 
GO

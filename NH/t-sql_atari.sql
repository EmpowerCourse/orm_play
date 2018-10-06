USE [master]
GO

CREATE DATABASE [atari] ON  PRIMARY ( NAME = N'atari', FILENAME = N'C:\DB\atari.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB ) LOG ON 
( NAME = N'atari_log', FILENAME = N'C:\DB\atari_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
USE atari
GO

create table dbo.Game (
Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
Name varchar(50) NOT NULL
)
GO

create table dbo.Player (
Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
Initials varchar(25) NOT NULL,
CreatedAt datetime NOT NULL DEFAULT(GETUTCDATE())
)
GO

create table dbo.HighScore (
Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
PlayerId int NOT NULL FOREIGN KEY REFERENCES Player(Id),
GameId int NOT NULL FOREIGN KEY REFERENCES Game(Id),
Score int NOT NULL,
OccurredAt datetime NOT NULL DEFAULT(GETUTCDATE())
)
GO

create table dbo.Contest (
Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
GameId int NOT NULL FOREIGN KEY REFERENCES Game(Id),
OccurredAt datetime NOT NULL DEFAULT(GETUTCDATE())
)
GO

create table dbo.Contestant (
Id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
ContestId int NOT NULL FOREIGN KEY REFERENCES Contest(Id),
PlayerId int NOT NULL FOREIGN KEY REFERENCES Player(Id),
Score int NOT NULL
)
GO

INSERT INTO dbo.Game (Name) VALUES ('Centipede'), ('Space Invaders'), ('Pong'), ('Pac-Man'), ('Q*bert'), ('DigDug'), ('Donkey Kong');
INSERT INTO dbo.Player (Initials) VALUES ('AT'),('TT'),('MC'),('BA'),('BG'),('SG'),('AW'),('SH'),('HT');

-- generate a few thousand initials
WITH CharacterIndex as (
  SELECT 1 as i
  UNION ALL
  SELECT x.i + 1 as i
  FROM CharacterIndex x
  WHERE x.i <25
)
INSERT INTO dbo.Player (Initials)
SELECT TOP 10000 CONCAT(CHAR(65+a.i),CHAR(65+b.i),CHAR(65+c.i)) as Initials
FROM CharacterIndex a, CharacterIndex b, CharacterIndex c
ORDER BY NEWID();

-- generate random game-association high scores for the S2 team
DECLARE @HighRollers TABLE (Seq int IDENTITY(1,1) NOT NULL, PlayerId int NOT NULL);

INSERT INTO @HighRollers(PlayerId)
SELECT p.Id
FROM dbo.Player p 
WHERE Id <= 9
ORDER BY NEWID();

INSERT INTO dbo.HighScore (PlayerId, GameId, Score)
SELECT h.PlayerId, g.Id, ((ABS(CHECKSUM(NEWID())) / 10000) / 100) * 100
FROM dbo.Game g INNER JOIN @HighRollers h ON g.Id = h.Seq;

INSERT INTO dbo.Contest (GameId)
SELECT g.Id
FROM dbo.Game g, sysobjects
ORDER BY NEWID();

DECLARE @ContestantCount int = (SELECT COUNT(*) FROM dbo.Contest) * 2
DECLARE @Contestants TABLE (Seq int IDENTITY(1,1) NOT NULL, PlayerId int NOT NULL);

INSERT INTO @Contestants(PlayerId)
SELECT TOP (@ContestantCount) p.Id
FROM dbo.Player p 
ORDER BY NEWID();

INSERT INTO dbo.Contestant (ContestId, PlayerId, Score)
-- player 1
SELECT c.Id, x.PlayerId, ((ABS(CHECKSUM(NEWID())) / 1000000) / 10) * 10
FROM dbo.Contest c INNER JOIN @Contestants x ON c.Id = x.Seq
UNION ALL
-- player 2
SELECT c.Id, x.PlayerId, ((ABS(CHECKSUM(NEWID())) / 1000000) / 10) * 10
FROM dbo.Contest c INNER JOIN @Contestants x ON (c.Id * 2) = x.Seq
-- select t.id, count(*) as playercnt from contestant x inner join contest t on t.id = x.contestid group by t.id
GO

CREATE PROCEDURE dbo.spGetContestResult
@ContestId int
as
SELECT t.Id as ContestId, 
	g.Name as GameName, 
	p.Initials as Player, 
	c.Score
FROM dbo.Contestant c INNER JOIN dbo.Contest t ON t.Id = c.ContestId
	INNER JOIN dbo.Game g ON g.Id = t.GameId
	INNER JOIN dbo.Player p ON p.Id = c.PlayerId
WHERE t.Id = @ContestId
ORDER BY c.Score DESC

GO
-- exec dbo.spGetContestResult 681

CREATE VIEW dbo.vwS2Games
AS
SELECT t.Id as ContestId, 
	g.Name as GameName, 
	p.Initials as Player, 
	c.Score
FROM dbo.Contestant c INNER JOIN dbo.Contest t ON t.Id = c.ContestId
	INNER JOIN dbo.Game g ON g.Id = t.GameId
	INNER JOIN dbo.Player p ON p.Id = c.PlayerId
WHERE EXISTS(
	SELECT 1
	FROM dbo.Contestant x
	WHERE x.PlayerId <= 9
		AND x.ContestId = t.Id
	)

GO

SELECT ContestId, GameName, Player, Score FROM dbo.vwS2Games ORDER BY ContestId, Score DESC;

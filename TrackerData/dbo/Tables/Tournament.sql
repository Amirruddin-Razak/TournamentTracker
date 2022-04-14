CREATE TABLE [dbo].[Tournament]
(
	  Id INT IDENTITY NOT NULL PRIMARY KEY,
	  TournamentName NVARCHAR(50) NOT NULL,
	  EntreeFee MONEY NOT NULL,
	  Active BIT NOT NULL,
)

CREATE TABLE [dbo].[Matchup]
(
	  Id INT IDENTITY NOT NULL PRIMARY KEY,
	  WinnerId INT,
	  TournamentId INT NOT NULL,
	  MatchupRound INT NOT NULL,
	  CONSTRAINT FK_Matchup_TournamentId FOREIGN KEY (TournamentId) REFERENCES Tournament(Id),
	  CONSTRAINT FK_Matchup_TeamId FOREIGN KEY (WinnerId) REFERENCES Team(Id),
)

CREATE TABLE [dbo].[TournamentEntry]
(
      Id INT IDENTITY NOT NULL PRIMARY KEY,
      TournamentId INT NOT NULL,
      TeamId INT NOT NULL,
      CONSTRAINT FK_TournamentEntry_TournamentId FOREIGN KEY (TournamentId) REFERENCES Tournament(Id),
      CONSTRAINT FK_TournamentEntry_TeamId FOREIGN KEY (TeamId) REFERENCES Team(Id)
)

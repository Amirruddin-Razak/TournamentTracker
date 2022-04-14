CREATE TABLE [dbo].[MatchupEntry]
(
      Id INT IDENTITY NOT NULL PRIMARY KEY,
      TeamId INT,
      MatchupId INT NOT NULL,
      ParentMatchupId INT,
      Score INT,
      CONSTRAINT FK_MatchupEntry_TeamId FOREIGN KEY (TeamId) REFERENCES Team(Id),
      CONSTRAINT FK_MatchupEntry_MatchupId FOREIGN KEY (MatchupId) REFERENCES Matchup(Id),
      CONSTRAINT FK_MatchupEntry_ParentMatchupId FOREIGN KEY (ParentMatchupId) REFERENCES Matchup(Id)
)

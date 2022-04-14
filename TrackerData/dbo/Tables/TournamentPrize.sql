CREATE TABLE [dbo].[TournamentPrize]
(
	  Id INT IDENTITY NOT NULL PRIMARY KEY,
      TournamentId INT NOT NULL,
      PrizeId INT NOT NULL,
      CONSTRAINT FK_TournamentPrize_TournamentId FOREIGN KEY (TournamentId) REFERENCES Tournament(Id),
      CONSTRAINT FK_TournamentPrize_PrizeId FOREIGN KEY (PrizeId) REFERENCES Prize(Id)
)

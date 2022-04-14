CREATE TABLE [dbo].[Prize]
(
	  Id INT IDENTITY NOT NULL PRIMARY KEY,
	  PlaceNumber INT NOT NULL,
	  PrizeName NVARCHAR(20) NOT NULL,
	  PrizeAmount MONEY NOT NULL,
	  PrizePercentage FLOAT NOT NULL
)

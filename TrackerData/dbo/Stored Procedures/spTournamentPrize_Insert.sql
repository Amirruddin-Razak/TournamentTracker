﻿CREATE PROCEDURE [dbo].[spTournamentPrize_Insert] 
	-- Add the parameters for the stored procedure here
	@TournamentId INT,
	@PrizeId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.TournamentPrize (TournamentId, PrizeId)
	VALUES (@TournamentId, @PrizeId);
END
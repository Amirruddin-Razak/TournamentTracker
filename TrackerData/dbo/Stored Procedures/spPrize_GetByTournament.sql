-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 10 March 2022
-- Description:	Get all prize for a given tournament.
-- =============================================
CREATE PROCEDURE [dbo].[spPrize_GetByTournament]
	-- Add the parameters for the stored procedure here
	@TournamentId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT p.*
	FROM dbo.Prize p
	INNER JOIN dbo.TournamentPrize t ON p.Id = t.PrizeId
	WHERE t.TournamentId = @TournamentId;
END
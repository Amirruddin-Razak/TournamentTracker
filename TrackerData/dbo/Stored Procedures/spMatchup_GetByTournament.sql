-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 10 March 2022
-- Description:	Get all matchup for a given tournament.
-- =============================================
CREATE PROCEDURE [dbo].[spMatchup_GetByTournament]
	-- Add the parameters for the stored procedure here
	@TournamentId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT m.Id, m.MatchupRound, m.TournamentId, m.WinnerId
	FROM dbo.Matchup m
	WHERE TournamentId = @TournamentId
	ORDER BY MatchupRound;
END

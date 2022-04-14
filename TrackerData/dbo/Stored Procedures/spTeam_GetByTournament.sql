-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 10 March 2022
-- Description:	Get all team for a given tournament.
-- =============================================
CREATE PROCEDURE [dbo].[spTeam_GetByTournament]
	-- Add the parameters for the stored procedure here
	@TournamentId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t.Id, t.TeamName
	FROM dbo.Team t
	INNER JOIN dbo.TournamentEntry e ON t.Id = e.TeamId
	WHERE e.TournamentId = @TournamentId;
END
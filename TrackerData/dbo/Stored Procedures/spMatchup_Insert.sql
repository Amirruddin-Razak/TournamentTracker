﻿CREATE PROCEDURE [dbo].[spMatchup_Insert] 
	-- Add the parameters for the stored procedure here
	@Id INT = 0 OUTPUT,
	@TournamentId INT,
	@MatchupRound INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.MatchUp (TournamentId, MatchUpRound)
	VALUES (@TournamentId, @MatchupRound);

	SELECT @Id = SCOPE_IDENTITY();
END
CREATE PROCEDURE [dbo].[spTournamentEntry_Insert] 
	-- Add the parameters for the stored procedure here
	@TournamentId INT,
	@TeamId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.TournamentEntry (TournamentId, TeamId)
	VALUES (@TournamentId, @TeamId);
END
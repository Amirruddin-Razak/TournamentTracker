CREATE PROCEDURE [dbo].[spTournament_Insert] 
	-- Add the parameters for the stored procedure here
	@Id INT = 0 OUTPUT,
	@TournamentName NVARCHAR(50),
	@EntreeFee MONEY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.Tournament (TournamentName, EntreeFee, Active)
	VALUES (@TournamentName, @EntreeFee, 1);

	SELECT @Id = SCOPE_IDENTITY();
END
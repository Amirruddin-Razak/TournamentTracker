CREATE PROCEDURE [dbo].[spTournament_Complete]
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.Tournament
	SET Active = 0
	WHERE Id = @Id;
END
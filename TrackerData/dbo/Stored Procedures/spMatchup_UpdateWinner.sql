CREATE PROCEDURE [dbo].[spMatchup_UpdateWinner]
	-- Add the parameters for the stored procedure here
	@Id INT,
	@WinnerId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE dbo.Matchup
	SET WinnerId = @WinnerId
	WHERE Id = @Id;
END
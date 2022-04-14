CREATE PROCEDURE [dbo].[spMatchupEntry_Insert] 
	-- Add the parameters for the stored procedure here
	@Id INT = 0 OUTPUT,
	@TeamId INT,
	@MatchupId INT,
	@ParentMatchupId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.MatchUpEntry (TeamId, MatchupId, ParentMatchupId)
	VALUES (@TeamId, @MatchupId, @ParentMatchupId);

	SELECT @Id = SCOPE_IDENTITY();
END

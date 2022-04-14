CREATE PROCEDURE [dbo].[spTeamMember_Insert] 
	-- Add the parameters for the stored procedure here
	@PersonId INT,
	@TeamId INT,
	@Id INT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.TeamMember (PersonId, TeamId)
	VALUES (@PersonId, @TeamId);

	SELECT @Id = SCOPE_IDENTITY();
END
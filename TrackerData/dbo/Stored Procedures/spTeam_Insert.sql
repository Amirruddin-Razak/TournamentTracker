CREATE PROCEDURE [dbo].[spTeam_Insert] 
	-- Add the parameters for the stored procedure here
	@TeamName NVARCHAR(20),
	@Id INT = 0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.Team(TeamName) VALUES (@TeamName);

	SELECT @Id = SCOPE_IDENTITY();
END
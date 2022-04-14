-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 10 March 2022
-- Description:	Get all team member for a given team.
-- =============================================
CREATE PROCEDURE [dbo].[spTeamMember_GetByTeam]
	-- Add the parameters for the stored procedure here
	@TeamId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT p.Id, p.FirstName, p.LastName, p.EmailAddress, p.PhoneNumber
	FROM dbo.TeamMember m
	INNER JOIN dbo.Person p ON m.PersonId = p.Id
	WHERE TeamId = @TeamId;
END
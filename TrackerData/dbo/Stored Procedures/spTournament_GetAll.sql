-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 10 March 2022
-- Description:	Get all existing tournament for a given team.
-- =============================================
CREATE PROCEDURE [dbo].[spTournament_GetAll]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t.Id, t.TournamentName, t.EntreeFee, t.Active
	FROM dbo.Tournament t
	WHERE Active = 1;
END
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
	SELECT *
	FROM dbo.Tournament
	WHERE Active = 1;
END
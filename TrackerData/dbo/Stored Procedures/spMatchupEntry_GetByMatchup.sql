-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 10 March 2022
-- Description:	Get all matchup entry for a given matchup.
-- =============================================
CREATE PROCEDURE [dbo].[spMatchupEntry_GetByMatchup]
	-- Add the parameters for the stored procedure here
	@MatchupId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM dbo.MatchupEntry
	WHERE MatchupId = @MatchupId;
END

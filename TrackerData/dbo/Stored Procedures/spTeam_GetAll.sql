﻿CREATE PROCEDURE [dbo].[spTeam_GetAll] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t.Id, t.TeamName
	FROM dbo.Team t;
END
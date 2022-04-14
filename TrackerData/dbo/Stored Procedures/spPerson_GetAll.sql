CREATE PROCEDURE [dbo].[spPerson_GetAll]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT p.Id, p.FirstName, p.LastName, p.EmailAddress, p.PhoneNumber
	FROM dbo.Person p;
END
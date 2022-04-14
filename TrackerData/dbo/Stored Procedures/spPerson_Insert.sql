-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 16 March 2022
-- Description:	Insert new person into database and return Id
-- =============================================
CREATE PROCEDURE [dbo].[spPerson_Insert]
	-- Add the parameters for the stored procedure here
	@Id INT = 0 OUTPUT,
	@FirstName NVARCHAR(100), 
	@LastName NVARCHAR(100),
	@EmailAddress NVARCHAR(200),
	@PhoneNumber VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.Person (FirstName, LastName, EmailAddress, PhoneNumber)
	VALUES (@FirstName, @LastName, @EmailAddress, @PhoneNumber);

	SELECT @Id = SCOPE_IDENTITY();
END
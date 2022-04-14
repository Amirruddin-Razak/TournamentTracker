-- =============================================
-- Author:		Amirruddin Abdul Razak
-- Create date: 14 March 2022
-- Description:	Insert new prize into database and return the id
-- =============================================
CREATE PROCEDURE [dbo].[spPrize_Insert]
	-- Add the parameters for the stored procedure here
	@Id INT = 0 OUTPUT,
	@PlaceNumber INT,
	@PrizeName NVARCHAR(20),
	@PrizeAmount MONEY,
	@PrizePercentage FLOAT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.Prize(PlaceNumber, PrizeName, PrizeAmount, PrizePercentage) 
	VALUES (@PlaceNumber, @PrizeName, @PrizeAmount, @PrizePercentage);

	SELECT @Id = SCOPE_IDENTITY();
END
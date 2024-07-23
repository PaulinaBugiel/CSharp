CREATE PROCEDURE [dbo].[spPlantPositions_Insert]
	@Name nvarchar(50),
	@SunExpositionId int,
	@Id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.PlantPositions (Name, SunExpositionId)
	VALUES (@Name, @SunExpositionId);

	SELECT @Id = SCOPE_IDENTITY();
END

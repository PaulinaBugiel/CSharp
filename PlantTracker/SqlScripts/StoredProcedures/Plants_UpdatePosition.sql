CREATE PROCEDURE dbo.spPlants_UpdatePosition
	@PlantId int,
	@PositionId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE dbo.Plants
	SET PlantPositionId = @PositionId
	WHERE Id = @PlantId;
END
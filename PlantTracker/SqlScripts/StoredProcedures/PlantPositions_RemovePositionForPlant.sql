CREATE PROCEDURE dbo.spPlantPositions_RemovePositionForPlant
	@PlantId int,
	@PositionId int
AS
BEGIN
	delete from dbo.PlantsPositionsJunction
	where dbo.PlantsPositionsJunction.PlantId = @PlantId and dbo.PlantsPositionsJunction.PositionId = @PositionId;
END
GO
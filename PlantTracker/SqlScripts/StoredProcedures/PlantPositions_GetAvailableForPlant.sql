CREATE PROCEDURE dbo.spPlantPositions_GetGetAvailableForPlant 
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select p.Id, p.Name
	from dbo.PlantPositions p
	where p.Id not in 
	(
		select p.Id
		from dbo.PlantsPositionsJunction pp
		inner join dbo.PlantPositions p on pp.PositionId = p.Id
		where pp.PlantId = @PlantId
	)
END
GO

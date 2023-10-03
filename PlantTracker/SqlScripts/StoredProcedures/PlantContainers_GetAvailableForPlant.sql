CREATE PROCEDURE dbo.spPlantContainers_GetGetAvailableForPlant 
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select c.Id, c.Name
	from dbo.PlantContainers c
	where c.Id not in 
	(
		select c.Id
		from dbo.PlantsContainersJunction pc
		inner join dbo.PlantContainers t on pc.ContainerId = c.Id
		where pc.PlantId = @PlantId
	)
END
GO

CREATE PROCEDURE dbo.spPlantContainers_GetByPlantId 
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select c.Id, c.Name, c.Capacity
	from dbo.PlantsContainers pc
	inner join dbo.PlantContainers c on pc.ContainerId = c.Id
	where pc.PlantId = @PlantId;
END
GO

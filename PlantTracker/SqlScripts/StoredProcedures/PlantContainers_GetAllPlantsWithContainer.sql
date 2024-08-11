CREATE PROCEDURE [dbo].[spPlantContainers_GetAllPlantsWithContainer]
	@ContainerId int
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.PlantsContainersJunction pcj
	inner join dbo.Plants p on p.Id = pcj.PlantId
	where pcj.ContainerId = @ContainerId;
	
END

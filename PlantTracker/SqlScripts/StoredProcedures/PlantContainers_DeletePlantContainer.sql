CREATE PROCEDURE [dbo].[spPlantContainers_DeletePlantContainer]
	@ContainerId int
AS 
BEGIN
	SET NOCOUNT ON;
	delete from dbo.PlantsContainersJunction where ContainerId = @ContainerId;
	delete from dbo.PlantContainers where Id = @ContainerId;
END
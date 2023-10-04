SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantContainers_RemoveContainerForPlant
	-- Add the parameters for the stored procedure here
	@PlantId int,
	@ContainerId int
AS
BEGIN
	delete from dbo.PlantsContainersJunction
	where dbo.PlantsContainersJunction.PlantId = @PlantId and dbo.PlantsContainersJunction.ContainerId = @ContainerId;
END
GO
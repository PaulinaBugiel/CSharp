SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantContainers_AddContainerForPlant
	-- Add the parameters for the stored procedure here
	@PlantId int,
	@ContainerId int
AS
BEGIN
	insert into dbo.PlantsContainersJunction(PlantId, ContainerId)
	values (@PlantId, @ContainerId);
END
GO
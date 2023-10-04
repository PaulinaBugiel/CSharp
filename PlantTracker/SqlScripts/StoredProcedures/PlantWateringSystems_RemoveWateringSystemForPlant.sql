SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantWateringSystems_RemoveWateringSystemForPlant
	-- Add the parameters for the stored procedure here
	@PlantId int,
	@WateringSystemId int
AS
BEGIN
	delete from dbo.PlantsWateringSystemsJunction
	where dbo.PlantsWateringSystemsJunction.PlantId = @PlantId and dbo.PlantsWateringSystemsJunction.WateringSystemId = @WateringSystemId;
END
GO
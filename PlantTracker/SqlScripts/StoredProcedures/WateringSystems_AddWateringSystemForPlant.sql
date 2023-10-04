SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spWateringSystems_AddWateringSystemForPlant
	-- Add the parameters for the stored procedure here
	@PlantId int,
	@WateringSystemId int
AS
BEGIN
	insert into dbo.PlantsWateringSystemsJunction (PlantId, WateringSystemId)
	values (@PlantId, @WateringSystemId);
END
GO
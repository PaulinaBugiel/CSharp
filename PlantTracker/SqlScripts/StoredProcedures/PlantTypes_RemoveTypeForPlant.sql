SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantTypes_RemoveTypeForPlant
	-- Add the parameters for the stored procedure here
	@PlantId int,
	@TypeId int
AS
BEGIN
	delete from dbo.PlantsTypesJunction
	where dbo.PlantsTypesJunction.PlantId = @PlantId and dbo.PlantsTypesJunction.TypeId = @TypeId;
END
GO
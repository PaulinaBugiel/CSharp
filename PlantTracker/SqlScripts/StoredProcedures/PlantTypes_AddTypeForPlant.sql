SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantTypes_AddTypeForPlant
	-- Add the parameters for the stored procedure here
	@PlantId int,
	@TypeId int
AS
BEGIN
	insert into dbo.PlantsTypesJunction (PlantId, TypeId)
	values (@PlantId, @TypeId);
END
GO

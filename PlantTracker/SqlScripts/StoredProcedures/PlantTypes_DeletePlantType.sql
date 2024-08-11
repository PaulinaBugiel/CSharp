CREATE PROCEDURE [dbo].[spPlantTypes_DeletePlantType]
	@TypeId int
AS 
BEGIN
	SET NOCOUNT ON;
	delete from dbo.PlantsTypesJunction where TypeId = @TypeId;
	delete from dbo.PlantTypes where Id = @TypeId;
END
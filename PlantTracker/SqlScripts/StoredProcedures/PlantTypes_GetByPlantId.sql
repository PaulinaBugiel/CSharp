CREATE PROCEDURE dbo.spPlantTypes_GetByPlantId 
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select t.Id, t.Name
	from dbo.PlantsTypesJunction pt
	inner join dbo.PlantTypes t on pt.TypeId = t.Id
	where pt.PlantId = @PlantId;
END
GO

CREATE PROCEDURE [dbo].[spPlantTypes_GetAllPlantsWithType]
	@PlantTypeId int
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.PlantsTypesJunction ptj
	inner join dbo.Plants p on p.Id = ptj.PlantId
	where ptj.TypeId = @PlantTypeId;
	
END

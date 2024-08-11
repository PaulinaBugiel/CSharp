CREATE PROCEDURE [dbo].[spWateringSystems_GetAllPlantsWithSystem]
	@WateringSystemId int
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.PlantsWateringSystemsJunction wsj
	inner join dbo.Plants p on p.Id = wsj.PlantId
	where wsj.WateringSystemId = @WateringSystemId;
	
END

CREATE PROCEDURE [dbo].[spWateringSystems_GetByPlantId]
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.WateringSystemsJunction wsj
	inner join dbo.WateringSystems w on w.Id = wsj.WateringSystemId
	where wsj.PlantId = @PlantId;
	
END

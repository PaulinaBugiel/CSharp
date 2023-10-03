CREATE PROCEDURE dbo.spWateringSystems_GetGetAvailableForPlant 
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select w.Id, w.Name
	from dbo.WateringSystems w
	where w.Id not in 
	(
		select w.Id
		from dbo.PlantsWateringSystemsJunction pw
		inner join dbo.WateringSystems t on pw.WateringSystemId = w.Id
		where pw.PlantId = @PlantId
	)
END
GO

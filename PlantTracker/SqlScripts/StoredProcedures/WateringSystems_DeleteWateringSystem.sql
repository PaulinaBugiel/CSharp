CREATE PROCEDURE [dbo].[WateringSystems_DeleteWateringSystem]
	@WateringSystemId int
AS 
BEGIN
	SET NOCOUNT ON;
	delete from dbo.PlantsWateringSystemsJunction where WateringSystemId = @WateringSystemId;
	delete from dbo.WateringSystems where Id = @WateringSystemId;
END
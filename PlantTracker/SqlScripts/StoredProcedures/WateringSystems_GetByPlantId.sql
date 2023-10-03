CREATE PROCEDURE [dbo].[spWateringSystems_GetByPlantId]
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.WateringSystemsJunction wsj
	
END

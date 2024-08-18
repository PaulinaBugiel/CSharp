CREATE PROCEDURE [dbo].[spPlantPositions_GetAllPlantsWithPosition]
	@PositionId int
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.Plants p
	where p.PlantPositionId = @PositionId;
	
END

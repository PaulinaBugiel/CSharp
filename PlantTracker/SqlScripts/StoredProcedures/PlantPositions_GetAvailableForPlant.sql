CREATE PROCEDURE dbo.spPlantPositions_GetGetAvailableForPlant 
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

	select pp.Id, pp.Name, pp.SunExpositionId
	from dbo.PlantPositions pp
	where pp.Id not in 
	(
		select p.PlantPositionId
		from dbo.Plants p
		where p.Id = @PlantId
	)
END
GO



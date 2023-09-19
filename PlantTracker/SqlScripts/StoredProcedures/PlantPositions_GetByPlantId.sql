CREATE PROCEDURE dbo.spPlantPositions_GetByPlantId
	@PlantId int
AS
BEGIN
	SET NOCOUNT ON;

    select pp.Id, pp.Name, pp.SunExpositionId as Exposition
	from dbo.Plants p
	inner join dbo.PlantPositions pp on p.PlantPositionId = pp.Id
	where p.Id = @PlantId;
END
GO

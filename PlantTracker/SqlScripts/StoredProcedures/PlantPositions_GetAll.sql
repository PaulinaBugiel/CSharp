CREATE PROCEDURE [dbo].[spPlantPositions_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Name, SunExpositionId AS Exposition
	FROM dbo.PlantPositions;
END

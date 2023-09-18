CREATE PROCEDURE [dbo].[spPlantPositions_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.PlantPositions;
END

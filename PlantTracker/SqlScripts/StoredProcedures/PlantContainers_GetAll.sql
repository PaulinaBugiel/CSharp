CREATE PROCEDURE [dbo].[spPlantContainers_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.PlantContainers;
END

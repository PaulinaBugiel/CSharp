CREATE PROCEDURE [dbo].[spPlantTypes_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.PlantTypes;
END

CREATE PROCEDURE [dbo].[spPlants_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.[Id], p.[Name], p.[PlantingDate], p.[LastModificationDate]
	FROM dbo.Plants p;
END


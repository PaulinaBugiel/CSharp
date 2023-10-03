CREATE PROCEDURE [dbo].[spPlants_GetById]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.[Id], p.[Name], p.[PlantingDate], p.[LastModificationDate]
	FROM dbo.Plants p
	WHERE p.Id = @Id
END


CREATE PROCEDURE [dbo].[spPlantTypes_Insert]
	@Name nvarchar(50),
	@Id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.PlantTypes (Name)
	VALUES (@Name);

	SELECT @Id = SCOPE_IDENTITY();
END

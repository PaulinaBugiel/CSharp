CREATE PROCEDURE [dbo].[spPlantTypes_Insert]
	@Name nvarchar(50),
	@Capacity float,
	@Id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.PlantTypes (Name, Capacity)
	VALUES (@Name, @Capacity);

	SELECT @Id = SCOPE_IDENTITY();
END

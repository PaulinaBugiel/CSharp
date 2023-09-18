CREATE PROCEDURE [dbo].[spWateringSystems_Insert]
	@Name nvarchar(50),
	@Id int = 0 output
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.WateringSystems (Name)
	VALUES (@Name);

	SELECT @Id = SCOPE_IDENTITY();
END

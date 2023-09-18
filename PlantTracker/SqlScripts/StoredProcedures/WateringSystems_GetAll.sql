CREATE PROCEDURE [dbo].[spWateringSystems_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.WateringSystems;
END

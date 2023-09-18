CREATE PROCEDURE [dbo].[spSunExpositions_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.SunExpositions;
END

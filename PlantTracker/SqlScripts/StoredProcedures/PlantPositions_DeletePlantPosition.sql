CREATE PROCEDURE [dbo].[spPlantPosition_DeletePlantPosition]
	@PositionId int
AS 
BEGIN
	SET NOCOUNT ON;
	delete from dbo.PlantPositions where Id = @PositionId;
END
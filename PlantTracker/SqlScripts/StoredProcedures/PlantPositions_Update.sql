SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantPositions_Update
	@PositionId int,
	@Name nvarchar(50),
	@SunExpositionId int
AS
BEGIN
	update dbo.PlantPositions
	set Name = @Name, SunExpositionId = @SunExpositionId
	where Id = @PositionId;
END
GO
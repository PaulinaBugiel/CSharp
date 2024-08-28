SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spWateringSystems_Update
	@WateringSystemId int,
	@Name nvarchar(50)
AS
BEGIN
	update dbo.WateringSystems
	set Name = @Name
	where Id = @WateringSystemId;
END
GO
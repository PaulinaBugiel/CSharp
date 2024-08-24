SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spPlantContainers_Update
	@ContainerId int,
	@Name nvarchar(50),
	@Capacity float,
	@Color nvarchar(50)
AS
BEGIN
	update dbo.PlantContainers
	set Name = @Name, Capacity = @Capacity, Color = @Color
	where Id = @ContainerId;
END
GO